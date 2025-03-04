using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class RegionUIManager : MonoBehaviour
{
    public CameraManager cameraManager;
    public Transform cameraWayPointsParent; // Camera WayPoints 저장할 부모 오브젝트
    public CinemachineDollyCart dollyCart; // 하나의 Dolly Cart 사용
    public Dictionary<string, CinemachineSmoothPath> dollyPaths = new Dictionary<string, CinemachineSmoothPath>();

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();

        if (cameraManager == null)
        {
            Debug.LogError("[RegionUIManager] CameraManager를 찾을 수 없습니다!");
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

        // CameraManager의 regionDollyPaths와 동기화 확인
        if (!cameraManager.regionDollyPaths.ContainsKey(regionName))
        {
            CinemachineSmoothPath generatedPath = GenerateCinemachinePath(cameraWayPointsParent);
            if (generatedPath != null)
            {
                cameraManager.regionDollyPaths[regionName] = generatedPath;
                //Debug.Log($"[RegionUIManager] {regionName} 경로가 생성되었습니다.");
            }
        }
    }


    /// <summary>
    /// 웨이포인트를 사용하여 CinemachineSmoothPath 생성
    /// </summary>
    private CinemachineSmoothPath GenerateCinemachinePath(Transform wayPointsParent)
    {
        if (wayPointsParent == null)
        {
            Debug.LogError("[RegionUIManager] Camera WayPoints 부모가 설정되지 않았습니다!");
            return null;
        }

        // 새로운 GameObject 생성 및 CinemachineSmoothPath 추가
        GameObject pathObject = new GameObject("Generated_CinemachinePath");
        CinemachineSmoothPath path = pathObject.AddComponent<CinemachineSmoothPath>();

        // 생성된 경로를 CameraWayPoints의 자식으로 설정 (씬 정리)
        pathObject.transform.SetParent(wayPointsParent);

        List<Transform> wayPoints = new List<Transform>();

        foreach (Transform wayPoint in wayPointsParent)
        {
            wayPoints.Add(wayPoint);
        }

        if (wayPoints.Count == 0)
        {
            Debug.LogError("[RegionUIManager] Camera Way Points가 없습니다! CinemachineSmoothPath를 생성할 수 없습니다.");
            return null;
        }

        // CinemachinePath의 노드 리스트 설정
        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[wayPoints.Count];

        for (int i = 0; i < wayPoints.Count; i++)
        {
            path.m_Waypoints[i] = new CinemachineSmoothPath.Waypoint
            {
                position = wayPoints[i].localPosition
            };
        }

        path.InvalidateDistanceCache(); // 경로 데이터 업데이트

        return path;
    }
}
