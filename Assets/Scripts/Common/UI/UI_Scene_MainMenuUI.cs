using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ookii.Dialogs;
using System.IO;
using System.Windows.Forms;

public class UI_Scene_MainMenuUI :MonoBehaviour
{
    public Canvas stageCanvas;
    public Canvas characterCanvas;
    VistaOpenFileDialog openFileDialog;

    private void Start()
    {
        UnityEngine.Cursor.visible = true;

        openFileDialog = new VistaOpenFileDialog();
        openFileDialog.Filter = "Json (*.json) |*.json";
        openFileDialog.DefaultExt = "*.json";
    }

    public void OpenStageCanvas()
    {
        stageCanvas.gameObject.SetActive(true);
    }

    public void OpenMapEditor()
    {
        GameManager.Instance.LoadScene(GameManager.PlayStates.MAP_EDIT, -1);
    }

    public void OpenCustomMap()
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string readJsonString = File.ReadAllText(openFileDialog.FileName);
            GameManager.Instance.customTileMap = JsonUtility.FromJson<MapEditorController.CustomTileMap>(readJsonString);
            
            characterCanvas.gameObject.SetActive(true);
            characterCanvas.GetComponent<UI_Popup_SelectCharacter>().stageIndex = GameManager.CUSTOM_MAP;
        }
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
