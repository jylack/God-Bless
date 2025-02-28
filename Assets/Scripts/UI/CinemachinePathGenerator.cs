using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class CinemachinePathGenerator : MonoBehaviour
{
    public static CinemachineSmoothPath GeneratePath(Gate_Way_Group group)
    {
        // ��������Ʈ ������ ��������
        List<Transform> waypoints = WayPoints.Instance.GetWayPos(group);

        if (waypoints.Count == 0)
        {
            Debug.LogWarning($"[CinemachinePathGenerator] {group} ������ ��������Ʈ�� �����ϴ�!");
            return null;
        }

        // ���ο� GameObject ���� �� CinemachineSmoothPath �߰�
        GameObject pathObject = new GameObject($"CinemachinePath_{group}");
        CinemachineSmoothPath path = pathObject.AddComponent<CinemachineSmoothPath>();

        // CinemachinePath�� ��� ����Ʈ ����
        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[waypoints.Count];

        for (int i = 0; i < waypoints.Count; i++)
        {
            path.m_Waypoints[i] = new CinemachineSmoothPath.Waypoint
            {
                position = waypoints[i].position
            };
        }

        // ��� ������Ʈ
        path.InvalidateDistanceCache();

        Debug.Log($"[CinemachinePathGenerator] {group} ������ CinemachinePath�� �����Ǿ����ϴ�.");

        return path;
    }
}
