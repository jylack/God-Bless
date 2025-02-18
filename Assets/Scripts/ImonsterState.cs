public interface IMonsterState
{
    void PlayState(MonsterCtrl monster);
    void UpdateState(MonsterCtrl monster);
}
public class CityAitState : IMonsterState
{
    public void PlayState(MonsterCtrl monster)
    {
        monster.Agent.speed = monster.patrolSpeed;
        monster.MoveToNextPatrolPoint();
    }

    public void UpdateState(MonsterCtrl monster)
    {
        if (!monster.Agent.pathPending && monster.Agent.remainingDistance < 0.5f)
        {
            monster.MoveToNextPatrolPoint();
        }

        if (monster.IsPlayerInChaseRange())
        {
            monster.ChangeState(new ChaseState());
        }
    }
}
public class ChaseState : IMonsterState
{
    public void PlayState(MonsterCtrl monster)
    {
        monster.Agent.speed = monster.chaseSpeed;
    }

    public void UpdateState(MonsterCtrl monster)
    {
        if (monster.IsPlayerInChaseRange())
        {
            monster.Agent.SetDestination(monster.Player.position);
        }
        else
        {
            monster.ChangeState(new CityAitState());
        }
    }
}
public class AttackState : IMonsterState
{
    public void PlayState(MonsterCtrl monster)
    {
        monster.AttackPlayer();
    }

    public void UpdateState(MonsterCtrl monster)
    {
        if (!monster.IsPlayerInChaseRange())
        {
            monster.ChangeState(new CityAitState());
        }
    }
}
