using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_Popup_Set_Wave : UI_Popup
{
    public InputField waveCountInputField;
    public Dropdown editEnemyDropdown;
    public const string INIT_STRING = "1";

    public override void Init()
    {
        base.Init();
    }

    public void InitWaveData()
    {
        waveCountInputField.text = INIT_STRING;
        SetEnemyDropDown(1);
        editEnemyDropdown.value = 0;
    }

    public void SetEnemyDropDown(int value)
    {
        if(value <= 0)
        {
            value = 1;
        }

        editEnemyDropdown.options.Clear();
        
        for(int i = 0; i < value; i ++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = (i+1).ToString();
            editEnemyDropdown.options.Add(option);
        }
    }

    public void EditWaveCount()
    {
        int waveCountInputFieldValue = int.Parse(waveCountInputField.text);

        if (waveCountInputFieldValue <= 0)
        {
            waveCountInputFieldValue = 1;
            waveCountInputField.text = INIT_STRING;
        }

        SetEnemyDropDown(waveCountInputFieldValue);
    }
}
