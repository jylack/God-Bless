using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� or ���� �Ǻ��� Ÿ��
/// </summary>
public enum UnitType { Monster, Hunter }

public class UnitCtrl : MonoBehaviour
{

    public IUnitState currentState;  // ���� ����
    public NavMeshAgent Agent { get; private set; }

    [Header("���� �⺻ �Ӽ�")]
    public UnitType type;
    public int level;
    public float maxHp;
    public float hp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public bool isGate;
    
    public float chaseSpeed = 5f;
    public float chaseRange = 10f; //�߰� ����
    public float patrolSpeed = 2f;



    public List<SkillType> skills;

    int currentPatrolPosIndex;//���� ������ǥ �ε���
    //vec3���� �ҷ��µ�... ��ӵ巡�װ� �ȵǼ� �ӽ÷� GameObj�� ����.
    //���� ���� ���������� start�� awake���� ������ ��ǥ���� ���� �������ҵ�.
    public List<Vector3> patrolPoints = new List<Vector3>();//���� ��ǥ

    private Transform target;
    public Transform Target { get => target; }


    private void Awake()
    {
        currentPatrolPosIndex = 0;
        Agent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        var obj = GameObject.Find("WayPoint_C");
        var WayPos = obj.GetComponentsInChildren<Transform>();

        foreach (var pos in WayPos)
        {
            //Debug.Log(pos.position);
            if (obj.name != pos.name)
            {
                patrolPoints.Add(pos.position);
            }
        }

        //Agent.autoRepath = true;
        //target = GameObject.FindWithTag("Player")?.transform;//�׽�Ʈ�� ���� Hunter�� �ٲܿ���

        ChangeState(new CityAitState()); // �ʱ� ���� = ����
        //Debug.Log(patrolPoints.Count);
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
    public void ChangeState(IUnitState newState)
    {
        currentState = newState;
        currentState.PlayState(this);
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���� �ִ��� üũ
    /// </summary>
    public bool IsPlayerInChaseRange()
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) < chaseRange;
    }

    /// <summary>
    /// ���� ��ġ�� �̵��ϴ� �޼��� (����)
    /// </summary>
    public void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Count <= 0)
        {
            Debug.LogError("���� ��Ұ� �������� �ʾҽ��ϴ�.");
            return;
        }

        if (currentPatrolPosIndex >= patrolPoints.Count)
        {
            currentPatrolPosIndex = 0;
        }

        Agent.SetDestination(patrolPoints[currentPatrolPosIndex]);

        currentPatrolPosIndex++;
    }

    /// <summary>
    /// �÷��̾ �����ϴ� �Լ�
    /// </summary>
    public void AttackPlayer()
    {
        Debug.Log("���Ͱ� �÷��̾ ����!");
    }

    /// <summary>
    /// ���� ���� �ʱ�ȭ (����)
    /// </summary>
    public void ResetStats()
    {

    }
}
