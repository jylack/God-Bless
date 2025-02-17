using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    //속성
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int Atk { get; set; }
    public int Matk { get; set; }
    public int Def { get; set; }
    public List<string> Skills { get; set; }
    public bool InGate { get; set; }

    public List<Transform> patrolPoints;
    public Transform player;
    public float chaseRange = 10f;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
    private NavMeshAgent agent;
    private IMonsterState currentState;
    private int currentPatrolIndex;
    public NavMeshAgent Agent { get => agent; }


    //현재상태
    public IMonsterState CurrentState { get; private set; }


    // 생성자
    public MonsterCtrl(int maxHp, int atk, int matk, int def, List<string> skills)
    {
        MaxHp = maxHp;
        Hp = maxHp;
        Atk = atk;
        Matk = matk;
        Def = def;
        Skills = skills;
        InGate = false;
        CurrentState = new CityAitState(); //기본상태
    }
    // 상태를 변경하는 메서드
    public void ChangeState(IMonsterState newState)
    {
        CurrentState = newState;
        currentState.PlayState(this);
    }
    // 현재 상태를 처리하는 메서드
    public void HandleState()
    {
        CurrentState.PlayState(this);

    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = new CityAitState();
        currentState.PlayState(this);
    }
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
    public bool IsPlayerInChaseRange() 
    {
        return Vector3.Distance(transform.position, player.position) <= chaseRange; 
    }
    public void MoveToNextPatrolPoint() 
    {
        if (patrolPoints.Count == 0) return;
        
        agent.destination = patrolPoints[currentPatrolIndex].position; 
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }

    public void SetSpeed(float speed) 
    {
        agent.speed = speed;
    }

}
