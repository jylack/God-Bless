using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitCtrl : MonoBehaviour
{
    public UnitData data; // ���� ������ (����, ����, �ù�)
    public Region assignedRegion; // �Ҵ�� ����
    private float hp;
    private List<SkillData> skills;
    private NavMeshAgent agent;
    private int patrolIndex = 0;

    private UnitEquipment equipment; // ��� �ý��� �߰�

    private static int unitCount = 0; // ���� ���� ���� ���� ���� ����
    private static int nevmashCount = 0; // nevmash ������ ���� ���� ���� ���� ����

    private void Start()
    {
        unitCount++; // ���� �� ����

        if (data == null) return;

        //ü�� �� ��ų �ʱ�ȭ
        hp = data.maxHp;
        skills = new List<SkillData>(data.skills);
        agent = GetComponent<NavMeshAgent>();

        // ��� �ý��� �ʱ�ȭ
        equipment = GetComponent<UnitEquipment>();
        if (equipment == null)
        {
            equipment = gameObject.AddComponent<UnitEquipment>();
        }

        // ������ ���� �Ǵ� �ù��̸� ���� ����
        if (data.unitType == UnitType.Citizen || data.unitType == UnitType.Hunter)
        {
            StartCoroutine(PatrolRoutine());
        }

        // nevmash ������ �Ҵ�
        AssignNevmashItem();

        //�ӽ��ڵ�
        // ������ ������ ����
        EquipRandomItems();
    }

    private void OnDestroy()
    {
        unitCount--; // ���� �� ����
    }

    private void Update()
    {
        if (data.unitType == UnitType.Monster)
            MonsterBehavior();
    }

    private void MonsterBehavior()
    {
        if (data == null || agent == null) return;

        GameObject hunterGO = GameObject.FindGameObjectWithTag("Hunter");
        if (hunterGO != null)
        {
            float distance = Vector3.Distance(transform.position, hunterGO.transform.position);
            if (distance < data.chaseRange)
            {
                agent.SetDestination(hunterGO.transform.position);
            }
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (assignedRegion != null && assignedRegion.patrolPoints.Count > 0)
        {
            Transform patrolPoint = assignedRegion.patrolPoints[patrolIndex];
            agent.SetDestination(patrolPoint.position);

            while (Vector3.Distance(transform.position, patrolPoint.position) > 1f)
            {
                yield return null;
            }

            patrolIndex = (patrolIndex + 1) % assignedRegion.patrolPoints.Count;
            yield return new WaitForSeconds(Random.Range(2, 5));
        }
    }

    // nevmash ������ �Ҵ�
    private void AssignNevmashItem()
    {
        if (unitCount % 3 == 0)
        {
            nevmashCount++;
            Debug.Log($"Nevmash ������ �Ҵ�: �� {nevmashCount}��");
            // ���⼭ nevmash �������� ������ ���ֿ��� �Ҵ��ϴ� ������ �߰��ϼ���.
        }
    }

    // ���� ������ ���� ��� �߰�
    private void EquipRandomItems()
    {
        if (equipment == null) return;

        ItemData[] allItems = DataBase.Instance.GetAllItems().ToArray();
        if (allItems.Length == 0) return;

        // ������ ���� ����
        List<ItemData> weapons = new List<ItemData>();
        List<ItemData> armors = new List<ItemData>();

        foreach (var item in allItems)
        {
            if (item.itemType == ItemType.Weapon)
                weapons.Add(item);
            else if (item.itemType == ItemType.Armor)
                armors.Add(item);
        }

        if (weapons.Count > 0)
            equipment.EquipItem(weapons[Random.Range(0, weapons.Count)]);

        if (armors.Count > 0)
        {
            equipment.EquipItem(armors[Random.Range(0, armors.Count)]);
        }
    }
}
