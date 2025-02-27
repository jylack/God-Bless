using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 게이트 난이도 등급 Enum
/// </summary>
public enum Gate_Class
{
    F = 0,   // 최하 등급
    E = 1,
    D = 2,
    C = 3,
    B = 4,
    A = 5,
    S = 6,
    SS = 7,
    SSS = 8  // 최고 등급
}


[CreateAssetMenu(fileName = "GateData", menuName = "Game Data/Gate")]
public class GateData : ScriptableObject
{
    [Header("기본 정보")]
    public string gateName;
    public Gate_Class gateClass; // 기존 int 대신 Gate_Class 사용

    [Header("몬스터 스폰 관련")]
    public List<UnitData> spawnableMonsters;
    public int monsterSpawnCount;
}
