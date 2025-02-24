using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOD : MonoBehaviour
{
    [Header("�� ���� ��ų ���� ��ü")]
    public GodSkill godSkill;

    [Header("���� ���� ���� ���")]
    public List<UnitCtrl> listUnit = new List<UnitCtrl>();

    [Header("���� �ο� �� �� �ִ� ��ų ���")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    private void Start()
    {
        if (godSkill == null)
        {
            godSkill = new GodSkill();
        }
    }

    // ��� ���Ϳ� ���� ����� ����
    public void AllDeBuffMonsters()
    {
        godSkill.AllDeBuff(this);
    }

    // ��� ���Ϳ� ���� ���� ����
    public void AllBuffUnits()
    {
        godSkill.AllBuff(this);
    }
}

[System.Serializable]
public class GodSkill
{
    // 1. ���� ���� (���� ���� �� ���� ��ų �Ҵ�)
    public void UnitCreate(GOD god, string unitName)
    {
        GameObject unitGO = new GameObject(unitName);
        // UnitCtrl �߰�
        UnitCtrl newUnit = unitGO.AddComponent<UnitCtrl>();
        //Hunter ������Ʈ�� �߰�
        if (newUnit.data != null)
        {
            unitGO.AddComponent<Hunter>();
        }

        // �����ͺ��̽����� ���� ��ų �Ҵ�
        DataBase db = GameObject.FindObjectOfType<DataBase>();
        if (db != null && db.listGrantSkill.Count > 0)
        {
            int randIndex = Random.Range(0, db.listGrantSkill.Count);
            GrantSkill randomSkill = db.listGrantSkill[randIndex];
            // Hunter ������Ʈ�� �ִٸ� ��ų �Ҵ�
            Hunter hunterComp = unitGO.GetComponent<Hunter>();
            if (hunterComp != null)
            {
                hunterComp.skill = randomSkill;
                randomSkill.targetUnit = hunterComp;
            }
        }

        god.listUnit.Add(newUnit);
        Debug.Log($"[GodSkill] ���ο� ���� ����: {unitName}");
    }

    // 2. Ư�� ���Ϳ��� ��ų �ο�
    public void AllocateSkill(GOD god, Hunter targetUnit, GrantSkill skill)
    {
        if (targetUnit == null || skill == null)
        {
            Debug.LogWarning("[GodSkill] AllocateSkill ����: ��� �Ǵ� ��ų�� null");
            return;
        }
        targetUnit.skill = skill;
        skill.targetUnit = targetUnit;
        if (!god.listGrantSkill.Contains(skill))
        {
            god.listGrantSkill.Add(skill);
        }
        Debug.Log($"[GodSkill] {targetUnit.gameObject.name}���� ��ų [{skill.skillName}] �ο�");
    }

    // 3. ���� �����: ��� �ʵ� ������ ������ 50%�� ���� (���ӽð� 10��)
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
        Debug.Log("[GodSkill] ��� ���Ϳ��� ���� ����� ����");
    }

    // 4. ���� ����: ��� ���� ������ ������ 150%�� ���� (���ӽð� 10��)
    public void AllBuff(GOD god)
    {
        foreach (UnitCtrl unit in god.listUnit)
        {
            if (unit != null && unit.Type == UnitType.Hunter)
            {
                unit.ApplyBuff(1.5f, 10f);
            }
        }
        Debug.Log("[GodSkill] ��� ���ֿ��� ���� ���� ����");
    }

    // 5.UI���� �̱⸦ ���� ��ų�� �߰��ϴ� �޼���
    public void DrawSkill(GOD god)
    {
        DataBase db = GameObject.FindObjectOfType<DataBase>();
        if (db == null || db.listGrantSkill.Count == 0)
        {
            Debug.LogWarning("DrawSkill: �����ͺ��̽��� ��ų ����� �����ϴ�.");
            return;
        }
        int index = Random.Range(0, db.listGrantSkill.Count);
        GrantSkill drawnSkill = db.listGrantSkill[index];

        // ���ο� �ν��Ͻ��� �����Ͽ� �߰� (���� ���� ����)
        GrantSkill newSkill = new GrantSkill();
        newSkill.skillName = drawnSkill.skillName;
        newSkill.usageLimit = drawnSkill.usageLimit;
        newSkill.isActive = drawnSkill.isActive;

        god.listGrantSkill.Add(newSkill);
        Debug.Log($"DrawSkill: {newSkill.skillName} ��ų�� �̾� GOD�� ��ų ��Ͽ� �߰��߽��ϴ�.");
    }
}

[System.Serializable]
public class GrantSkill
{
    public string skillName;      // ��ų �̸�
    public Hunter targetUnit;       // �ο��� ����
    public int usageLimit = 3;    // ��� ���� Ƚ��
    public bool isActive = false; // �ߵ� �� ����

    public void ActivateSkill()
    {
        if (usageLimit <= 0)
        {
            Debug.Log($"[GrantSkill] {skillName} ��� �Ұ�(Ƚ�� ����)");
            return;
        }
        isActive = true;
        usageLimit--;
        Debug.Log($"[GrantSkill] ��ų [{skillName}] �ߵ�! ���� ��� Ƚ��: {usageLimit}");
        // TODO: ���� ��ų ȿ�� ����
    }

    public void DeactivateSkill()
    {
        isActive = false;
        Debug.Log($"[GrantSkill] ��ų [{skillName}] ��Ȱ��ȭ");
    }
}
