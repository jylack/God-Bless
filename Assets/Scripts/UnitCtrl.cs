using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 몬스터 or 헌터 판별용 타입
/// </summary>
public enum UnitType { Monster, Hunter }

public class UnitCtrl : MonoBehaviour
{

    //값 주고받을떄 어찌할지는 조금더 고민해보자.
    //[Header("유닛 기본 속성")]
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


    //[Header("네비메쉬 설정")]
    private float chaseSpeed ;
    private float chaseRange ;
    private float patrolSpeed;
    
    public bool leader;
    public NavMeshAgent Agent { get; private set; }
    int currentPatrolPosIndex;//현재 순찰좌표 인덱스
    //vec3으로 할랬는데... 드롭드래그가 안되서 임시로 GameObj로 만듬.
    //추후 지역 배정받을때 start나 awake에서 지역별 좌표지정 따로 만들어야할듯.
    public List<Vector3> patrolPoints = new List<Vector3>();//순찰 좌표

    private Transform target;

    //프로퍼티
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
        //target = GameObject.FindWithTag("Player")?.transform;//테스트용 추후 Hunter로 바꿀예정

        ChangeState(new CityAitState()); // 초기 상태 = 순찰
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
    /// 몬스터의 상태를 변경하는 메서드
    /// </summary>
    public void ChangeState(IUnitState newState)
    {
        currentState = newState;
        currentState.PlayState(this);
    }

    /// <summary>
    /// 플레이어가 추적 범위 내에 있는지 체크
    /// </summary>
    public bool IsPlayerInChaseRange()
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) < chaseRange;
    }

    /// <summary>
    /// 다음 위치로 이동하는 메서드 (순찰)
    /// </summary>
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

    /// <summary>
    /// 플레이어를 공격하는 함수
    /// </summary>
    public void AttackPlayer()
    {
        Debug.Log("몬스터가 플레이어를 공격!");
    }

    /// <summary>
    /// 몬스터 스탯 초기화 (예시)
    /// </summary>
    public void ResetStats()
    {

    }
}
