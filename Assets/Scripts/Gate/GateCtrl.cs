using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCtrl : MonoBehaviour
{
    private GateData gateData;
    private Region region;
    private bool isCleared = false;
    private float gateTimer = 60f; // ����Ʈ ���� �ð� (��: 60��)

    [Header("���� ���� ��ġ")]
    public Transform spawnPoint;

    private void Start()
    {
        if (gateData == null)
        {
            Debug.LogWarning("[GateCtrl] GateData�� �������� �ʾҽ��ϴ�.");
            return;
        }

        StartCoroutine(GateTimer());
    }

    /// <summary>
    /// ����Ʈ ������ ����
    /// </summary>
    public void SetGateData(GateData data)
    {
        gateData = data;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void SetRegion(Region regionData)
    {
        region = regionData;
    }

    /// <summary>
    /// ���� �ð��� ������ ����Ʈ ����
    /// </summary>
    private IEnumerator GateTimer()
    {
        yield return new WaitForSeconds(gateTimer);
        if (!isCleared)
        {
            Debug.Log($"[GateCtrl] {gateData.gateName} ����! ���� ��ȯ");
            SpawnMonsters();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ����Ʈ Ŭ���� (���Ͱ� Ŭ�������� ��)
    /// </summary>
    public void ClearGate()
    {
        isCleared = true;
        Debug.Log($"[GateCtrl] {gateData.gateName} Ŭ�����!");
        Destroy(gameObject);
    }

    /// <summary>
    /// ���� ��ȯ (����Ʈ ���� ��)
    /// </summary>
    private void SpawnMonsters()
    {
        for (int i = 0; i < gateData.monsterSpawnCount; i++)
        {
            UnitData randomMonster = gateData.spawnableMonsters[Random.Range(0, gateData.spawnableMonsters.Count)];
            GameObject monsterGO = new GameObject(randomMonster.unitName);
            monsterGO.transform.position = spawnPoint != null ? spawnPoint.position : transform.position;
            UnitCtrl monsterCtrl = monsterGO.AddComponent<UnitCtrl>();
            monsterCtrl.data = randomMonster;
        }
    }
}
