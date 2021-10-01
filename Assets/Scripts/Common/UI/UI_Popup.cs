using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Canvas canvas = UI_Utill.GetOrAddComponent<Canvas>(gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        UIManager.Instance.AddPopupUI(this, canvas); 
    }

    public virtual void ShowPopupUI()
    {
        UIManager.Instance.ShowPopupUI(gameObject.name);
    }

    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
