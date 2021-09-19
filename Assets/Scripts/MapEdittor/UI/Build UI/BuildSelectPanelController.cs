using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSelectPanelController : MonoBehaviour
{
    private int buttonCount = 0;
    private int childCount = 0;
    private RectTransform rectTransform;
    private RectTransform[] childRectTransform = null;
    private Vector2 setSizeVector = new Vector2(120,120);
    private const int HORIZONTAL_PANEL_COUNT = 4;
    private const int PADDING_SIZE = 20;
    private const int BUTTON_SIZE = 60;

    private void SetRectTransform(int buttonCount)
    {
        float y = (buttonCount-1) / HORIZONTAL_PANEL_COUNT + 1;

        setSizeVector.y = y * BUTTON_SIZE + PADDING_SIZE + (y-1) * PADDING_SIZE;
        if (buttonCount > HORIZONTAL_PANEL_COUNT)   // x 개수가 지정 가로 개수보다 많다면, 최대 가로 개수 크기 지정
        {
            setSizeVector.x = BUTTON_SIZE * HORIZONTAL_PANEL_COUNT + PADDING_SIZE + (HORIZONTAL_PANEL_COUNT-1) * PADDING_SIZE;
        }
        else   // 지정 가로 개수보다 작다면 개수만큼 크기 지정
        {
            setSizeVector.x = BUTTON_SIZE * buttonCount + buttonCount * PADDING_SIZE;
        }

        rectTransform.sizeDelta = setSizeVector;
        gameObject.SetActive(true);
    }

    private void SetChildRectTransform(int buttonNumber)
    {
        for(int i = 0; i < childCount; i ++)
        {
            childRectTransform[i].gameObject.SetActive(false);
        }

        childRectTransform[buttonNumber].gameObject.SetActive(true);
        setSizeVector.x = rectTransform.sizeDelta.x - PADDING_SIZE;
        setSizeVector.y = rectTransform.sizeDelta.y - PADDING_SIZE;

        childRectTransform[buttonNumber].sizeDelta = setSizeVector;
    }

    public void OpenPanel(int buttonNumber)
    {
        if (childCount < buttonNumber - 1 && buttonNumber < 0)
        {
            Debug.Log("OpenPanel(), buttonNumber가 childCount를 벗어났습니다.");
            return;
        }

        if (childRectTransform[buttonNumber].gameObject.activeSelf == true)
        {
            childRectTransform[buttonNumber].gameObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }
            
        buttonCount = transform.GetChild(buttonNumber).childCount;
        SetRectTransform(buttonCount);
        SetChildRectTransform(buttonNumber);
    }

    private void Start()
    {
        childCount = transform.childCount;

        rectTransform = transform.GetComponent<RectTransform>();
        childRectTransform = new RectTransform[childCount];

        for(int i = 0; i < childCount; i ++)
        {
            childRectTransform[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }

        // 기본 세팅 초기화 후 UI 비활성화 처리
        gameObject.SetActive(false);
    }
}
