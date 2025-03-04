using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;


public class GOD : MonoBehaviour
{
    public DataBase dataBase;
    public List<UnitCtrl> listUnit = new List<UnitCtrl>();
    public List<SkillData> godSkills = new List<SkillData>(); // GOD�� ������ ��ų ���


    private void Start()
    {
        LoadInitialSkills();
    }

    private void LoadInitialSkills()
    {
        godSkills = dataBase.GetAllSkills();

        if (godSkills == null || godSkills.Count == 0)
        {
            Debug.LogError("[God] ������ ��ų�� �����ϴ�! DataBase���� ��ų�� �ùٸ��� �ε��ߴ��� Ȯ���ϼ���.");
        }
        else
        {
            //Debug.Log($"[God] �� {godSkills.Count}���� ��ų�� ����.");
        }
    }
    /// <summary>
    /// GOD�� ������ ������ ��, ������ ��ų �� �������� �ϳ��� �ο��ϰ� ����
    /// </summary>
    public void AutoUnitCreate()
    {
        if (godSkills == null || godSkills.Count == 0)
        {
            Debug.LogError("[God] GOD�� ������ �ο� ��ų�� �����ϴ�!");
            return;
        }

        UnitData randomHunterData = dataBase.GetRandomHunter();
        if (randomHunterData == null)
        {
            Debug.LogError("[God] ���� ������ ���� �����Ͱ� �����ϴ�. DataBase�� Ȯ���ϼ���.");
            return;
        }

        SkillData randomSkill = godSkills[Random.Range(0, godSkills.Count)];
        if (randomSkill == null)
        {
            Debug.LogError("[God] ���õ� ���� ��ų�� NULL�Դϴ�. godSkills ����Ʈ�� Ȯ���ϼ���.");
            return;
        }

        Vector3 spawnPosition = GetCameraSpawnPosition();
        if (!IsValidVector(spawnPosition))
        {
            Debug.LogError($"[God] ��ȿ���� ���� ��ġ ({spawnPosition})�� �����߽��ϴ�! �⺻ ��ġ�� �̵�.");
            spawnPosition = new Vector3(0, 1, 0);
        }

        //Debug.Log($"[God] ���� ���� ���� ��ġ: {spawnPosition}");

        GameObject hunterGO = new GameObject(randomHunterData.unitName);
        hunterGO.tag = "Hunter";
        hunterGO.transform.position = spawnPosition;

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        if (newHunter == null)
        {
            Debug.LogError("[God] UnitCtrl ������Ʈ�� �߰����� �ʾҽ��ϴ�!");
            return;
        }

        NavMeshAgent agent = hunterGO.AddComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        agent.acceleration = 8f;
        agent.angularSpeed = 120f;

        newHunter.data = randomHunterData;
        newHunter.ReceiveSkill(randomSkill);

        // ���� ������ ���� ����Ʈ �� �ϳ��� �׺���̼� ��ǥ�� ����
        Vector3 patrolPoint = GetRandomPatrolPoint();
        if (IsValidVector(patrolPoint))
        {
            agent.SetDestination(patrolPoint);
            Debug.Log($"[God] {randomHunterData.unitName}�� �ʱ� �׺���̼� ��ǥ: {patrolPoint}");
        }
        else
        {
            Debug.LogError($"[God] {randomHunterData.unitName}�� �׺���̼� ��ǥ ���� ����! �⺻ ��ġ ����.");
        }

        listUnit.Add(newHunter);
        Debug.Log($"[God] ���ο� ���� ���� �Ϸ�! ��ġ: {spawnPosition} / ��ǥ: {patrolPoint} / �ο��� ��ų: {randomSkill.skillName}");
    }

    private Vector3 GetRandomPatrolPoint()
    {
        string currentRegion = CameraManager.Instance.CurrentRegion;
        if (string.IsNullOrEmpty(currentRegion))
        {
            Debug.LogError("[God] ���� ������ ã�� �� �����ϴ�! CameraManager�� Ȱ��ȭ�Ǿ� �ִ��� Ȯ���ϼ���.");
            return new Vector3(0, 1, 0);
        }

        GameObject regionObject = GameObject.Find($"WayPoint_{currentRegion}");
        if (regionObject == null)
        {
            Debug.LogError($"[God] {currentRegion} ������ ��������Ʈ�� ã�� �� �����ϴ�!");
            return new Vector3(0, 1, 0);
        }

        Transform[] wayPoints = regionObject.GetComponentsInChildren<Transform>();
        List<Transform> validPoints = new List<Transform>();

        foreach (Transform point in wayPoints)
        {
            if (point.name.StartsWith("WayPoint_"))
            {
                validPoints.Add(point);
            }
        }

        if (validPoints.Count == 0)
        {
            Debug.LogError($"[God] {currentRegion} ������ ��� ������ ��������Ʈ�� �����ϴ�!");
            return new Vector3(0, 1, 0);
        }

        Transform selectedWayPoint = validPoints[Random.Range(0, validPoints.Count)];
        Vector3 patrolPosition = selectedWayPoint.position;

        Debug.Log($"[God] {currentRegion} �������� ���õ� ���� ��ġ: {patrolPosition}");

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(patrolPosition, out hit, 5f, NavMesh.AllAreas))
        {
            //Debug.LogWarning($"[God] {patrolPosition} ��ġ�� NavMesh�� �����ϴ�! ����� NavMesh�� ��ġ�մϴ�.");
            return new Vector3(0, 1, 0);
        }

