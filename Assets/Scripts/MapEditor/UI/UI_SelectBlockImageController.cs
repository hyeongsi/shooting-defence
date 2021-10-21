using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectBlockImageController : MonoBehaviour
{
    public RectTransform selectedImageRectTransform;
    public GameObject contentGameObject;

    private List<Button> contentChildButtonList = new List<Button>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (MapEditorController.Instance == null)
        {
            Debug.Log("MapEditorController is null");
            return;
        }

        for (int i = 0; i < contentGameObject.transform.childCount; i ++)
        {
            int _i = i;
            contentChildButtonList.Add(contentGameObject.transform.GetChild(i).GetComponent<Button>());
            contentChildButtonList[i].onClick.AddListener(delegate() {
                MapEditorController.Instance.SetSelectBlockIndex(_i);
                FixedSelectImageLocation();
            });
        }
    }
    
    public void FixedSelectImageLocation()
    {
        if (MapEditorController.Instance == null)
        {
            Debug.Log("MapEditorController is null");
            return;
        }
            

        selectedImageRectTransform.SetParent(contentChildButtonList[(int)MapEditorController.Instance.SelectBlockIndex].transform);
        selectedImageRectTransform.anchoredPosition = Vector2.zero;
    }
}
