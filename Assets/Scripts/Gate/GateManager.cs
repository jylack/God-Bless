using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GateManager : MonoBehaviour
{
    public static GateManager Instance;

    [Header("�����ͺ��̽� ����")]
    public DataBase dataBase;

    [Header("��������Ʈ ����")]
    public WayPoints wayPoints;

    [Header("���� ������ �θ� ������Ʈ")]
    public Transform monsterParent;

    [Header("���̺� ��� ����Ʈ �ڵ� ���� Ȱ��ȭ")]
    public bool autoSpawnEnabled = true;

    [Header("��� ���� ����")]
    List<Region> regions = new List<Region>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // ���� ���� �� �⺻ ����Ʈ ����
        SpawnAllRegions();

        // �ڵ� ���̺� ����Ʈ ���� Ȱ��ȭ (������)
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
                Debug.LogError("[GateManager] WayPoints �ν��Ͻ��� ã�� �� �����ϴ�!");
                return;
            }
        }

        List<Transform> spawnPositions = wayPoints.GetWayPos(region.WayGroup);
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            Debug.LogWarning($"[GateManager] {region.Name}�� ��������Ʈ�� �����ϴ�.");
            return;
        }

        for (int i = 0; i < region.GateCount; i++)
        {
            Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)].position;

            GateData gateData = dataBase.GetGateData(region.MaxGateLevel);
            if (gateData == null)
            {
                Debug.LogWarning($"[GateManager] {region.Name}�� ����� {region.MaxGateLevel} ����� ����Ʈ �����Ͱ� �����ϴ�.");
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
    /// Ư�� ������ ����Ʈ�� ���� (�ܺο��� ȣ�� ����)
    /// </summary>
    public void SpawnGate(GateData gateData, Vector3 position)
    {
        if (dataBase == null)
        {
            Debug.LogError("[GateManager] DataBase�� �������� �ʽ��ϴ�.");
            return;
        }

        if (gateData == null)
        {
            Debug.LogWarning("[GateManager] ����Ʈ �����Ͱ� �����ϴ�.");
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
    /// ���� ���� �� ��� ������ ����Ʈ ����
    /// </summary>
    public void SpawnAllRegions()
    {
        List<GateData> allGates = dataBase.GetGateDatas();
        foreach (var gateData in allGates)
        {
            Vector3 randomPos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50)); // ���� ��ġ
            SpawnGate(gateData, randomPos);
        }
    }

    /// <summary>
    /// ���� �ð����� �ڵ����� ����Ʈ ���� (���̺� �ý���)
    /// </summary>
    private IEnumerator AutoSpawnGates()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f); // 60�ʸ��� ���ο� ����Ʈ ����
            SpawnAllRegions();
        }
    }
}
