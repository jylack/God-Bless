using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class RegionUIManager : MonoBehaviour
{
    public CameraManager cameraManager;
    public Transform cameraWayPointsParent; // Camera WayPoints ������ �θ� ������Ʈ
    public CinemachineDollyCart dollyCart; // �ϳ��� Dolly Cart ���
    public Dictionary<string, CinemachinePath> dollyPaths = new Dictionary<string, CinemachinePath>();

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();

        if (cameraManager == null)
        {
            Debug.LogError("[RegionManager] CameraManager�� ã�� �� �����ϴ�!");
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

        CinemachinePath generatedPath = GenerateCinemachinePath(cameraWayPointsParent);
        if (generatedPath != null)
        {
            dollyPaths["GeneratedPath"] = generatedPath;
            Debug.Log("[RegionManager] Camera WayPoint�� CinemachinePath�� ��ȯ�Ͽ� ����߽��ϴ�.");
        }

        cameraManager.regionDollyPaths = dollyPaths;
        cameraManager.dollyCart = dollyCart;
    }

    private CinemachinePath GenerateCinemachinePath(Transform wayPointsParent)
    {
        GameObject pathObject = new GameObject("Generated_CinemachinePath");
        CinemachinePath path = pathObject.AddComponent<CinemachinePath>();

        List<Transform> wayPoints = new List<Transform>();

        foreach (Transform wayPoint in wayPointsParent)
        {
            wayPoints.Add(wayPoint);
        }
        
        if (wayPoints.Count == 0)
        {
            Debug.LogError("[RegionManager] Camera Way Points�� �ϳ��� �����ϴ�! CinemachinePath�� ������ �� �����ϴ�.");
            return null;
        }

        path.m_Waypoints = new CinemachinePath.Waypoint[wayPoints.Count];

        for (int i = 0; i < wayPoints.Count; i++)
        {
            path.m_Waypoints[i] = new CinemachinePath.Waypoint
            {
                position = wayPoints[i].localPosition
            };
        }

        


        return path;
    }
}
