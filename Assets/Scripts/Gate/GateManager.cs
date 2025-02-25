using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public static GateManager Instance;

    [Header("데이터베이스 참조")]
    public DataBase dataBase;

    [Header("웨이포인트 참조")]
    public WayPoints wayPoints;

    [Header("지역 목록")]
    public List<Region> regions = new List<Region>();

    [Header("몬스터 프리팹 모아둘곳")]
    public GameObject monster;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeRegions();
        }
    }

    private void InitializeRegions()
    {
        // 난이도별 최대 게이트 레벨 제한 설정
        regions.Add(new Region("Zone A", Gate_Way_Group.A, Gate_Class.C, 3));
        regions.Add(new Region("Zone B", Gate_Way_Group.B, Gate_Class.C, 3));
        regions.Add(new Region("Zone C", Gate_Way_Group.C, Gate_Class.C, 3));
        regions.Add(new Region("Zone D", Gate_Way_Group.D, Gate_Class.S, 5));
        regions.Add(new Region("Zone E", Gate_Way_Group.E, Gate_Class.SSS, 6));
        regions.Add(new Region("Zone F", Gate_Way_Group.F, Gate_Class.E, 3));
        regions.Add(new Region("Zone G", Gate_Way_Group.G, Gate_Class.A, 4));
        regions.Add(new Region("Zone H", Gate_Way_Group.H, Gate_Class.SS, 4));
        regions.Add(new Region("Zone I", Gate_Way_Group.I, Gate_Class.F, 3));
    }

    public void SpawnGatesForRegion(Region region)
    {
        List<Transform> spawnPositions = wayPoints.GetWayPos(region.WayGroup);

        for (int i = 0; i < region.GateCount; i++)
        {
            if (spawnPositions.Count == 0) continue;

            Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
            //GameObject gateObj = Instantiate(dataBase.gateTypes[(int)region.MaxGateLevel], spawnPos, Quaternion.identity);
            //지금 0번 게이트밖에없음.
            GameObject gateObj = Instantiate(dataBase.gateTypes[0], spawnPos, Quaternion.identity, monster.transform);

            GateCtrl gate = gateObj.GetComponent<GateCtrl>();

            if (gate != null)
            {
                gate.gateClass = region.MaxGateLevel;
                gate.SetRegion(null);
                gate.SetMonster(dataBase.GetMonster(MonsterName.Slime));
            }
        }
    }

    public void SpawnAllRegions()
    {
        foreach (var region in regions)
        {
            SpawnGatesForRegion(region);
        }
    }
}

public class Region
{
    public string Name;
    public Gate_Way_Group WayGroup;
    public Gate_Class MaxGateLevel;
    public int GateCount;

    public Region(string name, Gate_Way_Group wayGroup, Gate_Class maxGateLevel, int gateCount)
    {
        Name = name;
        WayGroup = wayGroup;
        MaxGateLevel = maxGateLevel;
        GateCount = gateCount;
    }
}
