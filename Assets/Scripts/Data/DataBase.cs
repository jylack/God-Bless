using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterName
{
    Slime, end
}

/// <summary>
/// 모든 오브젝트 종류 가지고 있을 클래스
/// 모든 오브젝트 종류의 풀을 가지고 있을 클래스
/// </summary>
public class DataBase : MonoBehaviour
{
    
    [Header("몬스터 종류별 데이터 목록")]//등급별 몬스터
    public List<GameObject> monsterObj = new List<GameObject>();
        
    [Header("헌터 종류별 데이터 목록")]
    public List<UnitData> hunterDataTypes = new List<UnitData>();
    
    [Header("게이트 종류 목록")]//등급별 몬스터가 나오는 등급별 게이트.
    public List<GameObject> gateTypes = new List<GameObject>();    
    
    [Header("부여 할 수 있는 스킬 목록")]
    public List<GrantSkill> listGrantSkill = new List<GrantSkill>();

    /// <summary>
    /// 세팅된 몬스터 목록중 하나 반환
    /// </summary>
    /// <param name="name">원하는 몬스터 종류 이름 입력</param>
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
