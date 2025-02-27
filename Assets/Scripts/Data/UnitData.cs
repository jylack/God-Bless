using UnityEngine;
using System.Collections.Generic;

public enum UnitType { Hunter, Monster, Citizen }

[CreateAssetMenu(fileName = "UnitData", menuName = "Game Data/Unit")]
public class UnitData : ScriptableObject
{
    [Header("기본 정보")]
    public string unitName;
    public UnitType unitType;
    public int level;
    public float maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;

    [Header("이동 및 전투")]
    public float chaseSpeed;
    public float chaseRange;
    public float patrolSpeed;
    public float attackRange;

    [Header("스킬 목록")]
    public List<SkillData> skills;
}
