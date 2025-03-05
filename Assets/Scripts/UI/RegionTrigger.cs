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
    /// UI ��ư�� ���� ���� �̵� (UI���� ȣ��)
    /// </summary>
    public void MoveToRegion()
    {
        if (cameraManager == null)
        {
            Debug.LogError("[RegionTrigger] CameraManager�� ã�� �� �����ϴ�!");
            return;
        }

        cameraManager.ChangeRegion(regionName);
        Debug.Log($"[RegionTrigger] {regionName} �������� ī�޶� �̵� ����!");
    }
}
