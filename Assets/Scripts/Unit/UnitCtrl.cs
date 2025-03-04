using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class UnitCtrl : MonoBehaviour
{
    public UnitData data; // ���� ������
    public UnitType unitType; // ���� Ÿ��
    public Region assignedRegion; // �Ҵ�� ����
    private float hp;
    public List<SkillData> skills;
    private float lastSkillUseTime = 0f;

    public NavMeshAgent agent;
    private int patrolIndex = 0;
    private UnitEquipment equipment;

    private static int unitCount = 0; // ���� �� ī��Ʈ
    private static int nevmashCount = 0; // nevmash ������ ī��Ʈ

    public float detectionRange = 10f; // Ž�� ����
    public float attackRange = 2f; // ���� ����
    public float attackCooldown = 1.5f; // ���� ����
    private float lastAttackTime = 0f;
    public float escapeDistance = 20f; // �ù��� �������� �Ÿ�

    private static Dictionary<int, List<UnitCtrl>> unitGroups = new Dictionary<int, List<UnitCtrl>>(); // �׷� ����
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
            Debug.LogError($"[UnitCtrl] {gameObject.name}�� NavMeshAgent�� �������� �ʽ��ϴ�! GOD���� �߰��Ǿ����� Ȯ���ϼ���.");
            return;
        }
        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}��(��) NavMesh�� ��ġ���� �ʾҽ��ϴ�! ��ġ: {transform.position}");
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

        // �ʱ� ���� ����
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
            currentState.UpdateState(this); // ���� ������Ʈ ����
        }
        else
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}�� currentState�� NULL �Դϴ�!");
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
            Debug.Log($"{gameObject.name}��(��) {target.name}�� ����!");
            lastAttackTime = Time.time;
        }
    }

    private void Patrol()
    {
        if (agent == null)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}�� NavMeshAgent�� �����ϴ�!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}��(��) NavMesh�� ��ġ���� �ʾҽ��ϴ�! �̵� �Ұ���.");
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
            Debug.Log($"{gameObject.name}��(��) {monster.name}�� ���� �������ϴ�!");
        }
    }

    private void AssignNevmashItem()
    {
        if (unitCount % 3 == 0)
        {
            nevmashCount++;
            Debug.Log($"Nevmash ������ �Ҵ�: �� {nevmashCount}��");
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
        Debug.Log($"{gameObject.name}��(��) {target.name}�� ����!");
    }

    public Transform GetNextPatrolPoint()
    {
        // WayPoint ���� ���� �߰� �ʿ�
        return null;
    }

    /// <summary>
    /// ���� �׷� �� ���ֵ��� ���� ��ġ�� �ʵ��� ����
    /// </summary>
    private void AdjustFormation()
    {
        if (!unitGroups.ContainsKey(groupID)) return;

        int index = unitGroups[groupID].IndexOf(this);
        if (index == -1) return;

        // ���ֵ��� �¿�� �������� ��ġ
        Vector3 offset = new Vector3((index - 1) * 1.5f, 0, (index % 2) * 1.5f);
        agent.SetDestination(agent.destination + offset);
    }

    /// <summary>
    /// ������ WayPoint �ҷ����� (���� ��� ����)
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
            Debug.LogError($"[UnitCtrl] 'WayPoint' ������Ʈ�� ã�� �� �����ϴ�. ���� ���� �����ϴ� ������Ʈ ���:");
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
            Debug.LogError($"[UnitCtrl] {regionObject.name} ������ ��� ������ WayPoint�� �����ϴ�!");
        }
    }

    private IEnumerator ApplySkillOverTime(SkillData skill)
    {
        Debug.Log($"{data.unitName}�� {skill.skillName} ȿ�� ���� ���� (���ӽð�: {skill.effectDuration}��)");
        yield return new WaitForSeconds(skill.effectDuration);
        Debug.Log($"{data.unitName}�� {skill.skillName} ȿ�� ����.");
    }

    /// <summary>
    /// GOD���κ��� ��ų�� �ο�����
    /// </summary>
    public void ReceiveSkill(SkillData skill)
    {
        if (data == null)
        {
            Debug.LogError($"[UnitCtrl] {gameObject.name}�� UnitData�� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return;
        }

        if (skills == null)
        {
            skills = new List<SkillData>();
        }

        if (skills.Contains(skill))
        {
            Debug.Log($"{data.unitName}��(��) �̹� {skill.skillName} ��ų�� �����ϰ� �ֽ��ϴ�!");
            return;
        }

        skills.Add(skill);
        Debug.Log($"{data.unitName}��(��) {skill.skillName} ��ų�� �ο�����.");
    }

    /// <summary>
    /// ������ ��ų ��� (���� ���)
    /// </summary>
    public void UseRandomSkill()
    {
        if (skills.Count == 0)
        {
            Debug.Log($"{data.unitName}��(��) ����� ��ų�� �����ϴ�.");
            return;
        }

        SkillData skillToUse = skills[Random.Range(0, skills.Count)];
        if (Time.time - lastSkillUseTime < skillToUse.cooldown)
        {
            Debug.Log($"{data.unitName}�� {skillToUse.skillName} ��ų�� ���� ��Ÿ�� ��.");
            return;
        }

        ApplySkillEffect(skillToUse);
        lastSkillUseTime = Time.time;
    }

    /// <summary>
    /// ��ų ȿ�� ����
    /// </summary>
    private void ApplySkillEffect(SkillData skill)
    {
        if (skill.isAreaEffect)
        {
            Debug.Log($"{data.unitName}��(��) {skill.skillName} ��ų ���! (���� ����)");
        }
        else
        {
            Debug.Log($"{data.unitName}��(��) {skill.skillName} ��ų ���! (���� ���)");
        }

        if (skill.effectDuration > 0)
        {
            StartCoroutine(ApplySkillOverTime(skill));
        }
    }

}
