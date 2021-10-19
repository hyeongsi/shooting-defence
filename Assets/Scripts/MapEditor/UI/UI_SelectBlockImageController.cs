using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectBlockImageController : MonoBehaviour
{
    public RectTransform selectedImageRectTransform;
    public GameObject contentGameObject;
    [Space(10)]
    public UI_Scene_MapEditor mapEditor;

    private List<Button> contentChildButtonList = new List<Button>();

    private void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

    private void Init()
    {
        for (int i = 0; i < contentGameObject.transform.childCount; i ++)
        {
            int _i = i;
            contentChildButtonList.Add(contentGameObject.transform.GetChild(i).GetComponent<Button>());
            contentChildButtonList[i].onClick.AddListener(delegate() {
                mapEditor.SetSelectBlockIndex(_i);
                FixedSelectImageLocation();
            });
        }
    }
    
    public void FixedSelectImageLocation()
    {
        Debug.Log(mapEditor.SelectBlockIndex);
        selectedImageRectTransform.SetParent(contentChildButtonList[(int)mapEditor.SelectBlockIndex].transform);
        selectedImageRectTransform.anchoredPosition = Vector2.zero;
    }
}
