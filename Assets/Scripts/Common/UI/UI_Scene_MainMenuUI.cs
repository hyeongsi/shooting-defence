using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene_MainMenuUI : UI_Scene
{
    private void Start()
    {
        Init();
        ShowSceneUI();
    }

    public void StartGame()
    {
        UIManager.Instance.ShowPopupUI(UIManager.MainMenuPopUpUI.SelectStage_Canvas.ToString());
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
