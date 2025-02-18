using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

// -------------------------------------------------------
// GOD : ��ü ������ �����ϴ� �Ŵ��� ������ Ŭ����
// -------------------------------------------------------
public class GOD : MonoBehaviour
{
    [Header("�� ���� ��ų ���� ��ü")]
    public GodSkill godSkill;

    [Header("���� �����ϴ� ���� ���")]
    public List<Hunter> listUnit = new List<Hunter>();

    [Header("���� �����ϴ� ���� ���")]
    public List<MonsterCtrl> listMonster = new List<MonsterCtrl>();

    [Header("���� �ο��� ��ų ���")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    private void Start()
    {
        // ���� �� GodSkill �ʱ�ȭ (�ʿ��ϴٸ�)
        if (godSkill == null)
        {
            godSkill = new GodSkill();
        }

        // ����: ���� ���� �� ���� 2���� ����
        godSkill.UnitCreate(this, "HunterA");
        godSkill.UnitCreate(this, "HunterB");
    }

    private void Update()
    {
        // ���� ��ü ����(���� ����, ��ų ��Ÿ�� ���� ��)�� ������Ʈ
        // TODO: ����
    }

    // ����: ��� ���Ϳ��� ���� ������� �ɰ� ���� ��
    public void AllDeBuffMonsters()
    {
        godSkill.AllDeBuff(this);
    }

    // ����: ��� ���ֿ��� ���� ������ �ɰ� ���� ��
    public void AllBuffUnits()
    {
        godSkill.AllBuff(this);
    }
}

// -------------------------------------------------------
// GodSkill : ��(GOD)�� ����� �� �ִ� ���� ��ų/���
// -------------------------------------------------------

[System.Serializable]
public class GodSkill
{
    // 1. ���� ���� (�Ӽ� ����, ��ųĭ ��)
    public void UnitCreate(GOD god, string unitName)
    {
        // �����δ� ������ Instantiate�� ���� ���� GameObject�� �����ϴ� ������ ���� ����
        GameObject unitGO = new GameObject(unitName);
        Hunter newUnit = unitGO.AddComponent<Hunter>();

        // ���� �ʱ� ���� ���� (����)
        newUnit.MaxHP = 100;
        newUnit.HP = 100;
        newUnit.Atk = 10;
        newUnit.MAtk = 5;
        newUnit.Def = 5;
        newUnit.inGate = false;

        // GOD �Ŵ����� ���
        god.listUnit.Add(newUnit);

        Debug.Log($"[GodSkill] ���ο� ���� ����: {unitName}");
    }

    // 2. Ư�� ���ֿ��� ��ų �ο�
    public void AllocateSkill(GOD god, Hunter targetUnit, GrantSkill skill)
    {
        if (targetUnit == null || skill == null)
        {
            Debug.LogWarning("[GodSkill] AllocateSkill ����: ��� �Ǵ� ��ų�� null");
            return;
        }
        // ��ų �ο�
        targetUnit.skill = skill;
        // ��ų�� �����ϴ� ���ֵ� ����
        skill.targetUnit = targetUnit;

        // GOD �Ŵ����� �ο��� ��ų ��Ͽ� �߰�
        if (!god.listGrantSkill.Contains(skill))
        {
            god.listGrantSkill.Add(skill);
        }

        Debug.Log($"[GodSkill] {targetUnit.gameObject.name}���� ��ų [{skill.skillName}] �ο�");
    }

    // 3. ���� ����� (���� �ʵ�(����)�� �ִ� ���͵鿡�� ���� ����, ���ݷ� ���� ��)
    public void AllDeBuff(GOD god)
    {
        // ����: ���͵��� ����/�� �Ͻ������� 50% ����
        foreach (MonsterCtrl monster in god.listMonster)
        {
            // TODO: ���� ����� ����(���� ���� �ð�, ��ø ��)�� ����
            monster.Atk *= 0.5f;
            monster.Matk *= 0.5f;
            monster.Def *= 0.5f;
        }
        Debug.Log("[GodSkill] ��� ���Ϳ��� ���� ����� ����");
    }

    // 4. ���� ���� (���� �ʵ�(����)�� �ִ� ���ֵ鿡�� ���� ����, ���ݷ� ���� ��)
    public void AllBuff(GOD god)
    {
        // ����: ���ֵ��� ����/�� �Ͻ������� 150% ����
        foreach (Hunter unit in god.listUnit)
        {
            // TODO: ���� ���� ����(���� ���� �ð�, ��ø ��)�� ����
            unit.Atk *= 1.5f;
            unit.MAtk *= 1.5f;
            unit.Def *= 1.5f;
        }
        Debug.Log("[GodSkill] ��� ���ֿ��� ���� ���� ����");
    }
}

// -------------------------------------------------------
// GrantSkill : �ο� ��ų ���� (��: �Ÿ�����, ������, ���� ��)
// -------------------------------------------------------
[System.Serializable]
public class GrantSkill
{
    public string skillName;      // ��ų �̸�
    public Hunter targetUnit;       // � ���ֿ��� �ο��Ǿ��°�
    public int usageLimit = 3;    // ��� ���� Ƚ�� ��
    public bool isActive = false; // �ߵ� ������ ����

    // ��ų �ߵ� ����, Ƚ�� ����, ��Ÿ�� �� �پ��� ������ ���⿡ ���� ����
    // ����: ��ų �ߵ�
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

        // TODO: ���� ��ų ȿ��(������, ����, ����� ��) ����
    }

    // ��ų ��Ȱ��ȭ
    public void DeactivateSkill()
    {
        isActive = false;
        Debug.Log($"[GrantSkill] ��ų [{skillName}] ��Ȱ��ȭ");
    }
}
