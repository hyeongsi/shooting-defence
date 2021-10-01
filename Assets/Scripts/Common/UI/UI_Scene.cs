using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
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

        UIManager.Instance.AddSceneUI(this, canvas);
    }

    public virtual void ShowSceneUI()
    {
        UIManager.Instance.ShowSceneUI();
    }
}
