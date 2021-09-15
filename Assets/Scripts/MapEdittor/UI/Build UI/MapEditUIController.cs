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
            transform.Find("Build Menu UI")?.GetChild(0).gameObject.SetActive(false);
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
