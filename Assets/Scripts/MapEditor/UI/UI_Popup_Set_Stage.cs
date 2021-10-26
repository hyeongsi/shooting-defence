﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_Popup_Set_Stage : UI_Popup
{
    public UI_Popup_Set_Wave setWaveUI;

    public Dropdown waveCountDropDown;
    public const string INIT_STRING = "1";

    public override void Init()
    {
        base.Init();
    }

    private void OnEnable()
    {
        if (setWaveUI == null)
            return;

        if(setWaveUI.gameObject.activeSelf == true)
        {
            setWaveUI.gameObject.SetActive(false);
            if(waveCountDropDown != null)
            {
                waveCountDropDown.options.Clear();
                //waveCountDropDown.
            }
        }
        
    }
}
