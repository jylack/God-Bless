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


    [Header("게이트 등급")]
    public Gate_Class gateClass;

    [Header("오브젝트 풀에서 미리 받을 몬스터 수")]
    public int prefetchCount = 10;

    [Header("몬스터 스폰 위치")]
    public Vector3 spawnOffset = new Vector3(0, 0, 2);

    // 미리 받아놓은 몬스터들을 저장할 리스트
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
    /// 지정된 순찰좌표 세팅할 메서드
    /// </summary>
    /// <param name="pos">그룹내의 좌표들</param>
    public void GateSet(List<Transform> pos)
    {
        //그룹내의 좌표들 인자로 받음
        //그룹내의 좌표들 중에서 랜덤좌표
        var wayRnd = pos[Random.Range(0, pos.Count)];
        //지정

        gameObject.transform.position = wayRnd.position;
    }
}
