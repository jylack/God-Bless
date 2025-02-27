using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public string Name;
    public Gate_Way_Group WayGroup;
    public Gate_Class MaxGateLevel;
    public int GateCount;
    public List<Transform> patrolPoints; // 🔹 해당 지역의 순찰 경로

    public Region(string name, Gate_Way_Group wayGroup, Gate_Class maxGateLevel, List<Transform> patrolPoints)
    {
        Name = name;
        WayGroup = wayGroup;
        MaxGateLevel = maxGateLevel;
        GateCount = Random.Range(2, 6); // 게이트 개수 랜덤화
        this.patrolPoints = patrolPoints;
    }
}
