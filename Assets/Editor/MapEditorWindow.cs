using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow
{
    const string MAP_EDITOR_NAME = "MapEditor";
    GameObject mapEditorGameObject;
    Vector2 mapSize;

    int[,] Cell;
    string[] blockNames;

    [MenuItem("Window/Map Editor Window")]
    private static void Init()
    {
        MapEditorWindow mapEditorWindow = GetWindow<MapEditorWindow>();
        mapEditorWindow.InitMap();
    }

    private void InitMap()
    {
        LoadMapSize();
  
        blockNames = new string[System.Enum.GetValues(typeof(BlockManager.BlockName)).Length];

        for(int i = 0; i < blockNames.Length; i ++)
        {
            blockNames[i] = System.Enum.GetValues(typeof(BlockManager.BlockName)).GetValue(i).ToString();
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Load Map Size"))
        {
            LoadMapSize();
        }

        for (int x = 0; x < mapSize.x; x ++)
        {
            GUILayout.BeginHorizontal();
            for (int y = 0; y < mapSize.y; y++)
            {
                if(GUILayout.Button(GetCellString(x,y), GUILayout.Width(60), GUILayout.Height(60)) && Cell[y, x] == 0)
                {

                }
            }
            GUILayout.EndHorizontal();
        }
    }

    private string GetCellString(int x, int y)
    {
        return " ";
    }

    private void LoadMapSize()
    {
        if (mapEditorGameObject == null)
        {
            mapEditorGameObject = GameObject.Find(MAP_EDITOR_NAME);
        }

        if (mapEditorGameObject != null)
        {
            mapSize = mapEditorGameObject.GetComponent<MapEditorController>().mapSize;
            Cell = new int[(int)mapSize.y, (int)mapSize.x];
        }
        else
        {
            mapSize.x = 0;
            mapSize.y = 0;
        }
    }
}
