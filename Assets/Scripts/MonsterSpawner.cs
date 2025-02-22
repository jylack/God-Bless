using System.Collections.Generic;
using UnityEngine;

public enum Gate_Way_Group
{
    A, B, C, D, E, F, G, H, I, end
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("������Ʈ Ǯ���� �̸� ���� ���� ��")]
    public int prefetchCount = 5;

    [Header("���� ���� ��ġ")]
    public Vector3 spawnOffset = new Vector3(0, 0, 2);

    // �̸� �޾Ƴ��� ���͵��� ������ ����Ʈ
    private List<GameObject> pooledMonsters = new List<GameObject>();

    Gate_Way_Group group;

    public void GateSet(Gate_Way_Group posName, List<UnitCtrl> unit)
    {
        //�׷쳻�� ��ǥ��
        var wayPos = GameObject.Find("WayPoints").GetComponent<WayPoints>().GetWayPos(posName);
        //�׷쳻�� ��ǥ�� �߿��� ������ǥ
        var wayRnd = wayPos[Random.Range(0, wayPos.Count)];
        //����
        spawnOffset = wayRnd.position;
        group = posName;
    }

    void Start()
    {
        // ���� ����(�Ǵ� ����Ʈ ���� ��) ���ϴ� ����ŭ Ǯ���� ���͸� �̸� ������
        PreFetchMonsters();
        //�ӽ÷� ����Ʈ �����ڸ��� ��ȯ�Ǵ��� Ȯ���ڵ�
        SpawnMonsters();
    }

    /// <summary>
    /// ������Ʈ Ǯ���� ���͸� �̸� �޾ƿ� ��Ȱ��ȭ ���·� ����
    /// </summary>
    private void PreFetchMonsters()
    {

        for (int i = 0; i < prefetchCount; i++)
        {
            // ��ġ��ȸ���� �ӽ÷� (0,0,0)�� �ΰ�, �ٷ� ��Ȱ��ȭ ó��
            GameObject monsterObj = MonsterPool.Instance.GetMonster(Vector3.zero, Quaternion.identity);

            // Ǯ���� ������ �⺻������ Ȱ��ȭ�ǹǷ�, �ٽ� ��Ȱ��ȭ
            monsterObj.SetActive(false);

            // ����Ʈ�� ����
            pooledMonsters.Add(monsterObj);
        }
    }

    /// <summary>
    /// ������ ���͵��� ����(Ȱ��ȭ)�ϴ� �޼���
    /// ��: ����Ʈ�� �����ϰų�, ���� �ð� �� ȣ��
    /// </summary>
    public void SpawnMonsters()
    {
        // ����: ����Ʈ ��ġ �ֺ��� ���͸� ��ġ
        for (int i = 0; i < pooledMonsters.Count; i++)
        {
            GameObject monsterObj = pooledMonsters[i];
            if (monsterObj == null) continue;

            // ���� ��ġ ���� (����Ʈ ��ġ + ������)
            Vector3 spawnPos = transform.position + spawnOffset * i;

            monsterObj.transform.position = spawnPos;
            monsterObj.transform.rotation = Quaternion.identity;

            // Ȱ��ȭ
            monsterObj.SetActive(true);

            // ���� Monster ������Ʈ���� �߰��� �ʱ�ȭ�ؾ� �Ѵٸ�
            UnitCtrl monster = monsterObj.GetComponent<UnitCtrl>();
            if (monster != null)
            {
                monster.ResetStats(); // Ȥ�� �ٸ� �ʱ�ȭ ����
            }
        }

        Debug.Log($"{pooledMonsters.Count}���� ���� ���� �Ϸ�!");

        // ���� �� �� ���� �Ŀ��� ����Ʈ�� �ʿ� ���ٸ�, 
        // pooledMonsters.Clear(); ���� ó���� �ص� ��.
    }

    /// <summary>
    /// (����) ����Ʈ�� ���� ��, ��� ���͸� �ٽ� Ǯ�� ��ȯ�ϰ� �ʹٸ�
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
