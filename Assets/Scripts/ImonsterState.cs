public interface IUnitState
{
    /// <summary>
    /// ���� ����
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
        if (monster.Agent.pathPending == false && //NavMeshAgent.pathPending ��� ����� ���� �� Ȯ�ο�
            monster.Agent.remainingDistance < 0.5f) // Agent.remainingDistance ��ǥ���� �����Ÿ�
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
