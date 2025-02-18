using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

// -------------------------------------------------------
// GOD : 전체 게임을 관리하는 매니저 느낌의 클래스
// -------------------------------------------------------
public class GOD : MonoBehaviour
{
    [Header("신 전용 스킬 관리 객체")]
    public GodSkill godSkill;

    [Header("현재 존재하는 유닛 목록")]
    public List<Hunter> listUnit = new List<Hunter>();

    [Header("현재 존재하는 몬스터 목록")]
    public List<MonsterCtrl> listMonster = new List<MonsterCtrl>();

    [Header("현재 부여된 스킬 목록")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    private void Start()
    {
        // 시작 시 GodSkill 초기화 (필요하다면)
        if (godSkill == null)
        {
            godSkill = new GodSkill();
        }

        // 예시: 게임 시작 시 유닛 2마리 생성
        godSkill.UnitCreate(this, "HunterA");
        godSkill.UnitCreate(this, "HunterB");
    }

    private void Update()
    {
        // 게임 전체 로직(승패 판정, 스킬 쿨타임 관리 등)을 업데이트
        // TODO: 구현
    }

    // 예시: 모든 몬스터에게 광역 디버프를 걸고 싶을 때
    public void AllDeBuffMonsters()
    {
        godSkill.AllDeBuff(this);
    }

    // 예시: 모든 유닛에게 광역 버프를 걸고 싶을 때
    public void AllBuffUnits()
    {
        godSkill.AllBuff(this);
    }
}

// -------------------------------------------------------
// GodSkill : 신(GOD)이 사용할 수 있는 전용 스킬/기능
// -------------------------------------------------------

[System.Serializable]
public class GodSkill
{
    // 1. 유닛 생성 (속성 설정, 스킬칸 등)
    public void UnitCreate(GOD god, string unitName)
    {
        // 실제로는 프리팹 Instantiate를 통해 유닛 GameObject를 생성하는 식으로 구현 가능
        GameObject unitGO = new GameObject(unitName);
        Hunter newUnit = unitGO.AddComponent<Hunter>();

        // 유닛 초기 스탯 설정 (예시)
        newUnit.MaxHP = 100;
        newUnit.HP = 100;
        newUnit.Atk = 10;
        newUnit.MAtk = 5;
        newUnit.Def = 5;
        newUnit.inGate = false;

        // GOD 매니저에 등록
        god.listUnit.Add(newUnit);

        Debug.Log($"[GodSkill] 새로운 유닛 생성: {unitName}");
    }

    // 2. 특정 유닛에게 스킬 부여
    public void AllocateSkill(GOD god, Hunter targetUnit, GrantSkill skill)
    {
        if (targetUnit == null || skill == null)
        {
            Debug.LogWarning("[GodSkill] AllocateSkill 실패: 대상 또는 스킬이 null");
            return;
        }
        // 스킬 부여
        targetUnit.skill = skill;
        // 스킬이 참조하는 유닛도 설정
        skill.targetUnit = targetUnit;

        // GOD 매니저에 부여된 스킬 목록에 추가
        if (!god.listGrantSkill.Contains(skill))
        {
            god.listGrantSkill.Add(skill);
        }

        Debug.Log($"[GodSkill] {targetUnit.gameObject.name}에게 스킬 [{skill.skillName}] 부여");
    }

    // 3. 광역 디버프 (현재 필드(도시)에 있는 몬스터들에게 방어력 감소, 공격력 감소 등)
    public void AllDeBuff(GOD god)
    {
        // 예시: 몬스터들의 공격/방어를 일시적으로 50% 감소
        foreach (MonsterCtrl monster in god.listMonster)
        {
            // TODO: 실제 디버프 로직(버프 지속 시간, 중첩 등)을 구현
            monster.Atk *= 0.5f;
            monster.Matk *= 0.5f;
            monster.Def *= 0.5f;
        }
        Debug.Log("[GodSkill] 모든 몬스터에게 광역 디버프 적용");
    }

    // 4. 광역 버프 (현재 필드(도시)에 있는 유닛들에게 방어력 증가, 공격력 증가 등)
    public void AllBuff(GOD god)
    {
        // 예시: 유닛들의 공격/방어를 일시적으로 150% 증가
        foreach (Hunter unit in god.listUnit)
        {
            // TODO: 실제 버프 로직(버프 지속 시간, 중첩 등)을 구현
            unit.Atk *= 1.5f;
            unit.MAtk *= 1.5f;
            unit.Def *= 1.5f;
        }
        Debug.Log("[GodSkill] 모든 유닛에게 광역 버프 적용");
    }
}

// -------------------------------------------------------
// GrantSkill : 부여 스킬 정보 (예: 거리유리, 광역마, 번개 등)
// -------------------------------------------------------
[System.Serializable]
public class GrantSkill
{
    public string skillName;      // 스킬 이름
    public Hunter targetUnit;       // 어떤 유닛에게 부여되었는가
    public int usageLimit = 3;    // 사용 가능 횟수 등
    public bool isActive = false; // 발동 중인지 여부

    // 스킬 발동 조건, 횟수 제한, 쿨타임 등 다양한 로직을 여기에 구현 가능
    // 예시: 스킬 발동
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

        // TODO: 실제 스킬 효과(데미지, 버프, 디버프 등) 구현
    }

    // 스킬 비활성화
    public void DeactivateSkill()
    {
        isActive = false;
        Debug.Log($"[GrantSkill] 스킬 [{skillName}] 비활성화");
    }
}
