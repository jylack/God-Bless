using UnityEngine;

public class SelectUnitManager : MonoBehaviour
{
    private CameraManager cameraManager;
    private Transform selectedUnit;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    public void SelectUnit(Transform unit)
    {
        if (unit == null)
        {
            Debug.LogWarning("[SelectUnitManager] 선택할 유닛이 없습니다.");
            return;
        }

        selectedUnit = unit;
        cameraManager.FollowUnit(selectedUnit);
        Debug.Log($"[SelectUnitManager] {unit.name} 유닛을 선택하여 추적합니다.");
    }

    public void DeselectUnit()
    {
        selectedUnit = null;
        cameraManager.UnfollowUnit(); // 유닛 해제 후 Dolly Track 복귀
        Debug.Log("[SelectUnitManager] 유닛 선택 해제됨.");
    }
}
