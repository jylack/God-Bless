using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public IMonsterState currentState;  // ���� ����
    public NavMeshAgent Agent { get; private set; }

    [Header("���� �⺻ �Ӽ�")]
    public float maxHp = 100f;
    public float hp = 100f;
    
    public float chaseSpeed = 5f;
    public float chaseRange = 10f; //�߰� ����
    public float patrolSpeed = 2f;

    //public float attackPower = 10f;
    public float atk = 10f;
    public float magicAtk;
    public float def;
    public float magicDef;

    public List<string> skills;
    public List<Vector3> patrolPoints;//���� ��ǥ

    private Transform player;
    public Transform Player { get => player; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player")?.transform;

        ChangeState(new CityAitState()); // �ʱ� ���� = ����
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    /// <summary>
    /// ������ ���¸� �����ϴ� �޼���
    /// </summary>
    public void ChangeState(IMonsterState newState)
    {
        currentState = newState;
        currentState.PlayState(this);
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���� �ִ��� üũ
    /// </summary>
    public bool IsPlayerInChaseRange()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) < 10f;
    }

    /// <summary>
    /// ������ ��ġ�� �̵��ϴ� �޼��� (����)
    /// </summary>
    public void MoveToNextPatrolPoint()
    {
        Vector3 patrolPoint = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Agent.SetDestination(patrolPoint);
    }

    /// <summary>
    /// �÷��̾ �����ϴ� �Լ�
    /// </summary>
    public void AttackPlayer()
    {
        Debug.Log("���Ͱ� �÷��̾ ����!");
    }
}
