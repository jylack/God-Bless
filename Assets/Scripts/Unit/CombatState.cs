using UnityEngine;

public class CombatState : IUnitState
{
    private Transform target;

    public CombatState(Transform enemy)
    {
        target = enemy;
    }

    public void EnterState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) {target.name}과 전투 시작!");
    }

    public void UpdateState(UnitCtrl unit)
    {
        if (target == null)
        {
            unit.ChangeState(new PatrolState());
            return;
        }

        float distance = Vector3.Distance(unit.transform.position, target.position);
        if (distance <= unit.attackRange)
        {
            unit.Attack(target);
        }
        else
        {
            unit.MoveTo(target.position);
        }
    }

    public void ExitState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) 전투를 종료합니다.");
    }
}
