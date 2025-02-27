using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게이트가 생성될 수 있는 웨이포인트 그룹을 정의하는 Enum
/// </summary>
public enum Gate_Way_Group
{
    A = 0,  // Zone A
    B = 1,  // Zone B
    C = 2,  // Zone C
    D = 3,  // Zone D
    E = 4,  // Zone E
    F = 5,  // Zone F
    G = 6,  // Zone G
    H = 7,  // Zone H
    I = 8   // Zone I
}

public class WayPoints : MonoBehaviour
{
    public static WayPoints Instance;

    [Header("웨이포인트 목록")]
    public Dictionary<Gate_Way_Group, List<Transform>> wayPointDictionary = new Dictionary<Gate_Way_Group, List<Transform>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        LoadWayPoints();
    }

    /// <summary>
    /// `WayPoints` 아래의 `WayPoint_A, WayPoint_B, ...` 자식 오브젝트에서 웨이포인트 로드
    /// </summary>
    private void LoadWayPoints()
    {
        wayPointDictionary.Clear();

        // 최상위 "WayPoints" 오브젝트 찾기
        GameObject rootWayPoints = GameObject.Find("WayPoints");
        if (rootWayPoints == null)
        {
            Debug.LogError("[WayPoints] WayPoints 오브젝트를 찾을 수 없습니다!");
            return;
        }

        // WayPoints 아래의 모든 자식 (WayPoint_A, WayPoint_B, ...)
        foreach (Transform regionGroup in rootWayPoints.transform)
        {
            string groupName = regionGroup.name.Replace("WayPoint_", ""); // "WayPoint_A" → "A"
            if (System.Enum.TryParse(groupName, out Gate_Way_Group groupEnum))
            {
                List<Transform> points = new List<Transform>();

                // 해당 그룹의 모든 웨이포인트 추가
                foreach (Transform wayPoint in regionGroup)
                {
                    points.Add(wayPoint);
                }

                wayPointDictionary[groupEnum] = points;
                //Debug.Log($"[WayPoints] {groupEnum} 지역 웨이포인트 {points.Count}개 로드됨.");
            }
            else
            {
                Debug.LogWarning($"[WayPoints] {groupName}는 유효한 Gate_Way_Group이 아닙니다.");
            }
        }
    }

    /// <summary>
    /// 특정 지역의 웨이포인트 리스트 반환
    /// </summary>
    public List<Transform> GetWayPos(Gate_Way_Group group)
    {
        if (wayPointDictionary.TryGetValue(group, out List<Transform> waypoints))
        {
            return waypoints;
        }
        Debug.LogWarning($"[WayPoints] {group} 지역의 웨이포인트가 없습니다.");
        return new List<Transform>();
    }
}
