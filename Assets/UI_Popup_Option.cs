using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Option : MonoBehaviour
{
    public Canvas OptionCanvas;

    public void OptionSetting()
    {
        OptionCanvas.gameObject.SetActive(true);
    }
}
