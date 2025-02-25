using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class DollyTrackSetting : MonoBehaviour
{
    public List<Transform> trans = new List<Transform>();
    void Start()
    {
        var path = GetComponent<CinemachineSmoothPath>();
        
        if (path != null && trans != null)
        {
            //웨이포인트 사이즈 좌표들 사이즈만큼 ReSize
            path.m_Waypoints = new CinemachineSmoothPath.Waypoint[trans.Count];

            for (int i = 0; i < trans.Count; i++)
            {
                //값 넣어둘 녀석
                CinemachineSmoothPath.Waypoint waypoint = new CinemachineSmoothPath.Waypoint();
                waypoint.position = trans[i].position;//좌표 넣음
                waypoint.roll = 0; //회전값 필요없어서 0으로 초기화 해서 넣어줌. 어차피 나중에 타겟 지정 각으로 볼꺼임
                path.m_Waypoints[i] = waypoint;

            }
            //InvalidateDistanceCache 메서드는 트랙끼리의 거리를 다시 계산하도록 CinemachineSmoothPath에 지시하는 역할
            path.InvalidateDistanceCache();
        }

        //List<Vector3> pos = new List<Vector3>();

        //foreach (var t in trans)
        //{
        //    pos.Add(t.position);
        //}
        //m_Waypoints 까보니 vec3 배열이아니였따....ㅠ
        //waypointsPos = pos;
    }


}
