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
    }

    private void OnSceneGUI()
    {
        using (new HandleGUIScope())
        {
            GUILayout.Button("Button", GUILayout.Width(50));
        }
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
