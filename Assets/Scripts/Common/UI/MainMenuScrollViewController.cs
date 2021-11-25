using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScrollViewController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Button exit;
    public Button left;
    public Button right;

    const int CONTENT_SIZE = 2;
    float[] pos = new float[CONTENT_SIZE];
    float distance;
    float targetPos;
    bool isDrag;

    void Start()
    {
        distance = 1.0f / (CONTENT_SIZE - 1);
        for (int i = 0; i < CONTENT_SIZE; i++)
        {
            pos[i] = distance * i;
        }
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        targetPos = scrollbar.value = 0.0f;
        exit.gameObject.SetActive(true);
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;

        SetActiveFalseBtn();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < CONTENT_SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetPos = pos[i];
            }
        }

        isDrag = false;
        SetActiveBtn();
    }

    void Update()
    {
        if(isDrag == false)
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
    }

    public void SetActiveFalseBtn()
    {
        exit.gameObject.SetActive(false);
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
    }

    public void SetActiveBtn()
    {
        exit.gameObject.SetActive(true);

        if (targetPos <= 0.01f)
        {
            right.gameObject.SetActive(true);
        }
        else if (targetPos >= 0.99f)
        {
            left.gameObject.SetActive(true);
        }
        else
        {
            left.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
        }
    }

    public void LeftArrow()
    {
        for (int i = CONTENT_SIZE-1; i >= 1; i--)
        {
            if (targetPos == pos[i])
            {
                targetPos = pos[i - 1];
            }
        }

        SetActiveFalseBtn();
        SetActiveBtn();
    }

    public void RightArrow()
    {
        for(int i = 0; i < CONTENT_SIZE-1; i ++)
        {
            if(targetPos == pos[i])
            {
                targetPos = pos[i + 1];
            }
        }

        SetActiveFalseBtn();
        SetActiveBtn();
    }

}
