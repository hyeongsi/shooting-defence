using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_Popup_Set_Stage : UI_Popup
{
    public UI_Popup_Set_Wave setWaveUI;

    public Dropdown waveCountDropDown;
    public const string INIT_INT_STRING = "1";
    public const string INIT_FLOAT_STRING = "0.1f";
    public const int INIT_INT = 1;
    public const float INIT_FLOAT = 0.1f;

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
    [Header("enemy spawn list stat")]
    public InputField enemySpawnListHPInputField;
    public InputField enemySpawnListDamageSpeedInputField;
    public InputField enemySpawnListAttackDelayInputField;
    public InputField enemySpawnListAttackRangeInputField;
    public InputField enemySpawnListMoveSpeedInputField;

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

        List<SpawnEnemyInfo> spawnEnemyList = MapEditorController.Instance.GetSpawnEnemyInfoList();
        for (int i = 0; i < spawnEnemyList[0].spawnEnemyList.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = ((EnemyManager.EnemyName)spawnEnemyList[0].spawnEnemyList[i]).ToString(); 
            spawnEnemyListDropDown.options.Add(option);
        }

        spawnEnemyListDropDown.value = 0;
    }

    public void SetAllInputFieldInitText()
    {
        waitTimeBeforeSpawnInputField.text = INIT_FLOAT_STRING;
        spawnDelayInputField.text = INIT_FLOAT_STRING;
        spawnCountInputField.text = INIT_INT_STRING;
        spawnEnemyListDropDown.captionText.text = "";

        enemySpawnListHPInputField.text = "";
        enemySpawnListDamageSpeedInputField.text = "";
        enemySpawnListAttackDelayInputField.text = "";
        enemySpawnListAttackRangeInputField.text = "";
        enemySpawnListMoveSpeedInputField.text = "";
    }

    public void UpdateSpawnEnemyListDropDown()
    {
        if (MapEditorController.Instance.GetSpawnEnemyInfoList().Count <= 0)
            return;

        List<SpawnEnemyInfo> spawnEnemyList = MapEditorController.Instance.GetSpawnEnemyInfoList();

        spawnEnemyListDropDown.options.Clear();

        for (int i = 0; i < spawnEnemyList[waveCountDropDown.value].spawnEnemyList.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = ((EnemyManager.EnemyName)spawnEnemyList[waveCountDropDown.value].spawnEnemyList[i]).ToString();               // 나중에 몹 이름 넣어서 들고 있도록 변경하기
            spawnEnemyListDropDown.options.Add(option);
        }

        spawnEnemyListDropDown.value = 0;

        if (spawnEnemyListDropDown.options.Count <= 0)
        {
            SetAllInputFieldInitText();
            return;
        } 

        spawnEnemyListDropDown.captionText.text = ((EnemyManager.EnemyName)spawnEnemyList[waveCountDropDown.value].spawnEnemyList[0]).ToString();
        OnValueChangeSpawnEnemyList();
    }

    public void SetInpuFieldDefaultValue(InputField inputField)
    {
        int parseValue;

        if(inputField == waitTimeBeforeSpawnInputField || inputField == spawnDelayInputField)
        {
            if (!int.TryParse(inputField.text, out parseValue))
            {
                inputField.text = INIT_FLOAT_STRING;
                return;
            }
               
            if(parseValue < 0.1f)
            {
                inputField.text = INIT_FLOAT_STRING;
            }
        }
        else if (inputField == spawnCountInputField)
        {
            if (!int.TryParse(inputField.text, out parseValue))
            {
                inputField.text = INIT_INT_STRING;
                return;
            }

            if (parseValue < 1.0f)
            {
                inputField.text = INIT_INT_STRING;
            }
        }
    }

    public void InitInputFields()   // 인풋필드 데이터를 해당 ui를 처음 켰을 때 정보로 초기화
    {
        List<SpawnEnemyInfo> spawnEnemyInfo = MapEditorController.Instance.GetSpawnEnemyInfoList();

        if (spawnEnemyInfo == null)
            return;
        if (spawnEnemyInfo.Count <= 0)
            return;

        if(spawnEnemyInfo[0].spawnEnemyList.Count == 0)
        {
            waitTimeBeforeSpawnInputField.text = INIT_FLOAT_STRING;
            spawnDelayInputField.text = INIT_FLOAT_STRING;
            spawnCountInputField.text = INIT_INT_STRING;
        }
        else
        {
            waitTimeBeforeSpawnInputField.text = spawnEnemyInfo[0].waitTimeBeforeSpawnList[0].ToString();
            spawnDelayInputField.text = spawnEnemyInfo[0].spawnDelayList[0].ToString();
            spawnCountInputField.text = spawnEnemyInfo[0].spawnCountList[0].ToString();
        }

        UpdateSelectEnemy();
    }

    public void UpdateSelectEnemy() // 선택한 enemy에 따라 선택한 enemy의 정보 출력 업데이트 메소드
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

    public void AddEnemyToEnemyList()   // + 버튼 누르면 enemyspawnlist에 선택한 enemy 정보 추가
    {
        List<SpawnEnemyInfo> spawnEnemyInfo = MapEditorController.Instance.GetSpawnEnemyInfoList();

        spawnEnemyInfo[waveCountDropDown.value].spawnEnemyList.Add((int)MapEditorController.Instance.SelectEnemyIndex);
        spawnEnemyInfo[waveCountDropDown.value].waitTimeBeforeSpawnList.Add(float.Parse(waitTimeBeforeSpawnInputField.text));
        spawnEnemyInfo[waveCountDropDown.value].spawnDelayList.Add(float.Parse(spawnDelayInputField.text));
        spawnEnemyInfo[waveCountDropDown.value].spawnCountList.Add(int.Parse(spawnCountInputField.text));

        Dropdown.OptionData option = new Dropdown.OptionData();
        option.text = MapEditorController.Instance.SelectEnemyIndex.ToString();
        spawnEnemyListDropDown.options.Add(option);

        spawnEnemyListDropDown.value = spawnEnemyListDropDown.options.Count - 1;
        spawnEnemyListDropDown.captionText.text = MapEditorController.Instance.SelectEnemyIndex.ToString();

        enemySpawnListHPInputField.text = enemyHPInputField.text;
        enemySpawnListDamageSpeedInputField.text = enemyDamageSpeedInputField.text;
        enemySpawnListAttackDelayInputField.text = enemyAttackDelayInputField.text;
        enemySpawnListAttackRangeInputField.text = enemyAttackRangeInputField.text;
        enemySpawnListMoveSpeedInputField.text = enemyMoveSpeedInputField.text;
    }

    public void UpdateSpawnEnemyListInfo()   // update 버튼 누르면 spawn enemy list에 설정한 몹과 같이 설정한 스폰정보 수정
    {
        if (spawnEnemyListDropDown.options.Count <= 0)
            return;

        List<SpawnEnemyInfo> spawnEnemyInfo = MapEditorController.Instance.GetSpawnEnemyInfoList();

        if (spawnEnemyInfo[waveCountDropDown.value].spawnEnemyList.Count <= 0)
            return;

        // 드롭다운 텍스트도 바꿔야 함

        spawnEnemyInfo[waveCountDropDown.value].spawnEnemyList[spawnEnemyListDropDown.value] = ((int)MapEditorController.Instance.SelectEnemyIndex);
        spawnEnemyInfo[waveCountDropDown.value].waitTimeBeforeSpawnList[spawnEnemyListDropDown.value] = (float.Parse(waitTimeBeforeSpawnInputField.text));
        spawnEnemyInfo[waveCountDropDown.value].spawnDelayList[spawnEnemyListDropDown.value] = (float.Parse(spawnDelayInputField.text));
        spawnEnemyInfo[waveCountDropDown.value].spawnCountList[spawnEnemyListDropDown.value] = (int.Parse(spawnCountInputField.text));

        OnValueChangeSpawnEnemyList();
    }

    public void OnValueChangeSpawnEnemyList()   // 선택한 웨이브 몹 정보 출력 업데이트
    {
        List<SpawnEnemyInfo> spawnEnemyInfo = MapEditorController.Instance.GetSpawnEnemyInfoList();

        waitTimeBeforeSpawnInputField.text = spawnEnemyInfo[waveCountDropDown.value].waitTimeBeforeSpawnList[spawnEnemyListDropDown.value].ToString();
        spawnDelayInputField.text = spawnEnemyInfo[waveCountDropDown.value].spawnDelayList[spawnEnemyListDropDown.value].ToString();
        spawnCountInputField.text = spawnEnemyInfo[waveCountDropDown.value].spawnCountList[spawnEnemyListDropDown.value].ToString();

        EnemyStaticData enemyStaticData = EnemyManager.Instance.GetEnemyStaticData((EnemyManager.EnemyName)spawnEnemyInfo[waveCountDropDown.value].spawnEnemyList[spawnEnemyListDropDown.value]);

        enemySpawnListHPInputField.text = enemyStaticData.maxHp.ToString();
        enemySpawnListDamageSpeedInputField.text = enemyStaticData.attackDamage.ToString();
        enemySpawnListAttackDelayInputField.text = enemyStaticData.attackDelay.ToString();
        enemySpawnListAttackRangeInputField.text = enemyStaticData.attackRange.ToString();
        enemySpawnListMoveSpeedInputField.text = enemyStaticData.moveSpeed.ToString();
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
