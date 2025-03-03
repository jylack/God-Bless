using UnityEngine;

public class PatrolState : IUnitState
{
    public void EnterState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) 순찰을 시작합니다.");
    }

    public void UpdateState(UnitCtrl unit)
    {
        Transform nextPoint = unit.GetNextPatrolPoint();
        if (nextPoint != null)
        {
            unit.MoveTo(nextPoint.position);
        }

        // 적을 발견하면 전투 상태로 전환
        Collider[] colliders = Physics.OverlapSphere(unit.transform.position, unit.detectionRange);
        foreach (Collider col in colliders)
        {
            if ((unit.unitType == UnitType.Hunter && col.CompareTag("Monster")) ||
                (unit.unitType == UnitType.Monster && col.CompareTag("Hunter")))
            {
                unit.ChangeState(new CombatState(col.transform));
                return;
            }
        }
    }

    public void ExitState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) 순찰을 종료합니다.");
    }
}
