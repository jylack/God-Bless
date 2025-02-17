using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public int maxHp;
    public int atk;
    public int matk;
    public int def;
    public List<string> skills;
    public List<Vector3> patrolPoints;
    public float chaseRange = 10f;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
}
