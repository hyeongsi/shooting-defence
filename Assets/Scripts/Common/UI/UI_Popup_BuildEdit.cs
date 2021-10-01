using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_BuildEdit : UI_Popup
{
    public override void ShowPopupUI()
    {
        UIManager.Instance.ShowPopupUI(gameObject.name);
    }
}
