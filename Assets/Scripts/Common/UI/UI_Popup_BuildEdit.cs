using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_BuildEdit : UI_Popup
{
    public override void Init()
    {
        base.Init();
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
