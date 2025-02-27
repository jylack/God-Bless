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
            Debug.LogWarning($"[God] ���� ������({hunterName}) ����.");
            return;
        }

        GameObject hunterGO = new GameObject(hunterData.unitName);
        hunterGO.tag = "Hunter"; // ���� �±� �ڵ� ����

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        newHunter.data = hunterData;

        listUnit.Add(newHunter);
        Debug.Log($"[God] ���ο� ���� ����: {hunterName}, �±� ���� �Ϸ�!");
    }

    public void DrawSkill()
    {
        SkillData drawnSkill = dataBase.GetRandomSkill();
        if (drawnSkill != null)
        {
            Debug.Log($"[God] {drawnSkill.skillName} ��ų�� ȹ��!");
        }
        else
        {
            Debug.LogWarning("[God] ��ų �̱� ����: ��ų ������ ����.");
        }
    }
}
