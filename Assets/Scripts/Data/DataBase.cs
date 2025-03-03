using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase Instance { get; private set; } // 싱글톤 인스턴스

    [Header("아이템 데이터 파일")]
    [SerializeField] private List<ItemData> itemDatas;

    [Header("헌터 및 몬스터 데이터 파일")]
    [SerializeField] private List<UnitData> hunterDataFiles;
    [SerializeField] private List<UnitData> monsterDataFiles;

    [Header("게이트 데이터 파일")]
    [SerializeField] private List<GateData> gateDataFiles;

    [Header("스킬 데이터 파일")]
    [SerializeField] private List<SkillData> skillDataFiles;

    private Dictionary<string, UnitData> unitDataCache = new Dictionary<string, UnitData>();
    private Dictionary<Gate_Class, GateData> gateDataCache = new Dictionary<Gate_Class, GateData>();
    private List<SkillData> cachedSkills = new List<SkillData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 유지
            InitializeData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 데이터를 한 번만 로드하여 캐싱
    /// </summary>
    private void InitializeData()
    {
        // 헌터 및 몬스터 데이터를 하나의 캐시로 통합
        unitDataCache.Clear();
        foreach (var hunter in hunterDataFiles)
        {
            unitDataCache[hunter.unitName] = hunter;
        }
        foreach (var monster in monsterDataFiles)
        {
            unitDataCache[monster.unitName] = monster;
        }

        // 게이트 데이터 캐싱
        gateDataCache.Clear();
        foreach (var gate in gateDataFiles)
        {
            gateDataCache[gate.gateClass] = gate;
        }

        // 스킬 데이터 캐싱
        cachedSkills = new List<SkillData>(skillDataFiles);
    }

    /// <summary>
    /// 모든 아이템 데이터를 반환
    /// </summary>
    public List<ItemData> GetAllItems() => new List<ItemData>(itemDatas);

    /// <summary>
    /// 특정 이름의 유닛 데이터를 반환 (헌터 및 몬스터 공용)
    /// </summary>
    public UnitData GetUnitData(string name)
    {
        if (unitDataCache.TryGetValue(name, out UnitData data))
        {
            return data;
        }
        Debug.LogWarning($"[DataBase] 유닛 데이터({name}) 없음");
        return null;
    }

    /// <summary>
    /// 랜덤 헌터 반환 (헌터 생성용)
    /// </summary>
    public UnitData GetRandomHunter()
    {
        if (hunterDataFiles.Count == 0)
        {
            return null;
        }
        return hunterDataFiles[Random.Range(0, hunterDataFiles.Count)];
    }

    /// <summary>
    /// 랜덤 몬스터 반환 (게이트 스폰용)
    /// </summary>
    public UnitData GetRandomMonster()
    {
        if (monsterDataFiles.Count == 0)
        {
            return null;
        }

        return monsterDataFiles[Random.Range(0, monsterDataFiles.Count)];
    }

    /// <summary>
    /// 특정 등급의 게이트 데이터를 반환
    /// </summary>
    public GateData GetGateData(Gate_Class gateClass)
    {
        if (gateDataCache.TryGetValue(gateClass, out GateData data))
        {
            return data;
        }
        Debug.LogWarning($"[DataBase] {gateClass} 등급의 게이트 데이터 없음.");
        return null;
    }

    public List<GateData> GetGateDatas()
    {
        return gateDataFiles;
    }
    /// <summary>
    /// 모든 스킬 데이터를 반환
    /// </summary>
    public List<SkillData> GetAllSkills()
    {
        return new List<SkillData>(cachedSkills);
    }

    /// <summary>
    /// 랜덤 스킬 반환
    /// </summary>
    public SkillData GetRandomSkill()
    {
        if (cachedSkills.Count == 0)
        {
            Debug.LogWarning("[DataBase] 스킬 데이터가 없습니다.");
            return null;
        }
        return cachedSkills[Random.Range(0, cachedSkills.Count)];
    }


}
