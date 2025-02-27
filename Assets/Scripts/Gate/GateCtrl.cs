using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCtrl : MonoBehaviour
{
    private GateData gateData;
    private Region region;
    private bool isCleared = false;
    private float gateTimer = 60f; // 게이트 유지 시간 (예: 60초)

    [Header("몬스터 스폰 위치")]
    public Transform spawnPoint;

    private void Start()
    {
        if (gateData == null)
        {
            Debug.LogWarning("[GateCtrl] GateData가 설정되지 않았습니다.");
            return;
        }

        StartCoroutine(GateTimer());
    }

    /// <summary>
    /// 게이트 데이터 설정
    /// </summary>
    public void SetGateData(GateData data)
    {
        gateData = data;
    }

    /// <summary>
    /// 지역 설정
    /// </summary>
    public void SetRegion(Region regionData)
    {
        region = regionData;
    }

    /// <summary>
    /// 일정 시간이 지나면 게이트 폭발
    /// </summary>
    private IEnumerator GateTimer()
    {
        yield return new WaitForSeconds(gateTimer);
        if (!isCleared)
        {
            Debug.Log($"[GateCtrl] {gateData.gateName} 폭발! 몬스터 소환");
            SpawnMonsters();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 게이트 클리어 (헌터가 클리어했을 때)
    /// </summary>
    public void ClearGate()
    {
        isCleared = true;
        Debug.Log($"[GateCtrl] {gateData.gateName} 클리어됨!");
        Destroy(gameObject);
    }

    /// <summary>
    /// 몬스터 소환 (게이트 폭발 시)
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
