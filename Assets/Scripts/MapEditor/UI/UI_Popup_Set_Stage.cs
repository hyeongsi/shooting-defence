using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class UI_Popup_Set_Stage : UI_Popup
{
    public UI_Popup_Set_Wave setWaveUI;

    public Dropdown waveCountDropDown;
    public const string INIT_INT_STRING = "1";
    public const string INIT_FLOAT_STRING = "0.1f";
    public const int INIT_INT = 1;
    public const float INIT_FLOAT = 0.1f;

    public Dropdown spawnEnemyListDropDown;

    private List<SpawnEnemyInfo> spawnEnemyList;        // 스테이지 별 소환 몹 리스트

    [Space]
    [Header("spawn")]
    public InputField waitTimeBeforeSpawnInputField;    // 스테이지 시작 전 시간
    public InputField spawnDelayInputField;             // 몹 소환 딜레이
    public InputField spawnCountInputField;             // 몹 소환 수량 

    [Space]
    [Header("enemy stat")]
    public InputField enemyHPInputField;
    public InputField enemyDamageInputField;
    public InputField enemyAttackDelayInputField;
    public InputField enemyAttackRangeInputField;
    public InputField enemyMoveSpeedInputField;

    [Space]
    [Header("enemy spawn list stat")]
    public InputField enemySpawnListHPInputField;
    public InputField enemySpawnListDamageInputField;
    public InputField enemySpawnListAttackDelayInputField;
    public InputField enemySpawnListAttackRangeInputField;
    public InputField enemySpawnListMoveSpeedInputField;

    [Space]
    public InputField selectEnemyInputField;

    public override void Init()
    {
        base.Init();
        MapEditorController.Instance.setSelectEnemyIndexDelegate += UpdateSelectEnemy;
        spawnEnemyList = MapEditorController.Instance.GetSpawnEnemyInfoList();
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
        waveCountDropDown.captionText.text = waveCountDropDown.options[waveCountDropDown.value].text;
        InitSpawnEnemyListDropDown();
    }

    public void InitSpawnEnemyListDropDown()
    {
        if (spawnEnemyList == null)
            return;

        if (spawnEnemyList.Count <= 0)
            return;

        spawnEnemyListDropDown.options.Clear();

        for (int i = 0; i < spawnEnemyList[0].spawnEnemyList.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = ((EnemyManager.EnemyName)spawnEnemyList[0].spawnEnemyList[i]).ToString(); 
            spawnEnemyListDropDown.options.Add(option);
        }

        spawnEnemyListDropDown.value = 0;
        if(spawnEnemyList[0].spawnEnemyList.Count == 0)
        {
            spawnEnemyListDropDown.captionText.text = "";
        }
        else
        {
            spawnEnemyListDropDown.captionText.text = spawnEnemyListDropDown.options[0].text;
        }
    }

    public void SetAllInputFieldInitText()  // 입력값 정보 모두 초기화
    {
        waitTimeBeforeSpawnInputField.text = INIT_FLOAT_STRING;
        spawnDelayInputField.text = INIT_FLOAT_STRING;
        spawnCountInputField.text = INIT_INT_STRING;
        spawnEnemyListDropDown.captionText.text = "";

        enemySpawnListHPInputField.text = "";
        enemySpawnListDamageInputField.text = "";
        enemySpawnListAttackDelayInputField.text = "";
        enemySpawnListAttackRangeInputField.text = "";
        enemySpawnListMoveSpeedInputField.text = "";
    }
    public void InitSpawnEnemyDropDownData()
    {
        spawnEnemyListDropDown.value = 0;
        spawnEnemyListDropDown.captionText.text = "";

        enemySpawnListHPInputField.text = "";
        enemySpawnListDamageInputField.text = "";
        enemySpawnListAttackDelayInputField.text = "";
        enemySpawnListAttackRangeInputField.text = "";
        enemySpawnListMoveSpeedInputField.text = "";
    }


    public void UpdateSpawnEnemyListDropDown()  // 웨이브 선택 시 해당 웨이브 정보를 토대로 적리스트 정보 불러와서 ui 동기화
    {
        if (spawnEnemyList == null)
            return;
        if (spawnEnemyList.Count <= 0)
            return;

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

    public void SetInpuFieldDefaultValue(InputField inputField) // 입력값 중 지정한 범위를 벗어나는 값을 입력했을 경우, 기본값 적용 메소드
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
        if (spawnEnemyList == null)
            return;
        if (spawnEnemyList.Count <= 0)
            return;

        if(spawnEnemyList[0].spawnEnemyList.Count == 0)
        {
            waitTimeBeforeSpawnInputField.text = INIT_FLOAT_STRING;
            spawnDelayInputField.text = INIT_FLOAT_STRING;
            spawnCountInputField.text = INIT_INT_STRING;
        }
        else
        {
            waitTimeBeforeSpawnInputField.text = spawnEnemyList[0].waitTimeBeforeSpawnList[0].ToString();
            spawnDelayInputField.text = spawnEnemyList[0].spawnDelayList[0].ToString();
            spawnCountInputField.text = spawnEnemyList[0].spawnCountList[0].ToString();
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
        enemyDamageInputField.text = enemyStaticData.attackDamage.ToString();
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
        enemySpawnListDamageInputField.text = enemyDamageInputField.text;
        enemySpawnListAttackDelayInputField.text = enemyAttackDelayInputField.text;
        enemySpawnListAttackRangeInputField.text = enemyAttackRangeInputField.text;
        enemySpawnListMoveSpeedInputField.text = enemyMoveSpeedInputField.text;
    }

    public void DeleteEnemyToEnemyList()   // - 버튼 누르면 enemyspawnlist에 선택한 enemy 정보 제거
    {
        if (spawnEnemyList == null)
            return;

        if (spawnEnemyListDropDown.options.Count <= 0)
            return;

        int spawnEnemyListDropDownValue = spawnEnemyListDropDown.value;

        spawnEnemyList[waveCountDropDown.value].spawnEnemyList.RemoveAt(spawnEnemyListDropDownValue);
        spawnEnemyList[waveCountDropDown.value].waitTimeBeforeSpawnList.RemoveAt(spawnEnemyListDropDownValue);
        spawnEnemyList[waveCountDropDown.value].spawnDelayList.RemoveAt(spawnEnemyListDropDownValue);
        spawnEnemyList[waveCountDropDown.value].spawnCountList.RemoveAt(spawnEnemyListDropDownValue);

        spawnEnemyListDropDown.options.RemoveAt(spawnEnemyListDropDownValue);

        if(spawnEnemyListDropDown.options.Count == 0)
        {
            InitSpawnEnemyDropDownData();
        }
        else
        {
            spawnEnemyListDropDown.value = spawnEnemyListDropDown.options.Count - 1;
            spawnEnemyListDropDown.captionText.text = MapEditorController.Instance.SelectEnemyIndex.ToString();
            OnValueChangeSpawnEnemyList();
        }
    }

    public void UpdateSpawnEnemyListInfo()   // update 버튼 누르면 spawn enemy list에 설정한 몹과 같이 설정한 스폰정보 수정
    {
        if (spawnEnemyList == null)
            return;

        if (spawnEnemyListDropDown.options.Count <= 0)
            return;

        if (spawnEnemyList[waveCountDropDown.value].spawnEnemyList.Count <= 0)
            return;

        spawnEnemyListDropDown.options[spawnEnemyListDropDown.value].text = MapEditorController.Instance.SelectEnemyIndex.ToString();
        spawnEnemyListDropDown.captionText.text = MapEditorController.Instance.SelectEnemyIndex.ToString();

        spawnEnemyList[waveCountDropDown.value].spawnEnemyList[spawnEnemyListDropDown.value] = ((int)MapEditorController.Instance.SelectEnemyIndex);
        spawnEnemyList[waveCountDropDown.value].waitTimeBeforeSpawnList[spawnEnemyListDropDown.value] = (float.Parse(waitTimeBeforeSpawnInputField.text));
        spawnEnemyList[waveCountDropDown.value].spawnDelayList[spawnEnemyListDropDown.value] = (float.Parse(spawnDelayInputField.text));
        spawnEnemyList[waveCountDropDown.value].spawnCountList[spawnEnemyListDropDown.value] = (int.Parse(spawnCountInputField.text));

        OnValueChangeSpawnEnemyList();
    }

    public void InitSpawnEnemyList()    // init 버튼 누르면 현재 선택한 웨이브에서 소환 적 리스트 초기화
    {
        spawnEnemyList[waveCountDropDown.value].spawnEnemyList.Clear();
        spawnEnemyList[waveCountDropDown.value].waitTimeBeforeSpawnList.Clear();
        spawnEnemyList[waveCountDropDown.value].spawnDelayList.Clear();
        spawnEnemyList[waveCountDropDown.value].spawnCountList.Clear();

        spawnEnemyListDropDown.ClearOptions();

        InitSpawnEnemyDropDownData();
    }

    public void OnValueChangeSpawnEnemyList()   // 선택한 웨이브 몹 정보 출력 업데이트
    {
        waitTimeBeforeSpawnInputField.text = spawnEnemyList[waveCountDropDown.value].waitTimeBeforeSpawnList[spawnEnemyListDropDown.value].ToString();
        spawnDelayInputField.text = spawnEnemyList[waveCountDropDown.value].spawnDelayList[spawnEnemyListDropDown.value].ToString();
        spawnCountInputField.text = spawnEnemyList[waveCountDropDown.value].spawnCountList[spawnEnemyListDropDown.value].ToString();

        EnemyStaticData enemyStaticData = EnemyManager.Instance.GetEnemyStaticData((EnemyManager.EnemyName)spawnEnemyList[waveCountDropDown.value].spawnEnemyList[spawnEnemyListDropDown.value]);

        enemySpawnListHPInputField.text = enemyStaticData.maxHp.ToString();
        enemySpawnListDamageInputField.text = enemyStaticData.attackDamage.ToString();
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
