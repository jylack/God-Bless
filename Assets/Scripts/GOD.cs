using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;


public class GOD : MonoBehaviour
{
    public DataBase dataBase;
    public List<UnitCtrl> listUnit = new List<UnitCtrl>();
    public List<SkillData> godSkills = new List<SkillData>(); // GOD이 보유한 스킬 목록


    private void Start()
    {
        LoadInitialSkills();
    }

    private void LoadInitialSkills()
    {
        godSkills = dataBase.GetAllSkills();

        if (godSkills == null || godSkills.Count == 0)
        {
            Debug.LogError("[God] 보유한 스킬이 없습니다! DataBase에서 스킬을 올바르게 로드했는지 확인하세요.");
        }
        else
        {
            //Debug.Log($"[God] 총 {godSkills.Count}개의 스킬을 보유.");
        }
    }
    /// <summary>
    /// GOD이 유닛을 생성할 때, 보유한 스킬 중 랜덤으로 하나를 부여하고 생성
    /// </summary>
    public void AutoUnitCreate()
    {
        if (godSkills == null || godSkills.Count == 0)
        {
            Debug.LogError("[God] GOD이 보유한 부여 스킬이 없습니다!");
            return;
        }

        UnitData randomHunterData = dataBase.GetRandomHunter();
        if (randomHunterData == null)
        {
            Debug.LogError("[God] 생성 가능한 헌터 데이터가 없습니다. DataBase를 확인하세요.");
            return;
        }

        SkillData randomSkill = godSkills[Random.Range(0, godSkills.Count)];
        if (randomSkill == null)
        {
            Debug.LogError("[God] 선택된 랜덤 스킬이 NULL입니다. godSkills 리스트를 확인하세요.");
            return;
        }

        Vector3 spawnPosition = GetCameraSpawnPosition();
        if (!IsValidVector(spawnPosition))
        {
            Debug.LogError($"[God] 유효하지 않은 위치 ({spawnPosition})을 감지했습니다! 기본 위치로 이동.");
            spawnPosition = new Vector3(0, 1, 0);
        }

        //Debug.Log($"[God] 최종 유닛 생성 위치: {spawnPosition}");

        GameObject hunterGO = new GameObject(randomHunterData.unitName);
        hunterGO.tag = "Hunter";
        hunterGO.transform.position = spawnPosition;

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        if (newHunter == null)
        {
            Debug.LogError("[God] UnitCtrl 컴포넌트가 추가되지 않았습니다!");
            return;
        }

        NavMeshAgent agent = hunterGO.AddComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        agent.acceleration = 8f;
        agent.angularSpeed = 120f;

        newHunter.data = randomHunterData;
        newHunter.ReceiveSkill(randomSkill);

        // 현재 지역의 순찰 포인트 중 하나를 네비게이션 목표로 설정
        Vector3 patrolPoint = GetRandomPatrolPoint();
        if (IsValidVector(patrolPoint))
        {
            agent.SetDestination(patrolPoint);
            Debug.Log($"[God] {randomHunterData.unitName}의 초기 네비게이션 목표: {patrolPoint}");
        }
        else
        {
            Debug.LogError($"[God] {randomHunterData.unitName}의 네비게이션 목표 설정 실패! 기본 위치 유지.");
        }

        listUnit.Add(newHunter);
        Debug.Log($"[God] 새로운 헌터 생성 완료! 위치: {spawnPosition} / 목표: {patrolPoint} / 부여된 스킬: {randomSkill.skillName}");
    }

    private Vector3 GetRandomPatrolPoint()
    {
        string currentRegion = CameraManager.Instance.CurrentRegion;
        if (string.IsNullOrEmpty(currentRegion))
        {
            Debug.LogError("[God] 현재 지역을 찾을 수 없습니다! CameraManager가 활성화되어 있는지 확인하세요.");
            return new Vector3(0, 1, 0);
        }

        GameObject regionObject = GameObject.Find($"WayPoint_{currentRegion}");
        if (regionObject == null)
        {
            Debug.LogError($"[God] {currentRegion} 지역의 웨이포인트를 찾을 수 없습니다!");
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
            Debug.LogError($"[God] {currentRegion} 지역에 사용 가능한 웨이포인트가 없습니다!");
            return new Vector3(0, 1, 0);
        }

        Transform selectedWayPoint = validPoints[Random.Range(0, validPoints.Count)];
        Vector3 patrolPosition = selectedWayPoint.position;

        Debug.Log($"[God] {currentRegion} 지역에서 선택된 순찰 위치: {patrolPosition}");

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(patrolPosition, out hit, 5f, NavMesh.AllAreas))
        {
            //Debug.LogWarning($"[God] {patrolPosition} 위치에 NavMesh가 없습니다! 가까운 NavMesh에 배치합니다.");
            return new Vector3(0, 1, 0);
        }

