using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_Popup_SelectStage : UI_Popup
{
    public override void Init()
    {
        base.Init();
        gameObject.SetActive(false);
    }
}
