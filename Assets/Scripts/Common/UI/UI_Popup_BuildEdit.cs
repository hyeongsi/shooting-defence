using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_BuildEdit : UI_Popup
{
    public override void Init()
    {
        base.Init();
    }

    public override object Instance()
    {
        return this;
    }

    public override void ShowPopupUI()
    {
        UIManager.Instance.ShowPopupUI(gameObject.name);
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
