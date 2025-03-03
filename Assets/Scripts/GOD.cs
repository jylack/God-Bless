using UnityEngine;
using System.Collections.Generic;

public class GOD : MonoBehaviour
{
    public DataBase dataBase;
    public List<UnitCtrl> listUnit = new List<UnitCtrl>();
    public List<SkillData> godSkills = new List<SkillData>(); // GOD�� ������ ��ų ���


    private void Start()
    {
        LoadInitialSkills();
    }

    private void LoadInitialSkills()
    {
        godSkills = dataBase.GetAllSkills(); // GOD�� ��� ��ų�� ����
        Debug.Log($"[God] �� {godSkills.Count}���� ��ų�� ����.");
    }

    /// <summary>
    /// GOD�� ������ ������ ��, ������ ��ų �� �������� �ϳ��� �ο��ϰ� ����
    /// </summary>
    public void AutoUnitCreate()
    {
        if (godSkills.Count == 0)
        {
            Debug.LogWarning("[God] GOD�� ������ �ο� ��ų�� �����ϴ�!");
            return;
        }

        // ���� ������ ���� ���� (���͸� ����)
        UnitData randomHunterData = dataBase.GetRandomHunter();
        if (randomHunterData == null)
        {
            Debug.LogWarning("[God] ���� ������ ���� �����Ͱ� �����ϴ�.");
            return;
        }

        // ���� ��ų ����
        SkillData randomSkill = godSkills[Random.Range(0, godSkills.Count)];

        // ���� ���� �� ��ų �ο�
        GameObject hunterGO = new GameObject(randomHunterData.unitName);
        hunterGO.tag = "Hunter"; // ���� �±� �ڵ� ����

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        newHunter.data = randomHunterData;
        newHunter.ReceiveSkill(randomSkill); // ��ų �ο�

        listUnit.Add(newHunter);
        Debug.Log($"[God] ���ο� ���� ����: {randomHunterData.unitName} / �ο��� ��ų: {randomSkill.skillName}");
    }

    public void UnitCreate(string hunterName)
    {
        UnitData hunterData = dataBase.GetUnitData(hunterName);
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

   /// <summary>
    /// GOD�� ������ ��ų �� �ϳ��� ���ֿ��� �ο�
    /// </summary>
    public void GrantSkill(UnitCtrl targetUnit)
    {
        if (godSkills.Count == 0)
        {
            Debug.LogWarning("[God] ������ ��ų�� �����ϴ�!");
            return;
        }

        SkillData grantedSkill = godSkills[Random.Range(0, godSkills.Count)];
        targetUnit.ReceiveSkill(grantedSkill);

        Debug.Log($"[God] {targetUnit.data.unitName}���� {grantedSkill.skillName} ��ų�� �ο���.");
    }
}
