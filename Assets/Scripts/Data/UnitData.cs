using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("유닛 모델")]
    public UnitType unitType; // 몬스터인지 헌터인지 구별용

    [Header("특수 데이터형")]
    public List<SkillData> skills;
    public List<Vector3> patrolPoints; // 순찰 좌표

    [Header("네비메쉬 속성")]
    public float chaseRange = 10f; // 추격 범위
    public float chaseSpeed = 5f; // 추격 속도
    public float patrolSpeed = 3.5f; // 순찰 속도

    [Header("스테이터스")]
    public string unitName;
    public int level;
    public float exp;
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;

    [Header("전투 속성")]
    public float attackRange = 2f; //공격 범위 추가
}
