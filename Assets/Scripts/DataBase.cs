using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 오브젝트 풀을 가지고 있을 클래스
/// </summary>
public class DataBase : MonoBehaviour
{
    [Header("몬스터 종류 목록")]
    public List<UnitCtrl> monsterTypes = new List<UnitCtrl>();
    [Header("몬스터 종류별 데이터 목록")]
    public List<UnitData> monsterDataTypes = new List<UnitData>();

    [Header("헌터 종류 목록")]
    public List<UnitCtrl> hunterTypes = new List<UnitCtrl>();
    [Header("헌터 종류별 데이터 목록")]
    public List<UnitData> hunterDataTypes = new List<UnitData>();

    [Header("게이트 종류 목록")]
    public List<MonsterSpawner> gateTypes = new List<MonsterSpawner>();
    //[Header("헌터 종류별 데이터 목록")]
    //public List<GataData> gateDataTypes = new List<GataData>();

    [Header("부여 할 수 있는 스킬 목록")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    


}
