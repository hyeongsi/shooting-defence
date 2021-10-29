using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_PopupData
{
    public UI_Popup ui_popup;
    public Canvas canvas;

    public UI_PopupData(UI_Popup ui_popup, Canvas canvas)
    {
        this.ui_popup = ui_popup;
        this.canvas = canvas;
    }
}

public class UI_SceneData
{
    public UI_Scene ui_scene;
    public Canvas canvas;

    public UI_SceneData(UI_Scene ui_scene, Canvas canvas)
    {
        this.ui_scene = ui_scene;
        this.canvas = canvas;
    }
}

public class UIManager : Singleton<UIManager>
{
    private int _order = 10;    // 현재까지 최근에 사용한 오더

    private Dictionary<string, UI_PopupData> uiPopupDictionary = new Dictionary<string, UI_PopupData>();
    private List<UI_Popup> popupList = new List<UI_Popup>();    // 팝업 캔버스
    private UI_SceneData sceneUIData = null;    // 고정 캔버스

    private const string canvasParentName = "Canvas_Collection";

    #region Property
    public Dictionary<string, UI_PopupData> UiPopupDictionary
    {
        get { return uiPopupDictionary; }
    }
    public UI_SceneData SceneUIData { get { return sceneUIData; } }
    #endregion
    // 씬마다 사용하는 UI 이름 여기다가 enum 값으로 등록시켜 놓고, 해당 씬에서 UI 이름 찾아가서 enum값 string 으로 바꿔서 사용하면 됨
    #region SceneUIEnum
    public enum PopUpUIEnums
    {
        MainMenuPopUpUI,
        MapEditPopUpUI,
    }

    public enum MainMenuPopUpUI
    {
        SelectStage_Canvas,
    }

    public enum MapEditPopUpUI
    {
        Create_Base_Map_Canvas,
        Set_Stage_Info_Canvas,
        Set_Stage_Wave_Info_Canvas,
    }
    #endregion

    public void AddPopupUI(UI_Popup ui_popup, Canvas canvas)
    {
        UI_PopupData ui_popdata = new UI_PopupData(ui_popup, canvas);
        uiPopupDictionary.Add(ui_popup.gameObject.name, ui_popdata);
    }

    public void AddSceneUI(UI_Scene ui_scene, Canvas canvas)
    {
        UI_SceneData ui_scenedata = new UI_SceneData(ui_scene, canvas);
        canvas.sortingOrder = 0;    // 무조건 첫번째로 출력

        sceneUIData = ui_scenedata;
    }