        return hit.position;
    }


    /// <summary>
    /// ���� Ȱ��ȭ�� CinemachineVirtualCamera�� ã�� ī�޶��� ��ġ ���ʿ��� ������ ����
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCameraSpawnPosition()
    {
        Cinemachine.CinemachineVirtualCamera activeCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (activeCamera == null)
        {
            Debug.LogError("[God] Ȱ��ȭ�� CinemachineVirtualCamera�� ã�� �� �����ϴ�!");
            return new Vector3(0, 1, 0); // �⺻ ��ġ ��ȯ
        }

        Vector3 cameraPosition = activeCamera.transform.position;
        Vector3 spawnPosition = cameraPosition + (activeCamera.transform.forward * 5f) - new Vector3(0, 5f, 0); // ī�޶� ���� 5m & �Ʒ��� 5m

        //Debug.Log($"[God] ���� ī�޶� ��ġ: {cameraPosition} / �ʱ� ���� ���� ��ġ (NavMesh Ȯ�� ��): {spawnPosition}");

        // NavMesh�� ��ġ ���� ���� Ȯ�� (�ֺ� 10m ���� Ž��)
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas))
        {
            Debug.LogWarning($"[God] {spawnPosition} ��ġ�� NavMesh�� �����ϴ�! ����� NavMesh�� ��ġ�մϴ�.");
            return new Vector3(0, 1, 0); // �⺻ ��ġ ����
        }

        //Debug.Log($"[God] ���� ���� ���� ��ġ (NavMesh ���� ��): {hit.position}");
        return hit.position;
    }




    /// <summary>
    /// ���� Ȱ��ȭ�� ������ ��������Ʈ �� ������ ��ġ ��ȯ
    /// </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        string currentRegion = CameraManager.Instance.CurrentRegion;
        if (string.IsNullOrEmpty(currentRegion))
        {
            Debug.LogError("[God] ���� ������ ã�� �� �����ϴ�! CameraManager�� Ȱ��ȭ�Ǿ� �ִ��� Ȯ���ϼ���.");
            return new Vector3(0, 1, 0); // �⺻ ��ġ ��ȯ
        }

        GameObject regionObject = GameObject.Find($"WayPoint_{currentRegion}");
        if (regionObject == null)
        {
            Debug.LogError($"[God] {currentRegion} ������ ��������Ʈ�� ã�� �� �����ϴ�!");
            return new Vector3(0, 1, 0); // �⺻ ��ġ ��ȯ
        }

        Transform[] wayPoints = regionObject.GetComponentsInChildren<Transform>();
        List<Transform> validPoints = new List<Transform>();

        foreach (Transform point in wayPoints)
        {
            if (point.name.StartsWith("WayPoint_"))
            {
                validPoints.Add(point);
            }
        }

        if (validPoints.Count == 0)
        {
            Debug.LogError($"[God] {currentRegion} ������ ��� ������ ��������Ʈ�� �����ϴ�!");
            return new Vector3(0, 1, 0); // �⺻ ��ġ ��ȯ
        }

        Transform selectedWayPoint = validPoints[Random.Range(0, validPoints.Count)];
        Vector3 finalPosition = selectedWayPoint.position;

        // NavMesh�� ��ġ ���� ���� Ȯ��
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(finalPosition, out hit, 5f, NavMesh.AllAreas))
        {
            Debug.LogWarning($"[God] {finalPosition} ��ġ�� NavMesh�� �����ϴ�! ����� NavMesh�� ��ġ�մϴ�.");
            return new Vector3(0, 1, 0); // �⺻ ��ġ ����
        }

        return hit.position;
    }



    private IEnumerator WaitForUnitInitialization(UnitCtrl unit, SkillData skill)
    {
        yield return new WaitUntil(() => unit.data != null && unit.skills != null);

        unit.ReceiveSkill(skill);
        Debug.Log($"[God] {unit.data.unitName}���� {skill.skillName} ��ų�� �ο���.");
    }


    public void UnitCreate(string hunterName)
    {
        UnitData hunterData = dataBase.GetUnitData(hunterName);
        if (hunterData == null)
        {
            Debug.LogWarning($"[God] ���� ������({hunterName}) ����.");
            return;
        }

        GameObject hunterGO = new GameObject(hunterData.unitName);
        hunterGO.tag = "Hunter"; // ���� �±� �ڵ� ����

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        newHunter.data = hunterData;

        

        
        listUnit.Add(newHunter);
        Debug.Log($"[God] ���ο� ���� ����: {hunterName}, �±� ���� �Ϸ�!");
    }

   /// <summary>
    /// GOD�� ������ ��ų �� �ϳ��� ���ֿ��� �ο�
    /// </summary>
    public void GrantSkill(UnitCtrl targetUnit)
    {
        if (godSkills.Count == 0)
        {
            Debug.LogWarning("[God] ������ ��ų�� �����ϴ�!");
            return;
        }

        SkillData grantedSkill = godSkills[Random.Range(0, godSkills.Count)];
        targetUnit.ReceiveSkill(grantedSkill);

        Debug.Log($"[God] {targetUnit.data.unitName}���� {grantedSkill.skillName} ��ų�� �ο���.");
    }

    /// <summary>
    /// ��ġ ���� ��ȿ���� �˻� (Infinity, NaN ����)
    /// </summary>
    private bool IsValidVector(Vector3 position)
    {
        return !(float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z) ||
                 float.IsInfinity(position.x) || float.IsInfinity(position.y) || float.IsInfinity(position.z));
    }

}
