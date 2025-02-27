using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����Ʈ ���̵� ��� Enum
/// </summary>
public enum Gate_Class
{
    F = 0,   // ���� ���
    E = 1,
    D = 2,
    C = 3,
    B = 4,
    A = 5,
    S = 6,
    SS = 7,
    SSS = 8  // �ְ� ���
}


[CreateAssetMenu(fileName = "GateData", menuName = "Game Data/Gate")]
public class GateData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string gateName;
    public Gate_Class gateClass; // ���� int ��� Gate_Class ���

    [Header("���� ���� ����")]
    public List<UnitData> spawnableMonsters;
    public int monsterSpawnCount;
}
