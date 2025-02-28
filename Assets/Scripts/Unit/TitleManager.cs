using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject area_select_UI;


    void Start()
    {
        Time.timeScale = 0;
    }

    public void OnStartButtonClicked()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        area_select_UI.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); // ����Ƽ �����Ϳ��� �÷��̸�� ����
#else
        Application.Quit(); // ����� ���ӿ��� ����
#endif
    }
}
