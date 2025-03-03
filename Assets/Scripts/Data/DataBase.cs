using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase Instance { get; private set; } // �̱��� �ν��Ͻ�

    [Header("������ ������ ����")]
    [SerializeField] private List<ItemData> itemDatas;

    [Header("���� �� ���� ������ ����")]
    [SerializeField] private List<UnitData> hunterDataFiles;
    [SerializeField] private List<UnitData> monsterDataFiles;

    [Header("����Ʈ ������ ����")]
    [SerializeField] private List<GateData> gateDataFiles;

    [Header("��ų ������ ����")]
    [SerializeField] private List<SkillData> skillDataFiles;

    private Dictionary<string, UnitData> unitDataCache = new Dictionary<string, UnitData>();
    private Dictionary<Gate_Class, GateData> gateDataCache = new Dictionary<Gate_Class, GateData>();
    private List<SkillData> cachedSkills = new List<SkillData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ ����
            InitializeData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �����͸� �� ���� �ε��Ͽ� ĳ��
    /// </summary>
    private void InitializeData()
    {
        // ���� �� ���� �����͸� �ϳ��� ĳ�÷� ����
        unitDataCache.Clear();
        foreach (var hunter in hunterDataFiles)
        {
            unitDataCache[hunter.unitName] = hunter;
        }
        foreach (var monster in monsterDataFiles)
        {
            unitDataCache[monster.unitName] = monster;
        }

        // ����Ʈ ������ ĳ��
        gateDataCache.Clear();
        foreach (var gate in gateDataFiles)
        {
            gateDataCache[gate.gateClass] = gate;
        }

        // ��ų ������ ĳ��
        cachedSkills = new List<SkillData>(skillDataFiles);
    }

    /// <summary>
    /// ��� ������ �����͸� ��ȯ
    /// </summary>
    public List<ItemData> GetAllItems() => new List<ItemData>(itemDatas);

    /// <summary>
    /// Ư�� �̸��� ���� �����͸� ��ȯ (���� �� ���� ����)
    /// </summary>
    public UnitData GetUnitData(string name)
    {
        if (unitDataCache.TryGetValue(name, out UnitData data))
        {
            return data;
        }
        Debug.LogWarning($"[DataBase] ���� ������({name}) ����");
        return null;
    }

    /// <summary>
    /// ���� ���� ��ȯ (���� ������)
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
    /// ���� ���� ��ȯ (����Ʈ ������)
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
    /// Ư�� ����� ����Ʈ �����͸� ��ȯ
    /// </summary>
    public GateData GetGateData(Gate_Class gateClass)
    {
        if (gateDataCache.TryGetValue(gateClass, out GateData data))
        {
            return data;
        }
        Debug.LogWarning($"[DataBase] {gateClass} ����� ����Ʈ ������ ����.");
        return null;
    }

    public List<GateData> GetGateDatas()
    {
        return gateDataFiles;
    }
    /// <summary>
    /// ��� ��ų �����͸� ��ȯ
    /// </summary>
    public List<SkillData> GetAllSkills()
    {
        return new List<SkillData>(cachedSkills);
    }

    /// <summary>
    /// ���� ��ų ��ȯ
    /// </summary>
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
