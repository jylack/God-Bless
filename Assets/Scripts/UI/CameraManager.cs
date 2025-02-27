using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineDollyCart dollyCart;
    public Dictionary<string, CinemachinePath> regionDollyPaths = new Dictionary<string, CinemachinePath>();
    public Transform player; // GOD (플레이어 역할)

    private Transform selectedUnit = null; // 현재 선택된 유닛
    private string currentRegion = "A"; // 기본 지역
    private bool isFollowingUnit = false;

    private Vector3 lastMousePosition;
    private bool isDragging = false;
    public float rotationSpeed = 2f; // 마우스 회전 속도

    private void Start()
    {
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

        LoadDollyTracks(); // Dolly Track 불러오기

        MoveToRegion(currentRegion);
    }

    /// <summary>
    /// 씬에서 Dolly Track을 찾아서 등록
    /// </summary>
    private void LoadDollyTracks()
    {
        regionDollyPaths.Clear();
        CinemachinePath[] allPaths = FindObjectsOfType<CinemachinePath>();

        foreach (var path in allPaths)
        {
            Debug.Log($"[CameraManager] 발견된 Dolly Track: {path.name}");
            regionDollyPaths[path.name] = path;
        }

        Debug.Log($"[CameraManager] 초기화된 Dolly Tracks 개수: {regionDollyPaths.Count}");
    }

    /// <summary>
    /// 특정 지역으로 이동
    /// </summary>
    public void MoveToRegion(string region)
    {
        currentRegion = region;
        isFollowingUnit = false; // 지역 이동 시 트랙 따라 이동하도록 설정

        if (regionDollyPaths.TryGetValue(region, out CinemachinePath newPath) && newPath != null)
        {
            SetDollyTrack(newPath);
        }
        else
        {
            Debug.LogError($"[CameraManager] {region} 지역의 CinemachinePath를 찾을 수 없습니다!");
        }
    }

    /// <summary>
    /// Dolly Track을 따라 이동
    /// </summary>
    private void SetDollyTrack(CinemachinePath newPath)
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

        Debug.Log($"[CameraManager] {newPath.name} Dolly Track으로 이동");
    }

    /// <summary>
    /// 유닛이 생성되면 해당 유닛을 따라가기
    /// </summary>
    public void FollowUnit(Transform unit)
    {
        if (unit == null)
        {
            Debug.LogWarning("[CameraManager] 선택한 유닛이 없습니다!");
            return;
        }

        isFollowingUnit = true;
        selectedUnit = unit;
        virtualCamera.Follow = selectedUnit;
        virtualCamera.LookAt = selectedUnit;

        Debug.Log($"[CameraManager] {selectedUnit.name} 유닛을 따라감");
    }

    /// <summary>
    /// 유닛을 따라가기를 중단하고 다시 Dolly Track을 따라 이동
    /// </summary>
    public void UnfollowUnit()
    {
        selectedUnit = null;
        isFollowingUnit = false;

        if (regionDollyPaths.TryGetValue(currentRegion, out CinemachinePath newPath) && newPath != null)
        {
            SetDollyTrack(newPath);
            Debug.Log($"[CameraManager] {currentRegion} 지역 Dolly Track으로 복귀.");
        }
        else
        {
            Debug.LogWarning($"[CameraManager] {currentRegion} 지역의 Dolly Track이 없습니다. 기본 위치 유지.");
        }
    }

    /// <summary>
    /// 마우스 드래그로 카메라 회전
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            if (virtualCamera.LookAt != null)
            {
                virtualCamera.transform.RotateAround(virtualCamera.LookAt.position, Vector3.up, delta.x * rotationSpeed * Time.deltaTime);
            }
        }
    }
}
