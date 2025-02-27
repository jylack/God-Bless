using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitCtrl : MonoBehaviour
{
    public UnitData data;
    public Region assignedRegion; // 할당된 지역
    private float hp;
    private List<SkillData> skills;
    private NavMeshAgent agent;
    private int patrolIndex = 0;

    private void Start()
    {
        if (data == null) return;
        hp = data.maxHp;
        skills = new List<SkillData>(data.skills);
        agent = GetComponent<NavMeshAgent>();

        if (data.unitType == UnitType.Citizen || data.unitType == UnitType.Hunter)
        {
            StartCoroutine(PatrolRoutine());
        }

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

}
