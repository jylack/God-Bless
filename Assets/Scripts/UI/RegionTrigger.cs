using UnityEngine;

public class RegionTrigger : MonoBehaviour
{
    public string regionName; // ���� �̸�
    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    /// <summary>
    /// UI ��ư�� ���� ���� �̵� (UI���� �� �޼��带 ȣ��)
    /// </summary>
    public void MoveToRegion()
    {
        if (cameraManager != null)
        {
            cameraManager.MoveToRegion(regionName);
            Debug.Log($"[RegionTrigger] {regionName} �������� �̵�");
        }
        else
        {
            Debug.LogError("[RegionTrigger] CameraManager�� ã�� �� �����ϴ�!");
        }
    }
}
