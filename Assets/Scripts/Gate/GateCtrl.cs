using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gate_Way_Group
{
    A, B, C, D, E, F, G, H, I, end
}

public enum Gate_Class { F, E, D, C, B, A, S, SS, SSS, end }


public class GateCtrl : MonoBehaviour
{

    public int gateLevel;
    public NeighborhoodCtrl neighborhood;
    public GameObject monsterPrefab;
    public float explosionTime = 30f;


    [Header("����Ʈ ���")]
    public Gate_Class gateClass;

    [Header("������Ʈ Ǯ���� �̸� ���� ���� ��")]
    public int prefetchCount = 10;

    [Header("���� ���� ��ġ")]
    public Vector3 spawnOffset = new Vector3(0, 0, 2);

    // �̸� �޾Ƴ��� ���͵��� ������ ����Ʈ
    private List<GameObject> pooledMonsters = new List<GameObject>();

    public void SetRegion(NeighborhoodCtrl value)
    {
        neighborhood = value;
    }

    public void SetMonster(GameObject value) 
    {
        monsterPrefab = value; 
    }

    void Start()
    {
        StartCoroutine(GateTimer());
    }

    IEnumerator GateTimer()
    {
        yield return new WaitForSeconds(explosionTime);
        ExplodeGate();
    }

    public void ExplodeGate()
    {
        //int monsterCount = GetMonsterSpawnCount();
        for (int i = 0; i < prefetchCount; i++)
        {
            Instantiate(monsterPrefab, transform.position + new Vector3(i, 0, 0), Quaternion.identity,GameManager.Instance.MonsterOBJ.transform);
        }

        neighborhood.RemoveGate(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// ������ ������ǥ ������ �޼���
    /// </summary>
    /// <param name="pos">�׷쳻�� ��ǥ��</param>
    public void GateSet(List<Transform> pos)
    {
        //�׷쳻�� ��ǥ�� ���ڷ� ����
        //�׷쳻�� ��ǥ�� �߿��� ������ǥ
        var wayRnd = pos[Random.Range(0, pos.Count)];
        //����

        gameObject.transform.position = wayRnd.position;
    }
}
