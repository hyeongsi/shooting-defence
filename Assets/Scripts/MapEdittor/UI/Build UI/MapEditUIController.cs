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

        GameManager.Instance.IsStop = isActiveEditUi;

        if (isActiveEditUi)
        {
            Transform buildMenuUI = transform.Find("Build Menu UI");

            if (buildMenuUI == null)
                return;

            Transform selectPanel = buildMenuUI.GetChild(0);
            selectPanel.gameObject.SetActive(false);

            for(int i = 0; i < selectPanel.childCount; i++)
            {
                selectPanel.GetChild(i).gameObject.SetActive(false);
            }
        }  
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchEditUIActive();
        }
    }
}