        return hit.position;
    }


    /// <summary>
    /// 현재 활성화된 CinemachineVirtualCamera를 찾아 카메라의 위치 앞쪽에서 유닛을 생성
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCameraSpawnPosition()
    {
        Cinemachine.CinemachineVirtualCamera activeCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (activeCamera == null)
        {
            Debug.LogError("[God] 활성화된 CinemachineVirtualCamera를 찾을 수 없습니다!");
            return new Vector3(0, 1, 0); // 기본 위치 반환
        }

        Vector3 cameraPosition = activeCamera.transform.position;
        Vector3 spawnPosition = cameraPosition + (activeCamera.transform.forward * 5f) - new Vector3(0, 5f, 0); // 카메라 앞쪽 5m & 아래쪽 5m

        //Debug.Log($"[God] 현재 카메라 위치: {cameraPosition} / 초기 유닛 생성 위치 (NavMesh 확인 전): {spawnPosition}");

        // NavMesh에 배치 가능 여부 확인 (주변 10m 범위 탐색)
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(spawnPosition, out hit, 10f, NavMesh.AllAreas))
        {
            Debug.LogWarning($"[God] {spawnPosition} 위치에 NavMesh가 없습니다! 가까운 NavMesh에 배치합니다.");
            return new Vector3(0, 1, 0); // 기본 위치 설정
        }

        //Debug.Log($"[God] 최종 유닛 생성 위치 (NavMesh 적용 후): {hit.position}");
        return hit.position;
    }




    /// <summary>
    /// 현재 활성화된 지역의 웨이포인트 중 랜덤한 위치 반환
    /// </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        string currentRegion = CameraManager.Instance.CurrentRegion;
        if (string.IsNullOrEmpty(currentRegion))
        {
            Debug.LogError("[God] 현재 지역을 찾을 수 없습니다! CameraManager가 활성화되어 있는지 확인하세요.");
            return new Vector3(0, 1, 0); // 기본 위치 반환
        }

        GameObject regionObject = GameObject.Find($"WayPoint_{currentRegion}");
        if (regionObject == null)
        {
            Debug.LogError($"[God] {currentRegion} 지역의 웨이포인트를 찾을 수 없습니다!");
            return new Vector3(0, 1, 0); // 기본 위치 반환
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
            Debug.LogError($"[God] {currentRegion} 지역에 사용 가능한 웨이포인트가 없습니다!");
            return new Vector3(0, 1, 0); // 기본 위치 반환
        }

        Transform selectedWayPoint = validPoints[Random.Range(0, validPoints.Count)];
        Vector3 finalPosition = selectedWayPoint.position;

        // NavMesh에 배치 가능 여부 확인
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(finalPosition, out hit, 5f, NavMesh.AllAreas))
        {
            Debug.LogWarning($"[God] {finalPosition} 위치에 NavMesh가 없습니다! 가까운 NavMesh에 배치합니다.");
            return new Vector3(0, 1, 0); // 기본 위치 설정
        }

        return hit.position;
    }



    private IEnumerator WaitForUnitInitialization(UnitCtrl unit, SkillData skill)
    {
        yield return new WaitUntil(() => unit.data != null && unit.skills != null);

        unit.ReceiveSkill(skill);
        Debug.Log($"[God] {unit.data.unitName}에게 {skill.skillName} 스킬을 부여함.");
    }


    public void UnitCreate(string hunterName)
    {
        UnitData hunterData = dataBase.GetUnitData(hunterName);
        if (hunterData == null)
        {
            Debug.LogWarning($"[God] 헌터 데이터({hunterName}) 없음.");
            return;
        }

        GameObject hunterGO = new GameObject(hunterData.unitName);
        hunterGO.tag = "Hunter"; // 헌터 태그 자동 지정

        UnitCtrl newHunter = hunterGO.AddComponent<UnitCtrl>();
        newHunter.data = hunterData;

        

        
        listUnit.Add(newHunter);
        Debug.Log($"[God] 새로운 헌터 생성: {hunterName}, 태그 설정 완료!");
    }

   /// <summary>
    /// GOD이 보유한 스킬 중 하나를 유닛에게 부여
    /// </summary>
    public void GrantSkill(UnitCtrl targetUnit)
    {
        if (godSkills.Count == 0)
        {
            Debug.LogWarning("[God] 보유한 스킬이 없습니다!");
            return;
        }

        SkillData grantedSkill = godSkills[Random.Range(0, godSkills.Count)];
        targetUnit.ReceiveSkill(grantedSkill);

        Debug.Log($"[God] {targetUnit.data.unitName}에게 {grantedSkill.skillName} 스킬을 부여함.");
    }

    /// <summary>
    /// 위치 값이 유효한지 검사 (Infinity, NaN 방지)
    /// </summary>
    private bool IsValidVector(Vector3 position)
    {
        return !(float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z) ||
                 float.IsInfinity(position.x) || float.IsInfinity(position.y) || float.IsInfinity(position.z));
    }

}
