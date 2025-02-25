using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitType { Monster, Hunter }

public class UnitCtrl : MonoBehaviour
{
    // 기본 데이터
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
    private IUnitState currentState;  // 현재 상태
    //공격 범위
    private float attackRange;

    // 네비게이션 관련
    public Transform Target { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public List<Vector3> patrolPoints = new List<Vector3>();

    int currentPatrolPosIndex;
    public bool leader;
    private float chaseSpeed;
    private float chaseRange;
    private float patrolSpeed;

    //원래 스탯 저장 변수 (버프/디버프 복원용)
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

    // 기본 데이터 셋팅
    void UnitSetting(UnitData data)
    {
        if (data == null)
        {
            Debug.LogError("데이터가 없습니다.");
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

        // 공격 범위 설정
        attackRange = data.attackRange;

        chaseSpeed = data.chaseSpeed;
        chaseRange = data.chaseRange;
        patrolSpeed = data.patrolSpeed;

        // 원래 스탯 저장 (나중에 버프/디버프 해제 시 사용)
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
        ChangeState(new CityAitState()); // 초기 상태: 순찰
    }

    private void Update()
    {
        //if (currentState != null)
        //{
        //    currentState.UpdateState(this);
        //}
        //위와 같음
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
            Debug.LogError("순찰 장소가 지정되지 않았습니다.");
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
        Debug.Log("몬스터가 플레이어를 공격!");
    }


    // 외부에서 타겟을 지정하고 추격 상태로 전환하는 메서드
    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
        ChangeState(new ChaseState());
    }

    // 버프 적용: 일정 시간 동안 스탯 증가
    public void ApplyBuff(float multiplier, float duration)
    {
        atk = (int)(originalAtk * multiplier);
        magicAtk = (int)(originalMagicAtk * multiplier);
        def = (int)(originalDef * multiplier);
        magicDef = (int)(originalMagicDef * multiplier);
        Debug.Log($"{gameObject.name} 버프 적용: 배수 {multiplier}x, 지속 시간 {duration}초");
        StartCoroutine(RevertStatsAfter(duration));
    }

    // 디버프 적용: 일정 시간 동안 스탯 감소
    public void ApplyDebuff(float multiplier, float duration)
    {
        atk = (int)(originalAtk * multiplier);
        magicAtk = (int)(originalMagicAtk * multiplier);
        def = (int)(originalDef * multiplier);
        magicDef = (int)(originalMagicDef * multiplier);
        Debug.Log($"{gameObject.name} 디버프 적용: 배수 {multiplier}x, 지속 시간 {duration}초");
        StartCoroutine(RevertStatsAfter(duration));
    }

    IEnumerator RevertStatsAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        atk = originalAtk;
        magicAtk = originalMagicAtk;
        def = originalDef;
        magicDef = originalMagicDef;
        Debug.Log($"{gameObject.name} 스탯 원복");
    }
}
