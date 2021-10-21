using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_Popup_Create_Base : UI_Popup
{
    public InputField xInputField;
    public InputField yInputField;

    public override void Init()
    {
        base.Init();
    }

    public void SetInputValue()
    {
        MapEditorController.Instance.XValue = Int32.Parse(xInputField.text);
        MapEditorController.Instance.YValue = Int32.Parse(yInputField.text);
    }

}
