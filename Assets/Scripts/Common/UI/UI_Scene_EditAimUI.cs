using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene_EditAimUI : UI_Scene
{
    private void Start()
    {
        Init();
        ShowSceneUI();
        SwitchCursorLockState();
    }

    public void SwitchCursorLockState()
    {
        if (GameManager.Instance.IsPause)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
