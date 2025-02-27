using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ʈ�� ������ �� �ִ� ��������Ʈ �׷��� �����ϴ� Enum
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

    [Header("��������Ʈ ���")]
    public Dictionary<Gate_Way_Group, List<Transform>> wayPointDictionary = new Dictionary<Gate_Way_Group, List<Transform>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        LoadWayPoints();
    }

    /// <summary>
    /// `WayPoints` �Ʒ��� `WayPoint_A, WayPoint_B, ...` �ڽ� ������Ʈ���� ��������Ʈ �ε�
    /// </summary>
    private void LoadWayPoints()
    {
        wayPointDictionary.Clear();

        // �ֻ��� "WayPoints" ������Ʈ ã��
        GameObject rootWayPoints = GameObject.Find("WayPoints");
        if (rootWayPoints == null)
        {
            Debug.LogError("[WayPoints] WayPoints ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        // WayPoints �Ʒ��� ��� �ڽ� (WayPoint_A, WayPoint_B, ...)
        foreach (Transform regionGroup in rootWayPoints.transform)
        {
            string groupName = regionGroup.name.Replace("WayPoint_", ""); // "WayPoint_A" �� "A"
            if (System.Enum.TryParse(groupName, out Gate_Way_Group groupEnum))
            {
                List<Transform> points = new List<Transform>();

                // �ش� �׷��� ��� ��������Ʈ �߰�
                foreach (Transform wayPoint in regionGroup)
                {
                    points.Add(wayPoint);
                }

                wayPointDictionary[groupEnum] = points;
                //Debug.Log($"[WayPoints] {groupEnum} ���� ��������Ʈ {points.Count}�� �ε��.");
            }
            else
            {
                Debug.LogWarning($"[WayPoints] {groupName}�� ��ȿ�� Gate_Way_Group�� �ƴմϴ�.");
            }
        }
    }

    /// <summary>
    /// Ư�� ������ ��������Ʈ ����Ʈ ��ȯ
    /// </summary>
    public List<Transform> GetWayPos(Gate_Way_Group group)
    {
        if (wayPointDictionary.TryGetValue(group, out List<Transform> waypoints))
        {
            return waypoints;
        }
        Debug.LogWarning($"[WayPoints] {group} ������ ��������Ʈ�� �����ϴ�.");
        return new List<Transform>();
    }
}
