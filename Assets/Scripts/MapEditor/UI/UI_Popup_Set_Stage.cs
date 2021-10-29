using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_Popup_Set_Stage : UI_Popup
{
    public UI_Popup_Set_Wave setWaveUI;

    public Dropdown waveCountDropDown;
    public const string INIT_STRING = "1";

    public Dropdown spawnEnemyListDropDown;

    [Space]
    [Header("spawn")]
    public InputField waitTimeBeforeSpawnInputField;
    public InputField spawnDelayInputField;
    public InputField spawnCountInputField;

    [Space]
    [Header("enemy stat")]
    public InputField enemyHPInputField;
    public InputField enemyDamageSpeedInputField;
    public InputField enemyAttackDelayInputField;
    public InputField enemyAttackRangeInputField;
    public InputField enemyMoveSpeedInputField;

    [Space]
    public InputField selectEnemyInputField;

    public override void Init()
    {
        base.Init();
        MapEditorController.Instance.setSelectEnemyIndexDelegate += UpdateSelectEnemy;
    }

    public void InitEditWaveDropDown()
    {
        waveCountDropDown.options.Clear();

        for (int i = 0; i < setWaveUI.GetWaveCountNumber(); i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = (i + 1).ToString();
            waveCountDropDown.options.Add(option);
        }
        waveCountDropDown.value = 0;

        InitSpawnEnemyListDropDown();
    }

    public void InitSpawnEnemyListDropDown()
    {
        if (MapEditorController.Instance.GetSpawnEnemyInfoList().Count <= 0)
            return;

        spawnEnemyListDropDown.options.Clear();

        List<int> spawnEnemyList = MapEditorController.Instance.GetSpawnEnemyInfoList()[0].spawnEnemyList;
        for (int i = 0; i < spawnEnemyList.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = (i + 1).ToString();               // 나중에 몹 이름 넣어서 들고 있도록 변경하기
            spawnEnemyListDropDown.options.Add(option);
        }

        spawnEnemyListDropDown.value = -1;
    }

    public void SetInpuFieldDefaultValue(InputField inputField)
    {
        int parseValue;

        if(inputField == waitTimeBeforeSpawnInputField || inputField == spawnDelayInputField)
        {
            if (!int.TryParse(inputField.text, out parseValue))
            {
                inputField.text = "0.1";
                return;
            }
               
            if(parseValue < 0.1f)
            {
                inputField.text = "0.1";
            }
        }
        else if (inputField == spawnCountInputField)
        {
            if (!int.TryParse(inputField.text, out parseValue))
            {
                inputField.text = "1";
                return;
            }

            if (parseValue < 1.0f)
            {
                inputField.text = "1";
            }
        }
    }

    public void InitInputFields()
    {
        waitTimeBeforeSpawnInputField.text = "0.1";
        spawnDelayInputField.text = "0.1";
        spawnCountInputField.text = "1";

        UpdateSelectEnemy();
    }

    public void UpdateSelectEnemy()
    {
        selectEnemyInputField.text = MapEditorController.Instance.SelectEnemyIndex.ToString();

        EnemyStaticData enemyStaticData = EnemyManager.Instance.GetEnemyStaticData(MapEditorController.Instance.SelectEnemyIndex);

        if (enemyStaticData == null)
            return;

        enemyHPInputField.text = enemyStaticData.maxHp.ToString();
        enemyDamageSpeedInputField.text = enemyStaticData.maxHp.ToString();
        enemyAttackDelayInputField.text = enemyStaticData.attackDelay.ToString();
        enemyAttackRangeInputField.text = enemyStaticData.attackRange.ToString();
        enemyMoveSpeedInputField.text = enemyStaticData.moveSpeed.ToString();
    }

    private void OnEnable()
    {
        InitEditWaveDropDown();
        InitInputFields();
    }

    public enum UpdateEnemyList
    {
        ADD_ENEMYLIST = 1,
        DELETE_ENEMYLIST = 2,
        CLEAR_ENEMYLIST = 3,
    }
}
