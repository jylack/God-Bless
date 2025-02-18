using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public List<string> skills;
    public List<Vector3> patrolPoints;//순찰 좌표
    public float chaseRange = 10f; //추격 범위
    public float chaseSpeed = 5f; //추격 속도
    public float patrolSpeed = 3.5f;//순찰 속도
}
