using UnityEngine;
using System.Collections.Generic;

public class GOD : MonoBehaviour
{
    public DataBase dataBase;
    public List<UnitCtrl> listUnit = new List<UnitCtrl>();
    public void UnitCreate(string hunterName)
    {
        UnitData hunterData = dataBase.GetHunterData(hunterName);
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

    public void DrawSkill()
    {
        SkillData drawnSkill = dataBase.GetRandomSkill();
        if (drawnSkill != null)
        {
            Debug.Log($"[God] {drawnSkill.skillName} 스킬을 획득!");
        }
        else
        {
            Debug.LogWarning("[God] 스킬 뽑기 실패: 스킬 데이터 없음.");
        }
    }
}
