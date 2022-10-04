using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public int lifeLeft = 5;
    public Text lifeText;
    public GameObject screenUI;
    private GameObject clearUI = null;
    private GameObject failedUI = null;


    public void Start()
    {
        clearUI = screenUI.transform.Find("Clear").gameObject;
        failedUI = screenUI.transform.Find("Fail").gameObject;
    }

    public void LifeEarn()
    {
        lifeLeft++;
        lifeText.text = lifeLeft.ToString();
    }

    public void LifeLoss()
    {
        lifeLeft--;
        lifeText.text = lifeLeft.ToString();

        if (lifeLeft <= 0)
        {
            OnFailedUI();
        }
    }

    public void OnClearUI()
    {
        Cursor.visible = true;
        clearUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnFailedUI()
    {
        Cursor.visible = true;
        failedUI.SetActive(true);
        Time.timeScale = 0;
    }
}
