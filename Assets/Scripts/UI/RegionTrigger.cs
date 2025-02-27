using UnityEngine;

public class RegionTrigger : MonoBehaviour
{
    public string regionName; // 지역 이름
    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    /// <summary>
    /// UI 버튼을 통해 지역 이동 (UI에서 이 메서드를 호출)
    /// </summary>
    public void MoveToRegion()
    {
        if (cameraManager != null)
        {
            cameraManager.MoveToRegion(regionName);
            Debug.Log($"[RegionTrigger] {regionName} 지역으로 이동");
        }
        else
        {
            Debug.LogError("[RegionTrigger] CameraManager를 찾을 수 없습니다!");
        }
    }
}
