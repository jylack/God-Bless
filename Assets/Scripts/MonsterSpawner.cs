using System.Collections.Generic;
using UnityEngine;

public class GateCtrl : MonoBehaviour
{
    [Header("오브젝트 풀에서 미리 받을 몬스터 수")]
    public int prefetchCount = 5;

    [Header("몬스터 스폰 위치 Offset (예시)")]
    public Vector3 spawnOffset = new Vector3(0, 0, 2);

    // 미리 받아놓은 몬스터들을 저장할 리스트
    private List<GameObject> pooledMonsters = new List<GameObject>();

    void Start()
    {
        // 게임 시작(또는 게이트 생성 시) 원하는 수만큼 풀에서 몬스터를 미리 가져옴
        PreFetchMonsters();
    }

    /// <summary>
    /// 오브젝트 풀에서 몬스터를 미리 받아와 비활성화 상태로 보관
    /// </summary>
    private void PreFetchMonsters()
    {
        for (int i = 0; i < prefetchCount; i++)
        {
            // 위치·회전은 임시로 (0,0,0)에 두고, 바로 비활성화 처리
            GameObject monsterObj = MonsterPool.Instance.GetMonster(Vector3.zero, Quaternion.identity);

            // 풀에서 꺼내면 기본적으로 활성화되므로, 다시 비활성화
            monsterObj.SetActive(false);

            // 리스트에 저장
            pooledMonsters.Add(monsterObj);
        }
    }

    /// <summary>
    /// 실제로 몬스터들을 스폰(활성화)하는 메서드
    /// 예: 게이트가 폭발하거나, 일정 시간 후 호출
    /// </summary>
    public void SpawnMonsters()
    {
        // 예시: 게이트 위치 주변에 몬스터를 배치
        for (int i = 0; i < pooledMonsters.Count; i++)
        {
            GameObject monsterObj = pooledMonsters[i];
            if (monsterObj == null) continue;

            // 스폰 위치 설정 (게이트 위치 + 오프셋)
            Vector3 spawnPos = transform.position + spawnOffset * i;

            monsterObj.transform.position = spawnPos;
            monsterObj.transform.rotation = Quaternion.identity;

            // 활성화
            monsterObj.SetActive(true);

            // 만약 Monster 컴포넌트에서 추가로 초기화해야 한다면
            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                monster.ResetStats(); // 혹은 다른 초기화 로직
            }
        }

        Debug.Log($"{pooledMonsters.Count}마리 몬스터 스폰 완료!");

        // 만약 한 번 스폰 후에는 리스트가 필요 없다면, 
        // pooledMonsters.Clear(); 같은 처리를 해도 됨.
    }

    /// <summary>
    /// (선택) 게이트가 끝난 후, 모든 몬스터를 다시 풀로 반환하고 싶다면
    /// </summary>
    public void ReturnAllMonstersToPool()
    {
        foreach (var monsterObj in pooledMonsters)
        {
            if (monsterObj != null)
            {
                MonsterPool.Instance.ReturnMonster(monsterObj);
            }
        }
        pooledMonsters.Clear();
    }
}
