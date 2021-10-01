using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditUIController : MonoBehaviour
{
    private bool isActiveEditUi = false;

    void SwitchEditUIActive()
    {
        isActiveEditUi = !isActiveEditUi;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(isActiveEditUi);

        if (isActiveEditUi)
            GameManager.Instance.PauseGame();
        else
            GameManager.Instance.ContinueGame();

        if (isActiveEditUi)
        {
            Transform buildMenuUI = transform.Find("Build Menu UI");

            if (buildMenuUI == null)
                return;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Transform selectPanel = buildMenuUI.GetChild(0);
            selectPanel.gameObject.SetActive(false);

            for(int i = 0; i < selectPanel.childCount; i++)
            {
                selectPanel.GetChild(i).gameObject.SetActive(false);
            }
        } 
        else
        {
            SwitchLockCursor();
        }
    }

    private void SwitchLockCursor()
    {
        if(Cursor.visible == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Start()
    {
        SwitchLockCursor();

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SwitchEditUIActive();
        //}
    }
}
