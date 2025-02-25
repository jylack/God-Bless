using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitType { Monster, Hunter }

public class UnitCtrl : MonoBehaviour
{
    // �⺻ ������
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
    //���� ����
    private float attackRange;

    // �׺���̼� ����
    public Transform Target { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public List<Vector3> patrolPoints = new List<Vector3>();

    int currentPatrolPosIndex;
    public bool leader;
    private float chaseSpeed;
    private float chaseRange;
    private float patrolSpeed;

    //���� ���� ���� ���� (����/����� ������)
    private int originalAtk;
    private int originalMagicAtk;
    private int originalDef;
    private int originalMagicDef;

    public List<SkillData> Skills { get => skills; }
    public UnitType Type { get => type; }
    public string UnitName { get => unitName; }
    public int Level { get => level; }
    public float MaxHp { get => maxHp; }
    public float Hp { get => hp; }
    public int Atk { get => atk; }
    public int MagicAtk { get => magicAtk; }
    public int Def { get => def; }
    public int MagicDef { get => magicDef; }
    public bool IsGate { get => isGate; }
    public float ChaseSpeed { get => chaseSpeed; }
    public float ChaseRange { get => chaseRange; }
    public float PatrolSpeed { get => patrolSpeed; }

    // �⺻ ������ ����
    void UnitSetting(UnitData data)
    {
        if (data == null)
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

        // ���� ���� ����
        attackRange = data.attackRange;

        chaseSpeed = data.chaseSpeed;
        chaseRange = data.chaseRange;
        patrolSpeed = data.patrolSpeed;

        // ���� ���� ���� (���߿� ����/����� ���� �� ���)
        originalAtk = atk;
        originalMagicAtk = magicAtk;
        originalDef = def;
        originalMagicDef = magicDef;
    }

    private void Awake()
    {
        currentPatrolPosIndex = 0;
        Agent = GetComponent<NavMeshAgent>();
        UnitSetting(data);
    }

    private void Start()
    {
        var obj = GameObject.Find("WayPoint_C");
        var WayPos = obj.GetComponentsInChildren<Transform>();
        foreach (var pos in WayPos)
        {
            if (obj.name != pos.name)
            {
                patrolPoints.Add(pos.position);
            }
        }
        ChangeState(new CityAitState()); // �ʱ� ����: ����
    }

    private void Update()
    {
        //if (currentState != null)
        //{
        //    currentState.UpdateState(this);
        //}
        //���� ����
        currentState?.UpdateState(this);

    }

    public void ChangeState(IUnitState newState)
    {
        currentState = newState;
        currentState.PlayState(this);
    }


    public bool IsPlayerInChaseRange()
    {
        if (Target == null) return false;
        return Vector3.Distance(transform.position, Target.position) < chaseRange;
    }

    public bool IsTargetInAttackRange()
    {
        if (Target == null) return false;
        return Vector3.Distance(transform.position, Target.position) < attackRange;
    }

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

    public void AttackPlayer()
    {
        Debug.Log("���Ͱ� �÷��̾ ����!");
    }


    // �ܺο��� Ÿ���� �����ϰ� �߰� ���·� ��ȯ�ϴ� �޼���
    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
        ChangeState(new ChaseState());
    }

    // ���� ����: ���� �ð� ���� ���� ����
    public void ApplyBuff(float multiplier, float duration)
    {
        atk = (int)(originalAtk * multiplier);
        magicAtk = (int)(originalMagicAtk * multiplier);
        def = (int)(originalDef * multiplier);
        magicDef = (int)(originalMagicDef * multiplier);
        Debug.Log($"{gameObject.name} ���� ����: ��� {multiplier}x, ���� �ð� {duration}��");
        StartCoroutine(RevertStatsAfter(duration));
    }

    // ����� ����: ���� �ð� ���� ���� ����
    public void ApplyDebuff(float multiplier, float duration)
    {
        atk = (int)(originalAtk * multiplier);
        magicAtk = (int)(originalMagicAtk * multiplier);
        def = (int)(originalDef * multiplier);
        magicDef = (int)(originalMagicDef * multiplier);
        Debug.Log($"{gameObject.name} ����� ����: ��� {multiplier}x, ���� �ð� {duration}��");
        StartCoroutine(RevertStatsAfter(duration));
    }

    IEnumerator RevertStatsAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        atk = originalAtk;
        magicAtk = originalMagicAtk;
        def = originalDef;
        magicDef = originalMagicDef;
        Debug.Log($"{gameObject.name} ���� ����");
    }
}
