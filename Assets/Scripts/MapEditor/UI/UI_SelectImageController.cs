using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectImageController : MonoBehaviour
{
    public SelectCanvasKind selectCanvasKind = SelectCanvasKind.BLOCK_SELECT_CANVAS;

    public RectTransform selectedImageRectTransform;
    public GameObject contentGameObject;

    private List<Button> contentChildButtonList = new List<Button>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!MapEditorController.Instance)
        {
            Debug.Log("MapEditorController is null");
            return;
        }

        for (int i = 0; i < contentGameObject.transform.childCount; i ++)
        {
            int _i = i;
            contentChildButtonList.Add(contentGameObject.transform.GetChild(i).GetComponent<Button>());

            switch (selectCanvasKind)
            {
                case SelectCanvasKind.BLOCK_SELECT_CANVAS:
                    contentChildButtonList[i].onClick.AddListener(delegate () {
                        MapEditorController.Instance.SetSelectBlockIndex(_i);
                        FixedSelectImageLocation();
                    });
                    break;
                case SelectCanvasKind.ENEMY_SELECT_CANVAS:
                    contentChildButtonList[i].onClick.AddListener(delegate () {
                        MapEditorController.Instance.SetSelectEnemyIndex(_i);
                        FixedSelectImageLocation();
                    });
                    break;
                default:
                    return;
            } 
        }
    }

    public void FixedSelectImageLocation()
    {
        if (!MapEditorController.Instance)
        {
            Debug.Log("MapEditorController is null");
            return;
        }

        switch (selectCanvasKind)
        {
            case SelectCanvasKind.BLOCK_SELECT_CANVAS:
                selectedImageRectTransform.SetParent(contentChildButtonList[(int)MapEditorController.Instance.SelectBlockIndex].transform);
                break;
            case SelectCanvasKind.ENEMY_SELECT_CANVAS:
                selectedImageRectTransform.SetParent(contentChildButtonList[(int)MapEditorController.Instance.SelectEnemyIndex].transform);
                break;
            default:
                return;
        }

        selectedImageRectTransform.anchoredPosition = Vector2.zero;
    }

    public enum SelectCanvasKind
    {
        BLOCK_SELECT_CANVAS = 0,
        ENEMY_SELECT_CANVAS = 1,
    }
}
