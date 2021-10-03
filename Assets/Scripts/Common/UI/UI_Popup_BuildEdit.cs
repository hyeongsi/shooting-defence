using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_Popup_BuildEdit : UI_Popup
{
    public Text waveText;
    public Text stagePowerText;

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

    public void ChangeWaveText(InputField inputField)
    {
        const string EXPLANATION_TEXT = "Wave : ";
        const int MIN_WAVE = 1;

        string inputText = inputField.text;
        if (string.IsNullOrEmpty(inputText) || Int32.Parse(inputText) <= MIN_WAVE)
        {
            waveText.text = EXPLANATION_TEXT + MIN_WAVE.ToString();
        }
        else
        {
            waveText.text = EXPLANATION_TEXT + inputText;
        }
    }

    public void ChangeStagePowerText(InputField inputField)
    {
        const string EXPLANATION_TEXT = "Stage Power : ";
        const int MIN_STAGE_POWER = 100;

        string inputText = inputField.text;
        if (string.IsNullOrEmpty(inputText) || Int32.Parse(inputText) <= MIN_STAGE_POWER)
        {
            stagePowerText.text = EXPLANATION_TEXT + MIN_STAGE_POWER.ToString();
        }
        else
        {
            stagePowerText.text = EXPLANATION_TEXT + inputText;
        }
    }

    public void SetStageWave(Text text)
    {
        MapManager.Instance.StageData.Wave = Int32.Parse(text.text);
    }

    public void SetStagePower(Text text)
    {
        MapManager.Instance.StageData.StageEnemyPower = Int32.Parse(text.text);
    }
}
