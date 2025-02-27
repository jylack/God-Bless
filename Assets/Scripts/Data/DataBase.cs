using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{

    [Header("���� ������ ����")]
    public List<UnitData> hunterDataFiles;

    [Header("���� ������ ����")]
    public List<UnitData> monsterDataFiles;


    [Header("����Ʈ ������ ����")]
    public List<GateData> gateDataFiles;

    [Header("��ų ������ ����")]
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
    /// ���� ���� ��ȯ (���� ������)
    /// </summary>
    public UnitData GetRandomHunter()
    {
        if (hunterDataFiles.Count == 0) return null;
        return hunterDataFiles[Random.Range(0, hunterDataFiles.Count)];
    }

    /// <summary>
    /// ���� ���� ��ȯ (����Ʈ ������)
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
        Debug.LogWarning($"[DataBase] {gateClass} ����� ����Ʈ ������ ����.");
        return null;
    }


    public UnitData GetUnitData(string name)
    {
        if (unitDataCache.TryGetValue(name, out UnitData data))
        {
            return data;
        }
        Debug.LogWarning($"[DataBase] ���� ������({name}) ����");
        return null;
    }

    public SkillData GetRandomSkill()
    {
        if (cachedSkills.Count == 0)
        {
            Debug.LogWarning("[DataBase] ��ų �����Ͱ� �����ϴ�.");
            return null;
        }
        return cachedSkills[Random.Range(0, cachedSkills.Count)];
    }
}