    public void ShowPopupUI(string name)
    {
        UI_PopupData ui_popupdata;

        if(uiPopupDictionary.TryGetValue(name, out ui_popupdata))
        {
            popupList.Add(ui_popupdata.ui_popup);
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

    public void ClosePopupUI(Canvas canvas)
    {
        if (canvas == false)
            return;

        UI_PopupData ui_popupdata;
        if (uiPopupDictionary.TryGetValue(canvas.transform.name, out ui_popupdata))
        {
            for (int i = 0; i < popupList.Count; i++)
            {
                if (popupList[i] != ui_popupdata.ui_popup)
                    continue;

                popupList[i].ClosePopupUI();        // 만약 활성화 된 POPUP UI 라면 비활성화
                return;
            }
        }
    }

    public void ClosePopupUI(UI_Popup popup) // 안전 차원
    {
        if (popupList.Count == 0) // 비어있는 스택이라면 삭제 불가
            return;

        int i;
        for(i = 0;  i < popupList.Count; i ++)
        {
            if(popupList[i] == popup)
            {
                ClosePopupUI(i);
                return;
            }
        } 
    }

    private void ClosePopupUI(int index)
    {
        if (popupList.Count == 0)
            return;
        if (popupList.Count <= index || index < 0)
            return;

        UI_Popup popup = popupList[index];
        popup.gameObject.SetActive(false);
        popupList.RemoveAt(index);

        _order--; // order 줄이기
    }

    private void ClosePopupUI() // 최상위 popup 닫기
    {
        if (popupList.Count == 0)
            return;

        UI_Popup popup = popupList[0];
        popup.gameObject.SetActive(false);
        popupList.RemoveAt(0);

        _order--; // order 줄이기
    }

    public void CloseAllPopupUI()
    {
        while (popupList.Count > 0)
            ClosePopupUI();
    }

    public void SwitchPauseUI()
    {
        GameManager.Instance.SwitchIsPause();
        //UI_PopupData tempUi_PopupData;

        switch (GameManager.Instance.PlayeState)
        {
            case GameManager.PlayStates.MAIN_MENU:
                return;
            case GameManager.PlayStates.IN_GAME:
                // 인게임 씬에서의 esc 메뉴 활/비활성화
                return;
            //case GameManager.PlayStates.MAP_EDIT:
            //    uiPopupDictionary.TryGetValue(MapEditPopUpUI.Build_Canvas.ToString(), out tempUi_PopupData);
            //    if (tempUi_PopupData == null)
            //        return;

            //    if (GameManager.Instance.IsPause)
            //        tempUi_PopupData.ui_popup.ShowPopupUI();
            //    else
            //        tempUi_PopupData.ui_popup.ClosePopupUI();

            //    if (sceneUIData.ui_scene == default)
            //        return;

            //    if (sceneUIData.ui_scene is UI_Scene_EditAimUI)
            //    {
            //        UI_Scene_EditAimUI aimUI = sceneUIData.ui_scene as UI_Scene_EditAimUI;
            //        aimUI.SwitchCursorLockState();
            //    }
            //    return;
        }
    }

    public void ActiveCanvasUI(Canvas canvas)
    {
        UI_PopupData ui_popupdata;
        if (uiPopupDictionary.TryGetValue(canvas.transform.name, out ui_popupdata))
        {
            for (int i = 0; i < popupList.Count; i++)
            {
                if (popupList[i] != ui_popupdata.ui_popup)
                    continue;

                popupList[i].ClosePopupUI();        // 만약 활성화 된 POPUP UI 라면 비활성화
                break;
            }
        }

        ShowPopupUI(canvas.transform.name);
    }

    public void SwitchPopUpUIActivation(Canvas canvas)  // popui 전용 ui switch
    {
        if (canvas == false)
            return;

        UI_PopupData ui_popupdata;
        if (uiPopupDictionary.TryGetValue(canvas.transform.name, out ui_popupdata))
        {
            for (int i = 0; i < popupList.Count; i++)
            {
                if (popupList[i] != ui_popupdata.ui_popup)
                    continue;

                popupList[i].ClosePopupUI();        // 만약 활성화 된 POPUP UI 라면 비활성화
                return;
            }
        }

        for(int i = 0; i < popupList.Count; i ++)
        {
            popupList[i].ClosePopupUI();
        }

        ShowPopupUI(canvas.transform.name);
    }

    public void EnrollUI(PopUpUIEnums popUpUIEnums) // ui에 등록된 자식들을 찾아서 on,off 해서 UIManager에 등록 시키도록 하는 메소드
    {
        Array array;

        switch (popUpUIEnums)
        {
            case PopUpUIEnums.MainMenuPopUpUI:
                array = Enum.GetValues(typeof(MainMenuPopUpUI));
                break;
            case PopUpUIEnums.MapEditPopUpUI:
                array = Enum.GetValues(typeof(MapEditPopUpUI));
                break;
            default:
                return;
        }

        if (array.Length == 0)
            return;

        Transform findTransformParent;
        Transform findTransform;
        for(int i = 0; i < array.Length; i ++)
        {
            // 모든 Canvas들은 Canvas_Collection 이름을 가진 오브젝트 안에 들어가 있으며, 이것들 통해 비활성화 된 오브젝트를 찾아 start()를 실행

            findTransformParent = GameObject.Find(canvasParentName).transform;
            if (findTransformParent == false)
                return;

            for (int j = 0; j < findTransformParent.childCount; j++)
            {
                findTransform = findTransformParent.GetChild(j);

                if (findTransform.name != array.GetValue(i).ToString())
                    continue;

                if (findTransform.gameObject.activeSelf == true)
                    continue;

                StartCoroutine(GameObjectInitCoroutine(findTransform));
                break;
            } 
        }
    }

    private IEnumerator GameObjectInitCoroutine(Transform transform)
    {
        // start() 는 활성화 된 후 1프레임 후에 실행되기 때문에 조금 기다리고 비활성화 처리
        // 1프레임 쉬면서 start() / Init() 끝난 후 비활성화 처리
        transform.gameObject.SetActive(true);
        yield return null;
        transform.gameObject.SetActive(false);
    }

    public void ToggleCanvas(Canvas canvas)
    {
        if(canvas.gameObject.activeSelf == true)
        {
            canvas.gameObject.SetActive(false);
        }
        else
        {
            canvas.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameManager.Instance == false)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPauseUI();
        }
    }
}
