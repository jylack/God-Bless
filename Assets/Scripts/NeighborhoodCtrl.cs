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


    private void Start()
    {
        var wayPos = GameObject.Find("WayPoints").GetComponent<WayPoints>().GetWayPos(neighborhood);

        //������ �ִ� ����Ʈ�� ���� ����
        foreach (var g in gates)
        {
            g.GateSet(wayPos);            
        }

    }
    public void SpawnGate(GameObject gatePrefab)
    {
        if (gates.Count >= maxGates) return;

        GameObject gateObj = Instantiate(gatePrefab, GetRandomPosition(), Quaternion.identity);
        GateCtrl gate = gateObj.GetComponent<GateCtrl>();
        gate.SetRegion(this);
        gates.Add(gate);
    }

    public void RemoveGate(GateCtrl gate)
    {
        gates.Remove(gate);
    }

    private Vector3 GetRandomPosition()
    {
        return transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

}


