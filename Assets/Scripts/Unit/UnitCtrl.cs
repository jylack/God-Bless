using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class UnitCtrl : MonoBehaviour
{
    public UnitData data; // 유닛 데이터
    public UnitType unitType; // 유닛 타입
    public Region assignedRegion; // 할당된 지역
    private float hp;
    public List<SkillData> skills;
    private float lastSkillUseTime = 0f;

    public NavMeshAgent agent;
    private int patrolIndex = 0;
    private UnitEquipment equipment;

    private static int unitCount = 0; // 유닛 수 카운트
    private static int nevmashCount = 0; // nevmash 아이템 카운트

    public float detectionRange = 10f; // 탐색 범위
    public float attackRange = 2f; // 공격 범위
    public float attackCooldown = 1.5f; // 공격 간격
    private float lastAttackTime = 0f;
    public float escapeDistance = 20f; // 시민이 도망가는 거리

    private static Dictionary<int, List<UnitCtrl>> unitGroups = new Dictionary<int, List<UnitCtrl>>(); // 그룹 관리
    private static int groupCounter = 0;
    private int groupID;

    private List<Transform> patrolWayPoints;
    private bool isReversed = false;
    private Transform target;

    IUnitState currentState;

    private void Start()
    {
        unitCount++;

        if (data == null) return;

        hp = data.maxHp;
        skills = new List<SkillData>(data.skills);

        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}에 NavMeshAgent가 존재하지 않습니다! GOD에서 추가되었는지 확인하세요.");
            return;
        }
        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}이(가) NavMesh에 배치되지 않았습니다! 위치: {transform.position}");
            return;
        }

        //agent = gameObject.AddComponent<NavMeshAgent>();

        //agent.speed = 3.5f;
        //agent.acceleration = 8f;
        //agent.angularSpeed = 120f;

        equipment = GetComponent<UnitEquipment>() ?? gameObject.AddComponent<UnitEquipment>();

        AssignNevmashItem();
        EquipRandomItems();
        LoadWayPoints();

        if (data.unitType == UnitType.Hunter || data.unitType == UnitType.Monster)
        {
            AssignToGroup();
            StartCoroutine(AutoCombatSystem());
        }
        else if (data.unitType == UnitType.Citizen)
        {
            StartCoroutine(CitizenEscapeSystem());
        }

        // 초기 상태 설정
        if (unitType == UnitType.Hunter || unitType == UnitType.Monster)
        {
            ChangeState(new PatrolState());
        }
        else if (unitType == UnitType.Citizen)
        {
            ChangeState(new CitizenPatrolState());
        }
    }

    private void OnDestroy()
    {
        unitCount--;
    }

    private void Update()
    {
        if (data.unitType == UnitType.Hunter || data.unitType == UnitType.Monster)
        {
            if (unitGroups[groupID][0] == this)
            {
                FindNearestEnemy();
            }
        }
        else if (data.unitType == UnitType.Citizen)
        {
            Patrol();
        }

        if (currentState != null)
        {
            currentState.UpdateState(this); // 상태 업데이트 실행
        }
        else
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}의 currentState가 NULL 입니다!");
        }
    }

    private void AssignToGroup()
    {
        if (unitGroups.ContainsKey(groupCounter) && unitGroups[groupCounter].Count < 3)
        {
            unitGroups[groupCounter].Add(this);
        }
        else
        {
            groupCounter++;
            unitGroups[groupCounter] = new List<UnitCtrl> { this };
        }
        groupID = groupCounter;
    }

    private IEnumerator AutoCombatSystem()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (unitGroups[groupID][0] == this)
            {
                FindNearestEnemy();
            }
        }
    }

    private void FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if ((data.unitType == UnitType.Hunter && col.CompareTag("Monster")) ||
                (data.unitType == UnitType.Monster && col.CompareTag("Hunter")))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = col.transform;
                }
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy;
            if (minDistance <= attackRange)
            {
                Attack();
            }
            else
            {
                MoveToTarget();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            AdjustFormation();
        }
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log($"{gameObject.name}이(가) {target.name}을 공격!");
            lastAttackTime = Time.time;
        }
    }

    private void Patrol()
    {
        if (agent == null)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}에 NavMeshAgent가 없습니다!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}이(가) NavMesh에 배치되지 않았습니다! 이동 불가능.");
            return;
        }

        if (patrolWayPoints == null || patrolWayPoints.Count == 0) return;

        if (agent.remainingDistance < 1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolWayPoints.Count;
        }

        agent.SetDestination(patrolWayPoints[patrolIndex].position);
    }


    private IEnumerator CitizenEscapeSystem()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            DetectMonsterAndEscape();
        }
    }

    private void DetectMonsterAndEscape()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestMonster = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Monster"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestMonster = col.transform;
                }
            }
        }

        if (nearestMonster != null)
        {
            EscapeFrom(nearestMonster);
        }
    }

    private void EscapeFrom(Transform monster)
    {
        Vector3 direction = (transform.position - monster.position).normalized;
        Vector3 escapePoint = transform.position + direction * escapeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(escapePoint, out hit, escapeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log($"{gameObject.name}이(가) {monster.name}을 피해 도망갑니다!");
        }
    }

    private void AssignNevmashItem()
    {
        if (unitCount % 3 == 0)
        {
            nevmashCount++;
            Debug.Log($"Nevmash 아이템 할당: 총 {nevmashCount}개");
        }
    }

    private void EquipRandomItems()
    {
        if (equipment == null) return;

        ItemData[] allItems = DataBase.Instance.GetAllItems().ToArray();
        if (allItems.Length == 0) return;

        List<ItemData> weapons = new List<ItemData>();
        List<ItemData> armors = new List<ItemData>();

        foreach (var item in allItems)
        {
            if (item.itemType == ItemType.Weapon) weapons.Add(item);
            else if (item.itemType == ItemType.Armor) armors.Add(item);
        }

        if (weapons.Count > 0) equipment.EquipItem(weapons[Random.Range(0, weapons.Count)]);
        if (armors.Count > 0) equipment.EquipItem(armors[Random.Range(0, armors.Count)]);
    }

    public void ChangeState(IUnitState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }
    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Attack(Transform target)
    {
        Debug.Log($"{gameObject.name}이(가) {target.name}을 공격!");
    }

    public Transform GetNextPatrolPoint()
    {
        // WayPoint 순찰 로직 추가 필요
        return null;
    }

    /// <summary>
    /// 같은 그룹 내 유닛들이 서로 겹치지 않도록 조정
    /// </summary>
    private void AdjustFormation()
    {
        if (!unitGroups.ContainsKey(groupID)) return;

        int index = unitGroups[groupID].IndexOf(this);
        if (index == -1) return;

        // 유닛들이 좌우로 퍼지도록 배치
        Vector3 offset = new Vector3((index - 1) * 1.5f, 0, (index % 2) * 1.5f);
        agent.SetDestination(agent.destination + offset);
    }

    /// <summary>
    /// 지역별 WayPoint 불러오기 (순찰 경로 설정)
    /// </summary>
    private void LoadWayPoints()
    {
        GameObject[] possibleWayPoints = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject regionObject = null;

        foreach (var obj in possibleWayPoints)
        {
            if (obj.name.StartsWith("WayPoint"))
            {
                regionObject = obj;
                break;
            }
        }

        if (regionObject == null)
        {
            Debug.LogError($"[UnitCtrl] 'WayPoint' 오브젝트를 찾을 수 없습니다. 현재 씬에 존재하는 오브젝트 목록:");
            GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in allObjects)
            {
                Debug.Log($" - {obj.name}");
            }
            return;
        }

        Transform[] wayPoints = regionObject.GetComponentsInChildren<Transform>();
        patrolWayPoints = new List<Transform>();

        foreach (Transform point in wayPoints)
        {
            if (point.name.StartsWith("WayPoint_"))
            {
                patrolWayPoints.Add(point);
            }
        }

        if (patrolWayPoints.Count == 0)
        {
            Debug.LogError($"[UnitCtrl] {regionObject.name} 지역에 사용 가능한 WayPoint가 없습니다!");
        }
    }

    private IEnumerator ApplySkillOverTime(SkillData skill)
    {
        Debug.Log($"{data.unitName}의 {skill.skillName} 효과 적용 시작 (지속시간: {skill.effectDuration}초)");
        yield return new WaitForSeconds(skill.effectDuration);
        Debug.Log($"{data.unitName}의 {skill.skillName} 효과 종료.");
    }

    /// <summary>
    /// GOD으로부터 스킬을 부여받음
    /// </summary>
    public void ReceiveSkill(SkillData skill)
    {
        if (data == null)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}의 UnitData가 초기화되지 않았습니다!");
            return;
        }

        if (skills == null)
        {
            skills = new List<SkillData>();
        }

        if (skills.Contains(skill))
        {
            Debug.Log($"{data.unitName}은(는) 이미 {skill.skillName} 스킬을 보유하고 있습니다!");
            return;
        }

        skills.Add(skill);
        Debug.Log($"{data.unitName}이(가) {skill.skillName} 스킬을 부여받음.");
    }

    /// <summary>
    /// 보유한 스킬 사용 (랜덤 사용)
    /// </summary>
    public void UseRandomSkill()
    {
        if (skills.Count == 0)
        {
            Debug.Log($"{data.unitName}은(는) 사용할 스킬이 없습니다.");
            return;
        }

        SkillData skillToUse = skills[Random.Range(0, skills.Count)];
        if (Time.time - lastSkillUseTime < skillToUse.cooldown)
        {
            Debug.Log($"{data.unitName}의 {skillToUse.skillName} 스킬은 아직 쿨타임 중.");
            return;
        }

        ApplySkillEffect(skillToUse);
        lastSkillUseTime = Time.time;
    }

    /// <summary>
    /// 스킬 효과 적용
    /// </summary>
    private void ApplySkillEffect(SkillData skill)
    {
        if (skill.isAreaEffect)
        {
            Debug.Log($"{data.unitName}이(가) {skill.skillName} 스킬 사용! (범위 공격)");
        }
        else
        {
            Debug.Log($"{data.unitName}이(가) {skill.skillName} 스킬 사용! (단일 대상)");
        }

        if (skill.effectDuration > 0)
        {
            StartCoroutine(ApplySkillOverTime(skill));
        }
    }

}
