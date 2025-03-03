using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineDollyCart dollyCart;
    public Dictionary<string, CinemachineSmoothPath> regionDollyPaths = new Dictionary<string, CinemachineSmoothPath>();

    public Transform player; // GOD (�÷��̾� ����)

    private Transform selectedUnit = null; // ���� ���õ� ����
    private string currentRegion = "I"; // �⺻ ����

    private bool isDragging = false;
    private Vector3 lastMousePosition;
    public float moveSpeed = 0.1f;
    public Vector3 defaultOffset = new Vector3(-15f, -5f, 0);
    public float maxMoveDistance = 3f;


    public static CameraManager Instance;

    public string CurrentRegion { get => currentRegion; protected set => currentRegion = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        var pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (pov != null)
        {
            pov.m_HorizontalAxis.m_MaxSpeed = 300f; // �¿� ȸ�� �ӵ� ����
            pov.m_VerticalAxis.m_MaxSpeed = 200f;   // ���� ȸ�� �ӵ� ����
            pov.m_VerticalAxis.m_MinValue = -30f;  // �ּ� ȸ�� ���� ����
            pov.m_VerticalAxis.m_MaxValue = 60f;   // �ִ� ȸ�� ���� ����
        }
        
        StartCoroutine(InitializeCamera());
    }


    private IEnumerator InitializeCamera()
    {
        Debug.Log("[CameraManager] ī�޶� �ʱ�ȭ ����");

        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCamera == null)
            {
                Debug.LogError("[CameraManager] CinemachineVirtualCamera�� ã�� �� �����ϴ�!");
                yield break;
            }
        }

        if (player == null)
        {
            GameObject god = GameObject.FindGameObjectWithTag("GOD");
            if (god != null)
            {
                player = god.transform;
                Debug.Log("[CameraManager] GOD�� ã��");
            }
            else
            {
                Debug.LogError("[CameraManager] GOD�� ã�� �� �����ϴ�!");
                yield break;
            }
        }

        yield return new WaitForSeconds(1f);

        MoveToRegion(CurrentRegion);
    }

    public void MoveToRegion(string region)
    {
        CurrentRegion = region;

        if (!regionDollyPaths.TryGetValue(region, out CinemachineSmoothPath newPath))
        {
            Debug.LogWarning($"[CameraManager] {region} ������ CinemachinePath�� �����ϴ�! �ڵ� ���� �õ�.");
            newPath = GenerateDollyTrack(region);
            if (newPath == null)
            {
                Debug.LogError($"[CameraManager] {region} ������ CinemachinePath ���� ����!");
                return;
            }
            regionDollyPaths[region] = newPath;
        }

        if (newPath != null) //  ���� ó�� �߰�
        {
            SetDollyTrack(newPath);
        }
    }


    //�������� CinemachineSmoothPath ����
    private CinemachineSmoothPath GenerateDollyTrack(string regionName)
    {
        if (!System.Enum.TryParse(regionName, out Gate_Way_Group regionEnum))
        {
            Debug.LogError($"[CameraManager] {regionName} ������ Gate_Way_Group Enum�� �������� �ʽ��ϴ�!");
            return null;
        }

        List<Transform> waypoints = WayPoints.Instance.GetWayPos(regionEnum);
        if (waypoints.Count == 0)
        {
            Debug.LogWarning($"[CameraManager] {regionEnum} ������ ��������Ʈ�� �����ϴ�!");
            return null;
        }

        GameObject pathObject = new GameObject($"CinemachinePath_{regionName}");
        CinemachineSmoothPath path = pathObject.AddComponent<CinemachineSmoothPath>();

        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[waypoints.Count];

        for (int i = 0; i < waypoints.Count; i++)
        {
            path.m_Waypoints[i] = new CinemachineSmoothPath.Waypoint
            {
                position = waypoints[i].position
            };
        }
        path.m_Looped = true;//���� Ʈ�� ���� Ȱ��ȭ
        path.InvalidateDistanceCache();
        Debug.Log($"[CameraManager] {regionEnum} ������ CinemachineSmoothPath�� �����Ǿ����ϴ�.");
        return path;
    }

    private void SetDollyTrack(CinemachineSmoothPath newPath)
    {
        if (dollyCart == null)
        {
            Debug.LogError("[CameraManager] CinemachineDollyCart�� �������� �ʾҽ��ϴ�!");
            return;
        }

        dollyCart.m_Path = newPath;
        dollyCart.m_Position = 0f;
        dollyCart.m_Speed = 1f;


        virtualCamera.Follow = dollyCart.transform;
        virtualCamera.LookAt = dollyCart.transform;

        Debug.Log("[CameraManager] Dolly Track���� �̵�, LookAtTarget ������.");
    }

    public void FollowUnit(Transform unit)
    {
        if (unit != null)
        {
            selectedUnit = unit;//���õ� ������ �����Ͽ� ���� ����
            virtualCamera.Follow = unit;
            virtualCamera.LookAt = unit;
            Debug.Log($"[CameraManager] {unit.name} ������ ����");
        }
    }

    public void UnfollowUnit()
    {
        selectedUnit = null;//���� ���󰡱� ����

        if (regionDollyPaths.TryGetValue(CurrentRegion, out CinemachineSmoothPath newPath))
        {
            SetDollyTrack(newPath);
            Debug.Log($"[CameraManager] {CurrentRegion} ���� Dolly Track���� ����.");
        }
        else
        {
            Debug.LogWarning($"[CameraManager] {CurrentRegion} ������ Dolly Track�� �����ϴ�. �⺻ ��ġ ����.");
        }
    }

    private void Update()
    {
        if (dollyCart != null)//������ ���󰡴� ���� �ƴ� ���� �ڵ� �̵�
        {
            dollyCart.m_Position += Time.deltaTime * dollyCart.m_Speed;
        }


        HandleMouseMove();
    }

    private void HandleMouseMove()
    {
        if (Input.GetMouseButtonDown(1)) // ��Ŭ�� ����
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1)) // ��Ŭ�� ����
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = (Input.mousePosition - lastMousePosition);

            // �̵� �ӵ� ���� ���� (X��� Y���� �ΰ����� �ٸ��� ���� ����)
            float adjustedX = mouseDelta.x * moveSpeed * Time.deltaTime;
            float adjustedY = -mouseDelta.y * moveSpeed * Time.deltaTime; // ���Ʒ� ����

            lastMousePosition = Input.mousePosition;

            // ���ο� ��ġ ���
            Vector3 newLocalPos = dollyCart.transform.localPosition + new Vector3(adjustedX, adjustedY, 0);

            // �̵� �Ÿ� ���� (�ִ� �̵� �Ÿ� ����)
            newLocalPos = Vector3.ClampMagnitude(newLocalPos - defaultOffset, maxMoveDistance) + defaultOffset;


        }
    }

}
