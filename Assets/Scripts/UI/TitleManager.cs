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
        EditorApplication.ExitPlaymode(); // 유니티 에디터에서 플레이모드 종료
#else
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }
}
