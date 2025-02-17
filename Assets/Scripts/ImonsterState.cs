public interface IMonsterState
{
    void PlayState(MonsterCtrl monster);
    void UpdateState(MonsterCtrl monster);
}

////도시내에서 이동
//public class CityMoveState : IMonsterState
//{
//    public void PlayState(MonsterCtrl monster)
//    {
//        // 도시 이동 로직 구현
//        // 다른 상태로 변경
//        monster.ChangeState(new AttackState());
//    }

//    public void UpdateState(MonsterCtrl monster)
//    {
//        throw new System.NotImplementedException();
//    }
//}


//공격
public class AttackState : IMonsterState
{
    public void PlayState(MonsterCtrl monster)
    {
        // 공격 로직 구현
        // 다른 상태로 변경
        monster.ChangeState(new CityAitState());
    }

    public void UpdateState(MonsterCtrl monster)
    {
        throw new System.NotImplementedException();
    }
}

//도시 순회
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

//추적 상태
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