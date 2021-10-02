﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_Data<T> where T : UI_Base 
{
    public T ui_data;
    public Canvas canvas;

    public UI_Data(T ui_data, Canvas canvas)
    {
        this.ui_data = ui_data;
        this.canvas = canvas;
    }
}

public class UIManager : MonoBehaviour
{
    private int _order = 10;    // 현재까지 최근에 사용한 오더

    private Hashtable uiHashTable = new Hashtable();
    private Stack<UI_Popup> popupStack = new Stack<UI_Popup>();    // 팝업 캔버스
    private UI_Data<UI_Scene> sceneUIData = null;    // 고정 캔버스

    #region Property
    public Hashtable UiHashTable
    {
        get { return uiHashTable; }
    }
    #endregion
    // 씬마다 사용하는 UI 이름 여기다가 enum 값으로 등록시켜 놓고, 해당 씬에서 UI 이름 찾아가서 enum값 string 으로 바꿔서 사용하면 됨
    #region SceneUIEnum
    public enum MapEditPopUpUI
    {
        Build_Canvas,
        Aim_Canvas,
    }
    #endregion
    #region Singleton
    static UIManager instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static UIManager Instance { get { return instance; } }

    #endregion
    public void AddUIToHashTable<T>(T ui_data, Canvas canvas) where T : UI_Base
    {
        UI_Data<T> ui_popdata = new UI_Data<T>(ui_data, canvas);
        uiHashTable.Add(ui_data.gameObject.name, ui_popdata);
    }

    public void AddSceneUI<T>(T ui_scene, Canvas canvas) where T : UI_Scene
    {
        UI_Data<T> ui_scenedata = new UI_Data<T>(ui_scene, canvas);
        uiHashTable.Add(ui_scene.gameObject.name, ui_scenedata);
        canvas.sortingOrder = 0;    // 무조건 첫번째로 출력

        // sceneUIData = ui_scenedata;
    }

    public void ShowPopupUI(string name)
    {
        UI_Data<UI_Popup> ui_popupdata;

        if(uiHashTable.ContainsKey(name))
        {
            ui_popupdata = uiHashTable[name];
            popupStack.Push(ui_popupdata.ui_data);
            ui_popupdata.canvas.sortingOrder = _order;
            _order++;

            ui_popupdata.ui_popup.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Not Found PopupUIName");
        }
    }

    public void ShowSceneUI()
    {
        if(sceneUIData != null)
            sceneUIData.ui_scene.gameObject.SetActive(true);
    }

    public void CloseSceneUI()
    {
        if (sceneUIData != null)
        {
            sceneUIData.ui_scene.gameObject.SetActive(false);
        }
    }

    public void ClosePopupUI(UI_Popup popup) // 안전 차원
    {
        if (popupStack.Count == 0) // 비어있는 스택이라면 삭제 불가
            return;

        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!"); // 스택의 가장 위에있는 Peek() 것만 삭제할 수 잇기 때문에 popup이 Peek()가 아니면 삭제 못함
            return;
        }

        ClosePopupUI();
    }

    private void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UI_Popup popup = popupStack.Pop();
        popup.gameObject.SetActive(false);

        _order--; // order 줄이기
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
            ClosePopupUI();
    }

    public void SwitchPauseUI()
    {
        GameManager.Instance.SwitchIsPause();
        UI_PopupData tempUi_PopupData;

        switch (GameManager.Instance.PlayeState)
        {
            case GameManager.PlayStates.MAIN_MENU:
                return;
            case GameManager.PlayStates.SINGLE_PLAY:
            case GameManager.PlayStates.MULTY_PLAY:
                // 인게임 씬에서의 esc 메뉴 활/비활성화
                return;
            case GameManager.PlayStates.MAP_EDIT:
                uiPopupDictionary.TryGetValue(MapEditPopUpUI.Build_Canvas.ToString(), out tempUi_PopupData);
                if (tempUi_PopupData == null)
                    return;

                if(GameManager.Instance.IsPause)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    tempUi_PopupData.ui_popup.ShowPopupUI();
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    tempUi_PopupData.ui_popup.ClosePopupUI();
                }
                return;
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPauseUI();
        }
    }
}