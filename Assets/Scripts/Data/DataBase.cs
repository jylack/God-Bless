using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{

    [Header("헌터 데이터 파일")]
    public List<UnitData> hunterDataFiles;

    [Header("몬스터 데이터 파일")]
    public List<UnitData> monsterDataFiles;


    [Header("게이트 데이터 파일")]
    public List<GateData> gateDataFiles;

    [Header("스킬 데이터 파일")]
    public List<SkillData> skillDataFiles;

    private Dictionary<Gate_Class, GateData> gateDataCache = new Dictionary<Gate_Class, GateData>();
    private Dictionary<string, UnitData> unitDataCache = new Dictionary<string, UnitData>();
    private List<SkillData> cachedSkills = new List<SkillData>();
    //    public string CurrentRegion => currentRegion;
    private void Awake()
    {
        
    }

    public UnitData GetHunterData(string name)
    {
        return hunterDataFiles.Find(h => h.unitName == name);
    }

    public UnitData GetMonsterData(string name)
    {
        return monsterDataFiles.Find(m => m.unitName == name);
    }

    /// <summary>
    /// 랜덤 헌터 반환 (헌터 생성용)
    /// </summary>
    public UnitData GetRandomHunter()
    {
        if (hunterDataFiles.Count == 0) return null;
        return hunterDataFiles[Random.Range(0, hunterDataFiles.Count)];
    }

    /// <summary>
    /// 랜덤 몬스터 반환 (게이트 스폰용)
    /// </summary>
    public UnitData GetRandomMonster()
    {
        if (monsterDataFiles.Count == 0) return null;
        return monsterDataFiles[Random.Range(0, monsterDataFiles.Count)];
    }


    public GateData GetGateData(Gate_Class gateClass)
    {
        if (gateDataCache.TryGetValue(gateClass, out GateData data))
        {
            return data;
        }
        Debug.LogWarning($"[DataBase] {gateClass} 등급의 게이트 데이터 없음.");
        return null;
    }


    public UnitData GetUnitData(string name)
    {
        if (unitDataCache.TryGetValue(name, out UnitData data))
        {
            return data;
        }
        Debug.LogWarning($"[DataBase] 유닛 데이터({name}) 없음");
        return null;
    }

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
