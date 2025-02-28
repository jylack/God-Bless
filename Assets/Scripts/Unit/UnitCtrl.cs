using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitCtrl : MonoBehaviour
{
    public UnitData data; // 유닛 데이터 (헌터, 몬스터, 시민)
    public Region assignedRegion; // 할당된 지역
    private float hp;
    private List<SkillData> skills;
    private NavMeshAgent agent;
    private int patrolIndex = 0;

    private UnitEquipment equipment; // 장비 시스템 추가

    private static int unitCount = 0; // 유닛 수를 세기 위한 정적 변수
    private static int nevmashCount = 0; // nevmash 아이템 수를 세기 위한 정적 변수

    private void Start()
    {
        unitCount++; // 유닛 수 증가

        if (data == null) return;

        //체력 및 스킬 초기화
        hp = data.maxHp;
        skills = new List<SkillData>(data.skills);
        agent = GetComponent<NavMeshAgent>();

        // 장비 시스템 초기화
        equipment = GetComponent<UnitEquipment>();
        if (equipment == null)
        {
            equipment = gameObject.AddComponent<UnitEquipment>();
        }

        // 유닛이 헌터 또는 시민이면 순찰 시작
        if (data.unitType == UnitType.Citizen || data.unitType == UnitType.Hunter)
        {
            StartCoroutine(PatrolRoutine());
        }

        // nevmash 아이템 할당
        AssignNevmashItem();

        //임시코드
        // 랜덤한 아이템 장착
        EquipRandomItems();
    }

    private void OnDestroy()
    {
        unitCount--; // 유닛 수 감소
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

    // nevmash 아이템 할당
    private void AssignNevmashItem()
    {
        if (unitCount % 3 == 0)
        {
            nevmashCount++;
            Debug.Log($"Nevmash 아이템 할당: 총 {nevmashCount}개");
            // 여기서 nevmash 아이템을 실제로 유닛에게 할당하는 로직을 추가하세요.
        }
    }

    // 랜덤 아이템 장착 기능 추가
    private void EquipRandomItems()
    {
        if (equipment == null) return;

        ItemData[] allItems = DataBase.Instance.GetAllItems().ToArray();
        if (allItems.Length == 0) return;

        // 무작위 무기 장착
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
