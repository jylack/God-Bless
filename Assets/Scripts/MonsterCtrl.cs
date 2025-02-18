using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public IMonsterState currentState;  // 현재 상태
    public NavMeshAgent Agent { get; private set; }

    [Header("몬스터 기본 속성")]
    public float maxHp = 100f;
    public float hp = 100f;
    
    public float chaseSpeed = 5f;
    public float chaseRange = 10f; //추격 범위
    public float patrolSpeed = 2f;

    //public float attackPower = 10f;
    public float atk = 10f;
    public float magicAtk;
    public float def;
    public float magicDef;

    public List<string> skills;
    public List<Vector3> patrolPoints;//순찰 좌표

    private Transform player;
    public Transform Player { get => player; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player")?.transform;

        ChangeState(new CityAitState()); // 초기 상태 = 순찰
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    /// <summary>
    /// 몬스터의 상태를 변경하는 메서드
    /// </summary>
    public void ChangeState(IMonsterState newState)
    {
        currentState = newState;
        currentState.PlayState(this);
    }

    /// <summary>
    /// 플레이어가 추적 범위 내에 있는지 체크
    /// </summary>
    public bool IsPlayerInChaseRange()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) < 10f;
    }

    /// <summary>
    /// 랜덤한 위치로 이동하는 메서드 (순찰)
    /// </summary>
    public void MoveToNextPatrolPoint()
    {
        Vector3 patrolPoint = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Agent.SetDestination(patrolPoint);
    }

    /// <summary>
    /// 플레이어를 공격하는 함수
    /// </summary>
    public void AttackPlayer()
    {
        Debug.Log("몬스터가 플레이어를 공격!");
    }
}
