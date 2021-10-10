using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_SelectStage : UI_Popup
{
    public override void Init()
    {
        base.Init();
        gameObject.SetActive(false);
    }

    public void StartStage()
    {
        GameManager.Instance.LoadScene(GameManager.PlayStates.IN_GAME);
    }
}
