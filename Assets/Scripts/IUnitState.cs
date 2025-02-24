using UnityEngine;

public interface IUnitState
{
    /// <summary>
    /// 상태 실행
    /// </summary>
    void PlayState(UnitCtrl monster);
    void UpdateState(UnitCtrl monster);
    void EndState();

}
/// <summary>
/// 순찰
/// </summary>
public class CityAitState : IUnitState
{
    public void PlayState(UnitCtrl unit)
    {
        unit.Agent.speed = unit.PatrolSpeed;
        unit.MoveToNextPatrolPoint();
    }

    public void UpdateState(UnitCtrl unit)
    {
        if (unit.Agent.pathPending == false && unit.Agent.remainingDistance < 0.5f)
        {
            unit.MoveToNextPatrolPoint();
        }

        if (unit.IsPlayerInChaseRange())
        {
            unit.ChangeState(new ChaseState());
        }
    }

    public void EndState() { }
}

/// <summary>
/// 추적
/// </summary>
public class ChaseState : IUnitState
{
    public void PlayState(UnitCtrl unit)
    {
        unit.Agent.speed = unit.ChaseSpeed;
    }

    public void UpdateState(UnitCtrl unit)
    {
        if (unit.IsPlayerInChaseRange())
        {
            unit.Agent.SetDestination(unit.Target.position);
        }
        else
        {
            unit.ChangeState(new CityAitState());
        }

        if (unit.IsTargetInAttackRange())
        {
            unit.ChangeState(new AttackState());
        }
    }

    public void EndState() { }
}

/// <summary>
/// 공격
/// </summary>
public class AttackState : IUnitState
{
    private float attackCooldown = 2f;
    private float lastAttackTime;

    public void PlayState(UnitCtrl unit)
    {
        unit.AttackPlayer();
        lastAttackTime = Time.time;
    }

    public void UpdateState(UnitCtrl unit)
    {
        if (unit.IsPlayerInChaseRange()== false)
        {
            unit.ChangeState(new CityAitState());
            return;
        }

        if (Time.time - lastAttackTime > attackCooldown)
        {
            unit.AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    public void EndState() { }
}

