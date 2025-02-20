using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public static MonsterPool Instance;

    [Header("몬스터 프리팹")]
    public GameObject monsterPrefab;

    [Header("초기 생성 수")]
    public int initialCount = 10;

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 초기 개수만큼 미리 생성 후 비활성화
        for (int i = 0; i < initialCount; i++)
        {
            GameObject newObj = Instantiate(monsterPrefab);
            newObj.SetActive(false);
            poolQueue.Enqueue(newObj);
        }
    }

    public GameObject GetMonster(Vector3 position, Quaternion rotation)
    {
        GameObject monsterObj;

        if (poolQueue.Count > 0)
        {
            monsterObj = poolQueue.Dequeue();
        }
        else
        {
            // 풀이 비어 있으면 새로 생성(자동 확장)
            monsterObj = Instantiate(monsterPrefab);
        }

        // 위치·회전 세팅 후 활성화
        monsterObj.transform.position = position;
        monsterObj.transform.rotation = rotation;
        monsterObj.SetActive(true);

        // 몬스터 스탯 초기화 (예시)
        UnitCtrl monster = monsterObj.GetComponent<UnitCtrl>();
        if (monster != null)
        {
            monster.ResetStats();
        }

        return monsterObj;
    }

    public void ReturnMonster(GameObject monsterObj)
    {
        monsterObj.SetActive(false);
        poolQueue.Enqueue(monsterObj);
    }
}
