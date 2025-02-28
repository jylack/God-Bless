using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class CinemachinePathGenerator : MonoBehaviour
{
    public static CinemachineSmoothPath GeneratePath(Gate_Way_Group group)
    {
        // 웨이포인트 데이터 가져오기
        List<Transform> waypoints = WayPoints.Instance.GetWayPos(group);

        if (waypoints.Count == 0)
        {
            Debug.LogWarning($"[CinemachinePathGenerator] {group} 지역의 웨이포인트가 없습니다!");
            return null;
        }

        // 새로운 GameObject 생성 및 CinemachineSmoothPath 추가
        GameObject pathObject = new GameObject($"CinemachinePath_{group}");
        CinemachineSmoothPath path = pathObject.AddComponent<CinemachineSmoothPath>();

        // CinemachinePath의 노드 리스트 설정
        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[waypoints.Count];

        for (int i = 0; i < waypoints.Count; i++)
        {
            path.m_Waypoints[i] = new CinemachineSmoothPath.Waypoint
            {
                position = waypoints[i].position
            };
        }

        // 경로 업데이트
        path.InvalidateDistanceCache();

        Debug.Log($"[CinemachinePathGenerator] {group} 지역의 CinemachinePath가 생성되었습니다.");

        return path;
    }
}
