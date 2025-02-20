using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������Ʈ Ǯ�� ������ ���� Ŭ����
/// </summary>
public class DataBase : MonoBehaviour
{
    [Header("���� ���� ���")]
    public List<UnitCtrl> monsterTypes = new List<UnitCtrl>();
    [Header("���� ������ ������ ���")]
    public List<UnitData> monsterDataTypes = new List<UnitData>();

    [Header("���� ���� ���")]
    public List<UnitCtrl> hunterTypes = new List<UnitCtrl>();
    [Header("���� ������ ������ ���")]
    public List<UnitData> hunterDataTypes = new List<UnitData>();

    [Header("����Ʈ ���� ���")]
    public List<MonsterSpawner> gateTypes = new List<MonsterSpawner>();
    //[Header("���� ������ ������ ���")]
    //public List<GataData> gateDataTypes = new List<GataData>();

    [Header("�ο� �� �� �ִ� ��ų ���")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    


}
