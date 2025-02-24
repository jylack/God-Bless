using System.Collections.Generic;
using UnityEngine;

//한 동네마다 있을 클래스
//A , B C D 등등 동을 이용할듯
public class NeighborhoodCtrl : MonoBehaviour
{
    [Header("게이트 생성 지역")]
    public Gate_Way_Group neighborhood;
    public int nei_level;
    public int maxGates;

    //지역 게이트 
    List<GateCtrl> gates = new List<GateCtrl>();
    public List<GateCtrl> Gates { set => gates = value; }


    private void Start()
    {
        var wayPos = GameObject.Find("WayPoints").GetComponent<WayPoints>().GetWayPos(neighborhood);

        //가지고 있는 게이트에 지역 세팅
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


