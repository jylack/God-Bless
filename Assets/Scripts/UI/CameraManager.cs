using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineDollyCart dollyCart;
    public Dictionary<string, CinemachineSmoothPath> regionDollyPaths = new Dictionary<string, CinemachineSmoothPath>();

    public Transform player; // GOD (플레이어 역할)

    private Transform selectedUnit = null; // 현재 선택된 유닛
    private string currentRegion = "I"; // 기본 지역

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
            pov.m_HorizontalAxis.m_MaxSpeed = 300f; // 좌우 회전 속도 조정
            pov.m_VerticalAxis.m_MaxSpeed = 200f;   // 상하 회전 속도 조정
            pov.m_VerticalAxis.m_MinValue = -30f;  // 최소 회전 각도 제한
            pov.m_VerticalAxis.m_MaxValue = 60f;   // 최대 회전 각도 제한
        }
        
        StartCoroutine(InitializeCamera());
    }


    private IEnumerator InitializeCamera()
    {
        Debug.Log("[CameraManager] 카메라 초기화 시작");

        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCamera == null)
            {
                Debug.LogError("[CameraManager] CinemachineVirtualCamera를 찾을 수 없습니다!");
                yield break;
            }
        }

        if (player == null)
        {
            GameObject god = GameObject.FindGameObjectWithTag("GOD");
            if (god != null)
            {
                player = god.transform;
                Debug.Log("[CameraManager] GOD를 찾음");
            }
            else
            {
                Debug.LogError("[CameraManager] GOD를 찾을 수 없습니다!");
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
            Debug.LogWarning($"[CameraManager] {region} 지역의 CinemachinePath가 없습니다! 자동 생성 시도.");
            newPath = GenerateDollyTrack(region);
            if (newPath == null)
            {
                Debug.LogError($"[CameraManager] {region} 지역의 CinemachinePath 생성 실패!");
                return;
            }
            regionDollyPaths[region] = newPath;
        }

        if (newPath != null) //  예외 처리 추가
        {
            SetDollyTrack(newPath);
        }
    }


    //지역별로 CinemachineSmoothPath 생성
    private CinemachineSmoothPath GenerateDollyTrack(string regionName)
    {
        if (!System.Enum.TryParse(regionName, out Gate_Way_Group regionEnum))
        {
            Debug.LogError($"[CameraManager] {regionName} 지역이 Gate_Way_Group Enum에 존재하지 않습니다!");
            return null;
        }

        List<Transform> waypoints = WayPoints.Instance.GetWayPos(regionEnum);
        if (waypoints.Count == 0)
        {
            Debug.LogWarning($"[CameraManager] {regionEnum} 지역의 웨이포인트가 없습니다!");
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
        path.m_Looped = true;//만든 트랙 루프 활성화
        path.InvalidateDistanceCache();
        Debug.Log($"[CameraManager] {regionEnum} 지역의 CinemachineSmoothPath가 생성되었습니다.");
        return path;
    }

    private void SetDollyTrack(CinemachineSmoothPath newPath)
    {
        if (dollyCart == null)
        {
            Debug.LogError("[CameraManager] CinemachineDollyCart가 설정되지 않았습니다!");
            return;
        }

        dollyCart.m_Path = newPath;
        dollyCart.m_Position = 0f;
        dollyCart.m_Speed = 1f;


        virtualCamera.Follow = dollyCart.transform;
        virtualCamera.LookAt = dollyCart.transform;

        Debug.Log("[CameraManager] Dolly Track으로 이동, LookAtTarget 설정됨.");
    }

    public void FollowUnit(Transform unit)
    {
        if (unit != null)
        {
            selectedUnit = unit;//선택된 유닛을 저장하여 추적 유지
            virtualCamera.Follow = unit;
            virtualCamera.LookAt = unit;
            Debug.Log($"[CameraManager] {unit.name} 유닛을 따라감");
        }
    }

    public void UnfollowUnit()
    {
        selectedUnit = null;//유닛 따라가기 해제

        if (regionDollyPaths.TryGetValue(CurrentRegion, out CinemachineSmoothPath newPath))
        {
            SetDollyTrack(newPath);
            Debug.Log($"[CameraManager] {CurrentRegion} 지역 Dolly Track으로 복귀.");
        }
        else
        {
            Debug.LogWarning($"[CameraManager] {CurrentRegion} 지역의 Dolly Track이 없습니다. 기본 위치 유지.");
        }
    }

    private void Update()
    {
        if (dollyCart != null)//유닛을 따라가는 중이 아닐 때만 자동 이동
        {
            dollyCart.m_Position += Time.deltaTime * dollyCart.m_Speed;
        }


        HandleMouseMove();
    }

    private void HandleMouseMove()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭 시작
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1)) // 우클릭 해제
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = (Input.mousePosition - lastMousePosition);

            // 이동 속도 배율 조정 (X축과 Y축의 민감도를 다르게 설정 가능)
            float adjustedX = mouseDelta.x * moveSpeed * Time.deltaTime;
            float adjustedY = -mouseDelta.y * moveSpeed * Time.deltaTime; // 위아래 반전

            lastMousePosition = Input.mousePosition;

            // 새로운 위치 계산
            Vector3 newLocalPos = dollyCart.transform.localPosition + new Vector3(adjustedX, adjustedY, 0);

            // 이동 거리 제한 (최대 이동 거리 유지)
            newLocalPos = Vector3.ClampMagnitude(newLocalPos - defaultOffset, maxMoveDistance) + defaultOffset;


        }
    }

}
