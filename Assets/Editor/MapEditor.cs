using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapEditorController))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapEditorController mapEditorController = target as MapEditorController;

        if (GUILayout.Button("Create Map"))
        {
            if(mapEditorController)
            {
                mapEditorController.GenerateMap();
            }
        }
    }
}
