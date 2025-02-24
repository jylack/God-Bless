using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("���� ��")]
    public UnitType unitType; // �������� �������� ������

    [Header("Ư�� ��������")]
    public List<SkillData> skills;
    public List<Vector3> patrolPoints; // ���� ��ǥ

    [Header("�׺�޽� �Ӽ�")]
    public float chaseRange = 10f; // �߰� ����
    public float chaseSpeed = 5f; // �߰� �ӵ�
    public float patrolSpeed = 3.5f; // ���� �ӵ�

    [Header("�������ͽ�")]
    public string unitName;
    public int level;
    public float exp;
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;

    [Header("���� �Ӽ�")]
    public float attackRange = 2f; //���� ���� �߰�
}
