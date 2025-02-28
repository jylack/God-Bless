using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GateManager : MonoBehaviour
{
    public static GateManager Instance;

    [Header("데이터베이스 참조")]
    public DataBase dataBase;

    [Header("웨이포인트 참조")]
    public WayPoints wayPoints;

    [Header("몬스터 프리팹 부모 오브젝트")]
    public Transform monsterParent;

    [Header("웨이브 기반 게이트 자동 생성 활성화")]
    public bool autoSpawnEnabled = true;

    [Header("모든 지역 참조")]
    List<Region> regions = new List<Region>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // 게임 시작 시 기본 게이트 스폰
        SpawnAllRegions();

        // 자동 웨이브 게이트 생성 활성화 (선택적)
        if (autoSpawnEnabled)
        {
            StartCoroutine(AutoSpawnGates());
        }
    }
    private void InitializeRegions()
    {
        WayPoints wayPoints = FindObjectOfType<WayPoints>();

        regions.Add(new Region("Zone A", Gate_Way_Group.A, Gate_Class.C, wayPoints.GetWayPos(Gate_Way_Group.A)));
        regions.Add(new Region("Zone B", Gate_Way_Group.B, Gate_Class.C, wayPoints.GetWayPos(Gate_Way_Group.B)));
        regions.Add(new Region("Zone C", Gate_Way_Group.C, Gate_Class.C, wayPoints.GetWayPos(Gate_Way_Group.C)));
        regions.Add(new Region("Zone D", Gate_Way_Group.D, Gate_Class.S, wayPoints.GetWayPos(Gate_Way_Group.D)));
        regions.Add(new Region("Zone E", Gate_Way_Group.E, Gate_Class.SSS, wayPoints.GetWayPos(Gate_Way_Group.E)));
        regions.Add(new Region("Zone F", Gate_Way_Group.F, Gate_Class.E, wayPoints.GetWayPos(Gate_Way_Group.F)));
        regions.Add(new Region("Zone G", Gate_Way_Group.G, Gate_Class.A, wayPoints.GetWayPos(Gate_Way_Group.G)));
        regions.Add(new Region("Zone H", Gate_Way_Group.H, Gate_Class.SS, wayPoints.GetWayPos(Gate_Way_Group.H)));
        regions.Add(new Region("Zone I", Gate_Way_Group.I, Gate_Class.F, wayPoints.GetWayPos(Gate_Way_Group.I)));
    }

    public void SpawnGatesForRegion(Region region)
    {
        if (wayPoints == null)
        {
            wayPoints = FindObjectOfType<WayPoints>();
            if (wayPoints == null)
            {
                Debug.LogError("[GateManager] WayPoints 인스턴스를 찾을 수 없습니다!");
                return;
            }
        }

        List<Transform> spawnPositions = wayPoints.GetWayPos(region.WayGroup);
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            Debug.LogWarning($"[GateManager] {region.Name}의 웨이포인트가 없습니다.");
            return;
        }

        for (int i = 0; i < region.GateCount; i++)
        {
            Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)].position;

            GateData gateData = dataBase.GetGateData(region.MaxGateLevel);
            if (gateData == null)
            {
                Debug.LogWarning($"[GateManager] {region.Name}에 사용할 {region.MaxGateLevel} 등급의 게이트 데이터가 없습니다.");
                continue;
            }

            GameObject gateObj = new GameObject(gateData.gateName);
            gateObj.transform.position = spawnPos;

            GateCtrl gateCtrl = gateObj.AddComponent<GateCtrl>();
            gateCtrl.SetGateData(gateData);
            gateCtrl.SetRegion(region);
        }
    }


    /// <summary>
    /// 특정 지역에 게이트를 생성 (외부에서 호출 가능)
    /// </summary>
    public void SpawnGate(GateData gateData, Vector3 position)
    {
        if (dataBase == null)
        {
            Debug.LogError("[GateManager] DataBase가 존재하지 않습니다.");
            return;
        }

        if (gateData == null)
        {
            Debug.LogWarning("[GateManager] 게이트 데이터가 없습니다.");
            return;
        }

        GameObject gateObj = new GameObject(gateData.gateName);
        gateObj.transform.position = position;

        for (int i = 0; i < gateData.monsterSpawnCount; i++)
        {
            UnitData randomMonster = gateData.spawnableMonsters[Random.Range(0, gateData.spawnableMonsters.Count)];
            GameObject monsterGO = new GameObject(randomMonster.unitName);
            monsterGO.transform.position = position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            UnitCtrl monsterCtrl = monsterGO.AddComponent<UnitCtrl>();
            monsterCtrl.data = randomMonster;
            monsterGO.transform.SetParent(monsterParent);
        }
    }

    /// <summary>
    /// 게임 시작 시 모든 지역에 게이트 생성
    /// </summary>
    public void SpawnAllRegions()
    {
        List<GateData> allGates = dataBase.GetGateDatas();
        foreach (var gateData in allGates)
        {
            Vector3 randomPos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50)); // 랜덤 위치
            SpawnGate(gateData, randomPos);
        }
    }

    /// <summary>
    /// 일정 시간마다 자동으로 게이트 생성 (웨이브 시스템)
    /// </summary>
    private IEnumerator AutoSpawnGates()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f); // 60초마다 새로운 게이트 생성
            SpawnAllRegions();
        }
    }
}
