using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [Header("��� ������Ʈ ���� ��ü")]
    public DataBase dataBase;

    [Header("���� �����ϴ� ���� ���")]
    public List<GameObject> listMonster = new List<GameObject>();

    [Header("���� �����ϴ� ����Ʈ ���")]
    public List<GateCtrl> listGate = new List<GateCtrl>();


    public static GameManager Instance;
    public List<NeighborhoodCtrl> allRegions = new List<NeighborhoodCtrl>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        //������ ����
        var objs = GameObject.FindObjectsOfType<NeighborhoodCtrl>();
        
        foreach(var o in objs)
        {
            allRegions.Add(o);
        }
        
    }


    /*�ӽ� �� ���� �����ִ� �� �ı� ���ϰ� ����ϴ¹�
     //���� �ΰ� Ų��. ���� �ִ� ���� ""�� ���� Ŵ
        //var a = SceneManager.LoadSceneAsync("",LoadSceneMode.Additive);
        //a �� �� �� ""�� ���� ��Ȱ��ȭ? ��Ų��. 
        //a.allowSceneActivation = false;
     */
}
