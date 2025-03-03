using UnityEngine;

public class CitizenPatrolState : IUnitState
{
    public void EnterState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) 순찰을 시작합니다.");
    }

    public void UpdateState(UnitCtrl unit)
    {
        Collider[] colliders = Physics.OverlapSphere(unit.transform.position, unit.detectionRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Monster"))
            {
                unit.ChangeState(new CitizenEscapeState(col.transform));
                return;
            }
        }

        Transform nextPoint = unit.GetNextPatrolPoint();
        if (nextPoint != null)
        {
            unit.MoveTo(nextPoint.position);
        }
    }

    public void ExitState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) 순찰을 종료합니다.");
    }
}

public class CitizenEscapeState : IUnitState
{
    private Transform threat; // 도망가야 할 몬스터

    public CitizenEscapeState(Transform monster)
    {
        threat = monster;
    }

    public void EnterState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) {threat.name}을 피해 도망가기 시작합니다.");
    }

    public void UpdateState(UnitCtrl unit)
    {
        if (threat == null)
        {
            unit.ChangeState(new CitizenPatrolState());
            return;
        }

        float distance = Vector3.Distance(unit.transform.position, threat.position);
        if (distance >= unit.escapeDistance)
        {
            unit.ChangeState(new CitizenPatrolState());
            return;
        }

        EscapeFrom(unit, threat);
    }

    public void ExitState(UnitCtrl unit)
    {
        Debug.Log($"{unit.name}이(가) 도망을 종료하고 순찰로 복귀합니다.");
    }

    private void EscapeFrom(UnitCtrl unit, Transform monster)
    {
        Vector3 direction = (unit.transform.position - monster.position).normalized;
        Vector3 escapePoint = unit.transform.position + direction * unit.escapeDistance;

        unit.MoveTo(escapePoint);
    }
}