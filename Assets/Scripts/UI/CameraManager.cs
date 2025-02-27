using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineDollyCart dollyCart;
    public Dictionary<string, CinemachinePath> regionDollyPaths = new Dictionary<string, CinemachinePath>();
    public Transform player; // GOD (�÷��̾� ����)

    private Transform selectedUnit = null; // ���� ���õ� ����
    private string currentRegion = "A"; // �⺻ ����
    private bool isFollowingUnit = false;

    private Vector3 lastMousePosition;
    private bool isDragging = false;
    public float rotationSpeed = 2f; // ���콺 ȸ�� �ӵ�

    private void Start()
    {
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

        LoadDollyTracks(); // Dolly Track �ҷ�����

        MoveToRegion(currentRegion);
    }

    /// <summary>
    /// ������ Dolly Track�� ã�Ƽ� ���
    /// </summary>
    private void LoadDollyTracks()
    {
        regionDollyPaths.Clear();
        CinemachinePath[] allPaths = FindObjectsOfType<CinemachinePath>();

        foreach (var path in allPaths)
        {
            Debug.Log($"[CameraManager] �߰ߵ� Dolly Track: {path.name}");
            regionDollyPaths[path.name] = path;
        }

        Debug.Log($"[CameraManager] �ʱ�ȭ�� Dolly Tracks ����: {regionDollyPaths.Count}");
    }

    /// <summary>
    /// Ư�� �������� �̵�
    /// </summary>
    public void MoveToRegion(string region)
    {
        currentRegion = region;
        isFollowingUnit = false; // ���� �̵� �� Ʈ�� ���� �̵��ϵ��� ����

        if (regionDollyPaths.TryGetValue(region, out CinemachinePath newPath) && newPath != null)
        {
            SetDollyTrack(newPath);
        }
        else
        {
            Debug.LogError($"[CameraManager] {region} ������ CinemachinePath�� ã�� �� �����ϴ�!");
        }
    }

    /// <summary>
    /// Dolly Track�� ���� �̵�
    /// </summary>
    private void SetDollyTrack(CinemachinePath newPath)
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

        Debug.Log($"[CameraManager] {newPath.name} Dolly Track���� �̵�");
    }

    /// <summary>
    /// ������ �����Ǹ� �ش� ������ ���󰡱�
    /// </summary>
    public void FollowUnit(Transform unit)
    {
        if (unit == null)
        {
            Debug.LogWarning("[CameraManager] ������ ������ �����ϴ�!");
            return;
        }

        isFollowingUnit = true;
        selectedUnit = unit;
        virtualCamera.Follow = selectedUnit;
        virtualCamera.LookAt = selectedUnit;

        Debug.Log($"[CameraManager] {selectedUnit.name} ������ ����");
    }

    /// <summary>
    /// ������ ���󰡱⸦ �ߴ��ϰ� �ٽ� Dolly Track�� ���� �̵�
    /// </summary>
    public void UnfollowUnit()
    {
        selectedUnit = null;
        isFollowingUnit = false;

        if (regionDollyPaths.TryGetValue(currentRegion, out CinemachinePath newPath) && newPath != null)
        {
            SetDollyTrack(newPath);
            Debug.Log($"[CameraManager] {currentRegion} ���� Dolly Track���� ����.");
        }
        else
        {
            Debug.LogWarning($"[CameraManager] {currentRegion} ������ Dolly Track�� �����ϴ�. �⺻ ��ġ ����.");
        }
    }

    /// <summary>
    /// ���콺 �巡�׷� ī�޶� ȸ��
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
