using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class RegionUIManager : MonoBehaviour
{
    public CameraManager cameraManager;
    public Transform cameraWayPointsParent; // Camera WayPoints ������ �θ� ������Ʈ
    public CinemachineDollyCart dollyCart; // �ϳ��� Dolly Cart ���
    public Dictionary<string, CinemachineSmoothPath> dollyPaths = new Dictionary<string, CinemachineSmoothPath>();

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();

        if (cameraManager == null)
        {
            Debug.LogError("[RegionUIManager] CameraManager�� ã�� �� �����ϴ�!");
            return;
        }

        if (cameraWayPointsParent == null)
        {
            GameObject wayPointsObject = GameObject.Find("CameraWayPoints");
            if (wayPointsObject != null)
            {
                cameraWayPointsParent = wayPointsObject.transform;
            }
        }

        string regionName = "GeneratedPath";

        // CameraManager�� regionDollyPaths�� ����ȭ Ȯ��
        if (!cameraManager.regionDollyPaths.ContainsKey(regionName))
        {
            CinemachineSmoothPath generatedPath = GenerateCinemachinePath(cameraWayPointsParent);
            if (generatedPath != null)
            {
                cameraManager.regionDollyPaths[regionName] = generatedPath;
                //Debug.Log($"[RegionUIManager] {regionName} ��ΰ� �����Ǿ����ϴ�.");
            }
        }
    }


    /// <summary>
    /// ��������Ʈ�� ����Ͽ� CinemachineSmoothPath ����
    /// </summary>
    private CinemachineSmoothPath GenerateCinemachinePath(Transform wayPointsParent)
    {
        if (wayPointsParent == null)
        {
            Debug.LogError("[RegionUIManager] Camera WayPoints �θ� �������� �ʾҽ��ϴ�!");
            return null;
        }

        // ���ο� GameObject ���� �� CinemachineSmoothPath �߰�
        GameObject pathObject = new GameObject("Generated_CinemachinePath");
        CinemachineSmoothPath path = pathObject.AddComponent<CinemachineSmoothPath>();

        // ������ ��θ� CameraWayPoints�� �ڽ����� ���� (�� ����)
        pathObject.transform.SetParent(wayPointsParent);

        List<Transform> wayPoints = new List<Transform>();

        foreach (Transform wayPoint in wayPointsParent)
        {
            wayPoints.Add(wayPoint);
        }

        if (wayPoints.Count == 0)
        {
            Debug.LogError("[RegionUIManager] Camera Way Points�� �����ϴ�! CinemachineSmoothPath�� ������ �� �����ϴ�.");
            return null;
        }

        // CinemachinePath�� ��� ����Ʈ ����
        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[wayPoints.Count];

        for (int i = 0; i < wayPoints.Count; i++)
        {
            path.m_Waypoints[i] = new CinemachineSmoothPath.Waypoint
            {
                position = wayPoints[i].localPosition
            };
        }

        path.InvalidateDistanceCache(); // ��� ������ ������Ʈ

        return path;
    }
}
