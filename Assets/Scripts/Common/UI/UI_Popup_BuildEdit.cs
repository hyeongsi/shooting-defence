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

    public void ChangeWaveText()
    {
        const string EXPLANATION_TEXT = "Wave : ";
        const int MIN_WAVE = 1;

        int wave = MapManager.Instance.StageData.Wave;

        if (wave <= MIN_WAVE)
        {
            waveText.text = EXPLANATION_TEXT + MIN_WAVE.ToString();
        }
        else
        {
            waveText.text = EXPLANATION_TEXT + wave;
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

    public void ChangeStagePowerText()
    {
        const string EXPLANATION_TEXT = "Stage Power : ";
        const int MIN_STAGE_POWER = 100;

        int stageEnemyPower = MapManager.Instance.StageData.StageEnemyPower;

        if (stageEnemyPower <= MIN_STAGE_POWER)
        {
            stagePowerText.text = EXPLANATION_TEXT + MIN_STAGE_POWER.ToString();
        }
        else
        {
            stagePowerText.text = EXPLANATION_TEXT + stageEnemyPower;
        }
    }

    public void SetStageWave(Text text)
    {
        const string EXPLANATION_TEXT = "Wave : ";
        MapManager.Instance.StageData.Wave = int.Parse(text.text.Substring(EXPLANATION_TEXT.Length));
    }

    public void SetStagePower(Text text)
    {
        const string EXPLANATION_TEXT = "Stage Power : ";
        MapManager.Instance.StageData.StageEnemyPower = int.Parse(text.text.Substring(EXPLANATION_TEXT.Length));
    }
}
