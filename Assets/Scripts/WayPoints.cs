using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [Header("지역별 좌표 모음")]
    public List<Transform> PosA = new List<Transform>();
    public List<Transform> PosB = new List<Transform>();
    public List<Transform> PosC = new List<Transform>();
    public List<Transform> PosD = new List<Transform>();
    public List<Transform> PosE = new List<Transform>();
    public List<Transform> PosF = new List<Transform>();
    public List<Transform> PosG = new List<Transform>();
    public List<Transform> PosH = new List<Transform>();
    public List<Transform> PosI = new List<Transform>();

    public List<Transform> GetWayPos(Gate_Way_Group index)
    {
        List<Transform> temp = new List<Transform>();
        
        switch (index)
        {
            case Gate_Way_Group.A:
                temp = PosA;
                break;
            case Gate_Way_Group.B:
                temp = PosB;
                break;
            case Gate_Way_Group.C:
                temp = PosC;
                break;
            case Gate_Way_Group.D:
                temp = PosD;
                break;
            case Gate_Way_Group.E:
                temp = PosE;
                break;
            case Gate_Way_Group.F:
                temp = PosF;
                break;
            case Gate_Way_Group.G:
                temp = PosG;
                break;
            case Gate_Way_Group.H:
                temp = PosH;
                break;
            case Gate_Way_Group.I:
                temp = PosI;
                break;
            default:
                Debug.LogError("잘못된 값이 들어왔습니다.");
                break;
        }
      
        return temp;
    }
}
