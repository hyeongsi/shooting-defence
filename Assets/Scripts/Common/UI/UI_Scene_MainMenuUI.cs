using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_MainMenuUI :MonoBehaviour
{
    public Canvas stageCanvas;

    private void Start()
    {
        Cursor.visible = true;
    }

    public void OpenStageCanvas()
    {
        stageCanvas.gameObject.SetActive(true);
    }

    public void OpenMapEditor()
    {
        GameManager.Instance.LoadScene(GameManager.PlayStates.MAP_EDIT, -1);
    }

    public void ExitGame()
    {

#if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying=false; 
#else 
        Application.Quit();
#endif
    }
}
