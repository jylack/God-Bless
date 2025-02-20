using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public static MonsterPool Instance;

    [Header("���� ������")]
    public GameObject monsterPrefab;

    [Header("�ʱ� ���� ��")]
    public int initialCount = 10;

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        // �̱��� ����
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // �ʱ� ������ŭ �̸� ���� �� ��Ȱ��ȭ
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
            // Ǯ�� ��� ������ ���� ����(�ڵ� Ȯ��)
            monsterObj = Instantiate(monsterPrefab);
        }

        // ��ġ��ȸ�� ���� �� Ȱ��ȭ
        monsterObj.transform.position = position;
        monsterObj.transform.rotation = rotation;
        monsterObj.SetActive(true);

        // ���� ���� �ʱ�ȭ (����)
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
