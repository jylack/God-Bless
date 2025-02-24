using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOD : MonoBehaviour
{
    [Header("신 전용 스킬 관리 객체")]
    public GodSkill godSkill;

    [Header("현재 계약된 헌터 목록")]
    public List<UnitCtrl> listUnit = new List<UnitCtrl>();

    [Header("현재 부여 할 수 있는 스킬 목록")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    private void Start()
    {
        if (godSkill == null)
        {
            godSkill = new GodSkill();
        }
    }

    // 모든 몬스터에 광역 디버프 적용
    public void AllDeBuffMonsters()
    {
        godSkill.AllDeBuff(this);
    }

    // 모든 헌터에 광역 버프 적용
    public void AllBuffUnits()
    {
        godSkill.AllBuff(this);
    }
}

[System.Serializable]
public class GodSkill
{
    // 1. 유닛 생성 (헌터 생성 및 랜덤 스킬 할당)
    public void UnitCreate(GOD god, string unitName)
    {
        GameObject unitGO = new GameObject(unitName);
        // UnitCtrl 추가
        UnitCtrl newUnit = unitGO.AddComponent<UnitCtrl>();
        //Hunter 컴포넌트도 추가
        if (newUnit.data != null)
        {
            unitGO.AddComponent<Hunter>();
        }

        // 데이터베이스에서 랜덤 스킬 할당
        DataBase db = GameObject.FindObjectOfType<DataBase>();
        if (db != null && db.listGrantSkill.Count > 0)
        {
            int randIndex = Random.Range(0, db.listGrantSkill.Count);
            GrantSkill randomSkill = db.listGrantSkill[randIndex];
            // Hunter 컴포넌트가 있다면 스킬 할당
            Hunter hunterComp = unitGO.GetComponent<Hunter>();
            if (hunterComp != null)
            {
                hunterComp.skill = randomSkill;
                randomSkill.targetUnit = hunterComp;
            }
        }

        god.listUnit.Add(newUnit);
        Debug.Log($"[GodSkill] 새로운 유닛 생성: {unitName}");
    }

    // 2. 특정 헌터에게 스킬 부여
    public void AllocateSkill(GOD god, Hunter targetUnit, GrantSkill skill)
    {
        if (targetUnit == null || skill == null)
        {
            Debug.LogWarning("[GodSkill] AllocateSkill 실패: 대상 또는 스킬이 null");
            return;
        }
        targetUnit.skill = skill;
        skill.targetUnit = targetUnit;
        if (!god.listGrantSkill.Contains(skill))
        {
            god.listGrantSkill.Add(skill);
        }
        Debug.Log($"[GodSkill] {targetUnit.gameObject.name}에게 스킬 [{skill.skillName}] 부여");
    }

    // 3. 광역 디버프: 모든 필드 몬스터의 스탯을 50%로 감소 (지속시간 10초)
    public void AllDeBuff(GOD god)
    {
        foreach (GameObject monsterObj in GameManager.Instance.listMonster)
        {
            UnitCtrl monster = monsterObj.GetComponent<UnitCtrl>();
            if (monster != null && monster.Type == UnitType.Monster)
            {
                monster.ApplyDebuff(0.5f, 10f);
            }
        }
        Debug.Log("[GodSkill] 모든 몬스터에게 광역 디버프 적용");
    }

    // 4. 광역 버프: 모든 헌터 유닛의 스탯을 150%로 증가 (지속시간 10초)
    public void AllBuff(GOD god)
    {
        foreach (UnitCtrl unit in god.listUnit)
        {
            if (unit != null && unit.Type == UnitType.Hunter)
            {
                unit.ApplyBuff(1.5f, 10f);
            }
        }
        Debug.Log("[GodSkill] 모든 유닛에게 광역 버프 적용");
    }

    // 5.UI에서 뽑기를 통해 스킬을 추가하는 메서드
    public void DrawSkill(GOD god)
    {
        DataBase db = GameObject.FindObjectOfType<DataBase>();
        if (db == null || db.listGrantSkill.Count == 0)
        {
            Debug.LogWarning("DrawSkill: 데이터베이스에 스킬 목록이 없습니다.");
            return;
        }
        int index = Random.Range(0, db.listGrantSkill.Count);
        GrantSkill drawnSkill = db.listGrantSkill[index];

        // 새로운 인스턴스로 복사하여 추가 (참조 공유 방지)
        GrantSkill newSkill = new GrantSkill();
        newSkill.skillName = drawnSkill.skillName;
        newSkill.usageLimit = drawnSkill.usageLimit;
        newSkill.isActive = drawnSkill.isActive;

        god.listGrantSkill.Add(newSkill);
        Debug.Log($"DrawSkill: {newSkill.skillName} 스킬을 뽑아 GOD의 스킬 목록에 추가했습니다.");
    }
}

[System.Serializable]
public class GrantSkill
{
    public string skillName;      // 스킬 이름
    public Hunter targetUnit;       // 부여된 헌터
    public int usageLimit = 3;    // 사용 가능 횟수
    public bool isActive = false; // 발동 중 여부

    public void ActivateSkill()
    {
        if (usageLimit <= 0)
        {
            Debug.Log($"[GrantSkill] {skillName} 사용 불가(횟수 소진)");
            return;
        }
        isActive = true;
        usageLimit--;
        Debug.Log($"[GrantSkill] 스킬 [{skillName}] 발동! 남은 사용 횟수: {usageLimit}");
        // TODO: 실제 스킬 효과 구현
    }

    public void DeactivateSkill()
    {
        isActive = false;
        Debug.Log($"[GrantSkill] 스킬 [{skillName}] 비활성화");
    }
}
