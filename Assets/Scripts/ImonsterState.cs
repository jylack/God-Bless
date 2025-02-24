public interface IUnitState
{
    /// <summary>
    /// 상태 실행
    /// </summary>
    void PlayState(UnitCtrl monster);


    void UpdateState(UnitCtrl monster);
}
public class CityAitState : IUnitState
{
    public void PlayState(UnitCtrl monster)
    {
        monster.Agent.speed = monster.PatrolSpeed;
        monster.MoveToNextPatrolPoint();
    }

    public void UpdateState(UnitCtrl monster)
    {
        if (monster.Agent.pathPending == false && //NavMeshAgent.pathPending 경로 계산이 진행 중 확인용
            monster.Agent.remainingDistance < 0.5f) // Agent.remainingDistance 목표까지 남은거리
        {
            monster.MoveToNextPatrolPoint();
        }

        if (monster.IsPlayerInChaseRange())
        {
            monster.ChangeState(new ChaseState());
        }
    }
}
public class ChaseState : IUnitState
{
    public void PlayState(UnitCtrl monster)
    {
        monster.Agent.speed = monster.ChaseSpeed;
    }

    public void UpdateState(UnitCtrl monster)
    {
        if (monster.IsPlayerInChaseRange())
        {
            monster.Agent.SetDestination(monster.Target.position);
        }
        else
        {
            monster.ChangeState(new CityAitState());
        }
    }
}
public class AttackState : IUnitState
{
    public void PlayState(UnitCtrl monster)
    {
        monster.AttackPlayer();
    }

    public void UpdateState(UnitCtrl monster)
    {
        if (!monster.IsPlayerInChaseRange())
        {
            monster.ChangeState(new CityAitState());
        }
    }
}
