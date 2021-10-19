using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectBlockImageController : MonoBehaviour
{
    private Image currentSelectImage = null;
    private List<Image> childSelectImageList = new List<Image>();

    public GameObject contentGameObject;    // 콘텐츠들을 담고 있는 오브젝트

    private void Start()
    {
        InitSelectBlockImageComponent();
        gameObject.SetActive(false);
    }

    private void InitSelectBlockImageComponent()
    {
        for( int i = 0; i < contentGameObject.transform.childCount; i ++)
        {
            childSelectImageList.Add(contentGameObject.transform.GetChild(i).GetComponent<Image>());
        }
    }
    
    public void ChangeSelectedBlockButtonColor(UI_Scene_MapEditor mapEditor)
    {
        
    }
}
