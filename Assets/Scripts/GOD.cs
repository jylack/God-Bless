using UnityEngine;
using System.Collections.Generic;

public class GOD : MonoBehaviour
{
    public DataBase dataBase;
    public List<UnitCtrl> listUnit = new List<UnitCtrl>();
    public List<SkillData> godSkills = new List<SkillData>(); // GOD이 보유한 스킬 목록


    private void Start()
    {
        LoadInitialSkills();
    }

    private void LoadInitialSkills()
    {
        godSkills = dataBase.GetAllSkills(); // GOD이 모든 스킬을 보유
        Debug.Log($"[God] 총 {godSkills.Count}개의 스킬을 보유.");
    }

    /// <summary>
    /// GOD이 유닛을 생성할 때, 보유한 스킬 중 랜덤으로 하나를 부여하고 생성
    /// </summary>
    public void AutoUnitCreate()
    {
        if (godSkills.Count == 0)
        {
            Debug.LogWarning("[God] GOD이 보유한 부여 스킬이 없습니다!");
            return;
        }

        // 유닛 데이터 랜덤 선택 (헌터만 생성)
        UnitData randomHunterData = dataBase.GetRandomHunter();
        if (randomHunterData == null)
        {
            Debug.LogWarning("[God] 생성 가능한 헌터 데이터가 없습니다.");
            return;
        }

        // 랜덤 스킬 선택
        SkillData randomSkill = godSkills[Random.Range(0, godSkills.Count)];

        // 유닛 생성 및 스킬 부여
        GameObject hunterGO = new GameObject(randomHunterData.unitName);
        hunterGO.tag = "Hunter"; // 헌터 태그 자동 지정

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        newHunter.data = randomHunterData;
        newHunter.ReceiveSkill(randomSkill); // 스킬 부여

        listUnit.Add(newHunter);
        Debug.Log($"[God] 새로운 헌터 생성: {randomHunterData.unitName} / 부여된 스킬: {randomSkill.skillName}");
    }

    public void UnitCreate(string hunterName)
    {
        UnitData hunterData = dataBase.GetUnitData(hunterName);
        if (hunterData == null)
        {
            Debug.LogWarning($"[God] 헌터 데이터({hunterName}) 없음.");
            return;
        }

        GameObject hunterGO = new GameObject(hunterData.unitName);
        hunterGO.tag = "Hunter"; // 헌터 태그 자동 지정

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        newHunter.data = hunterData;

        

        
        listUnit.Add(newHunter);
        Debug.Log($"[God] 새로운 헌터 생성: {hunterName}, 태그 설정 완료!");
    }

   /// <summary>
    /// GOD이 보유한 스킬 중 하나를 유닛에게 부여
    /// </summary>
    public void GrantSkill(UnitCtrl targetUnit)
    {
        if (godSkills.Count == 0)
        {
            Debug.LogWarning("[God] 보유한 스킬이 없습니다!");
            return;
        }

        SkillData grantedSkill = godSkills[Random.Range(0, godSkills.Count)];
        targetUnit.ReceiveSkill(grantedSkill);

        Debug.Log($"[God] {targetUnit.data.unitName}에게 {grantedSkill.skillName} 스킬을 부여함.");
    }
}
