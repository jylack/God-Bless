using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public string Name;
    public Gate_Way_Group WayGroup;
    public Gate_Class MaxGateLevel;
    public int GateCount;
    public List<Transform> patrolPoints; //해당 지역의 순찰 경로

    /// <summary>
    /// 지역생성 설정
    /// </summary>
    /// <param name="name">지역명</param>
    /// <param name="wayGroup">게이트 웨이포인트 그룹명</param>
    /// <param name="maxGateLevel">게이트 등급</param>
    /// <param name="patrolPoints">게이트 웨이 좌표들</param>
    public Region(string name, Gate_Way_Group wayGroup, Gate_Class maxGateLevel, List<Transform> patrolPoints)
    {
        Name = name;
        WayGroup = wayGroup;
        MaxGateLevel = maxGateLevel;
        GateCount = Random.Range(2, 6); // 게이트 개수 랜덤화
        this.patrolPoints = patrolPoints;
    }
}
