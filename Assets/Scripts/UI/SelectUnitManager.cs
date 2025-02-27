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
            Debug.LogWarning("[SelectUnitManager] ������ ������ �����ϴ�.");
            return;
        }

        selectedUnit = unit;
        cameraManager.FollowUnit(selectedUnit);
        Debug.Log($"[SelectUnitManager] {unit.name} ������ �����Ͽ� �����մϴ�.");
    }

    public void DeselectUnit()
    {
        selectedUnit = null;
        cameraManager.UnfollowUnit(); // ���� ���� �� Dolly Track ����
        Debug.Log("[SelectUnitManager] ���� ���� ������.");
    }
}
