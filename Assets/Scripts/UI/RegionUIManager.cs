using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class RegionUIManager : MonoBehaviour
{
    public CameraManager cameraManager;
    public Transform cameraWayPointsParent; // Camera WayPoints 저장할 부모 오브젝트
    public CinemachineDollyCart dollyCart; // 하나의 Dolly Cart 사용
    public Dictionary<string, CinemachinePath> dollyPaths = new Dictionary<string, CinemachinePath>();

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();

        if (cameraManager == null)
        {
            Debug.LogError("[RegionManager] CameraManager를 찾을 수 없습니다!");
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
            Debug.Log("[RegionManager] Camera WayPoint를 CinemachinePath로 변환하여 등록했습니다.");
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
            Debug.LogError("[RegionManager] Camera Way Points가 하나도 없습니다! CinemachinePath를 생성할 수 없습니다.");
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
