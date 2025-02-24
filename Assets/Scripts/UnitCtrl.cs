using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� or ���� �Ǻ��� Ÿ��
/// </summary>
public enum UnitType { Monster, Hunter }

public class UnitCtrl : MonoBehaviour
{

    //�� �ְ������ ���������� ���ݴ� ����غ���.
    //[Header("���� �⺻ �Ӽ�")]
    public UnitData data;
    private UnitType type;
    private string unitName;
    private int level;
    private float maxHp;
    private float hp;
    private int atk;
    private int magicAtk;
    private int def;
    private int magicDef;
    private bool isGate;
    private List<SkillData> skills;
    private IUnitState currentState;  // ���� ����


    //[Header("�׺�޽� ����")]
    private float chaseSpeed ;
    private float chaseRange ;
    private float patrolSpeed;
    
    public bool leader;
    public NavMeshAgent Agent { get; private set; }
    int currentPatrolPosIndex;//���� ������ǥ �ε���
    //vec3���� �ҷ��µ�... ��ӵ巡�װ� �ȵǼ� �ӽ÷� GameObj�� ����.
    //���� ���� ���������� start�� awake���� ������ ��ǥ���� ���� �������ҵ�.
    public List<Vector3> patrolPoints = new List<Vector3>();//���� ��ǥ

    private Transform target;

    //������Ƽ
    public List<SkillData> Skills { get => skills; }

    public Transform Target { get => target; }

    public UnitType Type { get => type;  }

    public string UnitName { get => unitName; }

    public int Level { get => level; }

    public float MaxHp { get => maxHp; }
    public float Hp { get => hp; }
    public int Atk { get => atk; }
    public int MagicAtk { get => magicAtk; }
    public int Def { get => def; }
    public int MagicDef { get => magicDef; }

    public bool IsGate { get => isGate; }

    public float ChaseSpeed { get => chaseSpeed;  }
    public float ChaseRange { get => chaseRange; }
    public float PatrolSpeed { get => patrolSpeed; }

    void UnitSetting(UnitData data)
    {
        if(data == null)
        {
            Debug.LogError("�����Ͱ� �����ϴ�.");
            return;
        }

        type = data.unitType;
        
        unitName = data.unitName;
        
        level = data.level;

        maxHp = data.maxHp;
        hp = maxHp;
        atk = data.atk;
        magicAtk = data.magicAtk;
        def = data.def;
        magicDef = data.magicDef;
        isGate = true;
        
        skills = data.skills;

        chaseSpeed = data.chaseSpeed;
        chaseRange = data.chaseRange;
        patrolSpeed = data.patrolSpeed;
    }

    private void Awake()
    {
        currentPatrolPosIndex = 0;
        Agent = GetComponent<NavMeshAgent>();
        
        UnitSetting(data);
    }

    public void SetLeader(bool select)
    {
        leader = select;
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
