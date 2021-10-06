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

    public void StartSingleGame()
    {
        GameManager.Instance.PlayeState = GameManager.PlayStates.SINGLE_PLAY;
        
    }

    public void StartHostGame()
    {

    }

    public void BrowseGames()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
