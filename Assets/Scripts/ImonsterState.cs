public interface IMonsterState
{
    void PlayState(MonsterCtrl monster);
    void UpdateState(MonsterCtrl monster);
}

////���ó����� �̵�
//public class CityMoveState : IMonsterState
//{
//    public void PlayState(MonsterCtrl monster)
//    {
//        // ���� �̵� ���� ����
//        // �ٸ� ���·� ����
//        monster.ChangeState(new AttackState());
//    }

//    public void UpdateState(MonsterCtrl monster)
//    {
//        throw new System.NotImplementedException();
//    }
//}


//����
public class AttackState : IMonsterState
{
    public void PlayState(MonsterCtrl monster)
    {
        // ���� ���� ����
        // �ٸ� ���·� ����
        monster.ChangeState(new CityAitState());
    }

    public void UpdateState(MonsterCtrl monster)
    {
        throw new System.NotImplementedException();
    }
}

//���� ��ȸ
public class CityAitState : IMonsterState
{

    public void PlayState(MonsterCtrl monster)
    {
        monster.SetSpeed(monster.patrolSpeed);
        monster.MoveToNextPatrolPoint();
    }

    public void UpdateState(MonsterCtrl monster)
    {
        if (monster.Agent.pathPending  == false
            && monster.Agent.remainingDistance < 0.5f)
        {
            monster.MoveToNextPatrolPoint();
        }
        if (monster.IsPlayerInChaseRange())
        {
            monster.ChangeState(new ChaseState());
        }
    }
}

//���� ����
public class ChaseState : IMonsterState
{
    public void PlayState(MonsterCtrl monster)
    {
        monster.SetSpeed(monster.chaseSpeed);
    }

    public void UpdateState(MonsterCtrl monster)
    {
        monster.ChangeState(new CityAitState());

        //if (monster.IsPlayerInChaseRange())
        //{
        //    //monster.SetDestination(monster.player.position);
        //}
        //else
        //{
        //}
    }
}