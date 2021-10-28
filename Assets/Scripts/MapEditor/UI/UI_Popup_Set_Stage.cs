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

    public override void Init()
    {
        base.Init();
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
            option.text = (i + 1).ToString();
            spawnEnemyListDropDown.options.Add(option);
        }
    }

    private void OnEnable()
    {
        InitEditWaveDropDown();
    }

    public enum UpdateEnemyList
    {
        ADD_ENEMYLIST = 1,
        DELETE_ENEMYLIST = 2,
        CLEAR_ENEMYLIST = 3,
    }
}
