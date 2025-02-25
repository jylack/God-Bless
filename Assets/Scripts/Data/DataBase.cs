using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterName
{
    Slime, end
}

/// <summary>
/// ��� ������Ʈ ���� ������ ���� Ŭ����
/// ��� ������Ʈ ������ Ǯ�� ������ ���� Ŭ����
/// </summary>
public class DataBase : MonoBehaviour
{
    
    [Header("���� ������ ������ ���")]//��޺� ����
    public List<GameObject> monsterObj = new List<GameObject>();
        
    [Header("���� ������ ������ ���")]
    public List<UnitData> hunterDataTypes = new List<UnitData>();
    
    [Header("����Ʈ ���� ���")]//��޺� ���Ͱ� ������ ��޺� ����Ʈ.
    public List<GameObject> gateTypes = new List<GameObject>();    
    
    [Header("�ο� �� �� �ִ� ��ų ���")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    /// <summary>
    /// ���õ� ���� ����� �ϳ� ��ȯ
    /// </summary>
    /// <param name="name">���ϴ� ���� ���� �̸� �Է�</param>
    /// <returns></returns>
    public GameObject GetMonster(MonsterName name)
    {
        return monsterObj[(int)name];
    }

    //public UnitData GetData(UnitType type)
    //{
    //    if(type == UnitType.Hunter)
    //    {
    //        return 
    //    }
    //}


}
