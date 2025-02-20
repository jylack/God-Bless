using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    public GameObject obj;//유닛 오브젝트 생성시 사용

    public UnitType unitType;//몬스터인지 헌터인지 구별용
    public List<SkillType> skills;
    public List<Vector3> patrolPoints;//순찰 좌표
    
    public float chaseRange = 10f; //추격 범위
    public float chaseSpeed = 5f; //추격 속도
    public float patrolSpeed = 3.5f;//순찰 속도

    //스테이터스
    public int level;
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public float speed;
    public bool isGate;
}
