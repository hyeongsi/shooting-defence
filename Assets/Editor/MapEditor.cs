using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapEditorController))]
public class MapEditor : Editor
{
    MapEditorController mapEditorController = null;
    private void OnEnable()
    {
        mapEditorController = target as MapEditorController;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Map"))
        {
            if(mapEditorController)
            {
                mapEditorController.GenerateMap();
            }
        }

        GUILayout.Space(20f);

        if (GUILayout.Button("Open Map Editor Window"))
        {
            EditorApplication.ExecuteMenuItem("Window/Map Editor Window");
        }
    }

    private void OnSceneGUI()
    {
       
    }
}

public class HandleGUIScope : GUI.Scope
{
    public HandleGUIScope()
    {
        Handles.BeginGUI();
    }

    protected override void CloseScope()
    {
        Handles.EndGUI();
    }
}
