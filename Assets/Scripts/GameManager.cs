using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [Header("모든 오브젝트 관리 객체")]
    public DataBase dataBase;

    [Header("현재 존재하는 몬스터 목록")]
    public List<GameObject> listMonster = new List<GameObject>();

    [Header("현재 존재하는 게이트 목록")]
    public List<GateCtrl> listGate = new List<GateCtrl>();


    public static GameManager Instance;
    public List<NeighborhoodCtrl> allRegions = new List<NeighborhoodCtrl>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        //구역들 저장
        var objs = GameObject.FindObjectsOfType<NeighborhoodCtrl>();
        
        foreach(var o in objs)
        {
            allRegions.Add(o);
        }
        
    }


    /*임시 씬 사용시 전에있던 씬 파괴 안하고 사용하는법
     //씬을 두개 킨다. 현재 있는 씬과 ""의 씬을 킴
        //var a = SceneManager.LoadSceneAsync("",LoadSceneMode.Additive);
        //a 의 씬 즉 ""의 씬을 비활성화? 시킨다. 
        //a.allowSceneActivation = false;
     */
}
