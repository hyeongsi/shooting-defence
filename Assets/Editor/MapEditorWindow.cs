using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow
{
    private bool paintMode = false;
    private Vector2 cellSize = new Vector2(2f, 2f);

    [MenuItem("Window/Map Editor Window")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MapEditorWindow));
    }

    private void OnGUI()    // UI 추가
    {
        paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));
    }

    private void OnSceneGUI(SceneView sceneView)    // 장면 보기 도우미 표시
    {
        if(paintMode)
        {
            DisplayVisualHelp();
        }
    }

    private void DisplayVisualHelp()
    {
        //Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(Event.current.mousePosition);
        //Debug.Log(ray.origin);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Event.current.mousePosition);

        Debug.Log(worldPos);
    }

    private void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    private void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }
}
