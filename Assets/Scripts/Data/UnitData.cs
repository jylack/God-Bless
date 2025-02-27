using UnityEngine;
using System.Collections.Generic;

public enum UnitType { Hunter, Monster, Citizen }

[CreateAssetMenu(fileName = "UnitData", menuName = "Game Data/Unit")]
public class UnitData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string unitName;
    public UnitType unitType;
    public int level;
    public float maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;

    [Header("�̵� �� ����")]
    public float chaseSpeed;
    public float chaseRange;
    public float patrolSpeed;
    public float attackRange;

    [Header("��ų ���")]
    public List<SkillData> skills;
}
