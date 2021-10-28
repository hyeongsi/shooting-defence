using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_Popup_Set_Wave : UI_Popup
{
    [SerializeField]
    private InputField editWaveCountInputField;
    [SerializeField]
    private InputField waveCountInputField;

    public const string INIT_STRING = "1";
    public const int INIT_WAVE_COUNT = 1;

    public override void Init()
    {
        base.Init();
        MapEditorController.Instance.InitWaveData();
    }

    public void InitWaveData()
    {
        editWaveCountInputField.text = INIT_STRING;
        waveCountInputField.text = INIT_STRING;

        MapEditorController.Instance.InitWaveData();
    }

    public void EditWaveCount()
    {
        if (String.IsNullOrEmpty(editWaveCountInputField.text))
            return;

        int waveCountInputFieldValue = int.Parse(editWaveCountInputField.text);

        if (waveCountInputFieldValue <= 0)
        {
            waveCountInputFieldValue = 1;
            editWaveCountInputField.text = INIT_STRING;
        }

        waveCountInputField.text = editWaveCountInputField.text;
        MapEditorController.Instance.SetSpawnEnemyInfoList(waveCountInputFieldValue);
    }

    public int GetWaveCountNumber()
    {
        int result = 1;
        int.TryParse(waveCountInputField.text, out result);

        return result;
    }
}
