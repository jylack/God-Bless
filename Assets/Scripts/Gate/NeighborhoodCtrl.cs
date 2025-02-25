using System.Collections.Generic;
using UnityEngine;

//�� ���׸��� ���� Ŭ����
//A , B C D ��� ���� �̿��ҵ�
public class NeighborhoodCtrl : MonoBehaviour
{
    [Header("����Ʈ ���� ����")]
    public Gate_Way_Group neighborhood;
    public int nei_level;
    public int maxGates;

    //���� ����Ʈ 
    List<GateCtrl> gates = new List<GateCtrl>();
    public List<GateCtrl> Gates { set => gates = value; }

    List<Transform> wayPos;

    private void Start()
    {
        wayPos = GameObject.Find("WayPoints").GetComponent<WayPoints>().GetWayPos(neighborhood);

        //������ �ִ� ����Ʈ�� ���� ����
        foreach (var g in gates)
        {
            g.GateSet(wayPos);            
        }
    }

    private void FixedUpdate()
    {
        SpawnGate(GameManager.Instance.dataBase.gateTypes[0]);
    }


    public void SpawnGate(GameObject gatePrefab)
    {
        if (gates.Count >= maxGates) return;

        GameObject gateObj = Instantiate(gatePrefab, GetRandomPosition(), Quaternion.identity);
        GateCtrl gate = gateObj.GetComponent<GateCtrl>();
        gate.SetRegion(this);
        gate.SetMonster(GameManager.Instance.dataBase.GetMonster(MonsterName.Slime));
        gates.Add(gate);
    }

    public void RemoveGate(GateCtrl gate)
    {
        gates.Remove(gate);
    }

    private Vector3 GetRandomPosition()
    {
        var rndIndex = Random.Range(0, wayPos.Count);

        return wayPos[rndIndex].position + new Vector3(0,1f,0);
    }

}


