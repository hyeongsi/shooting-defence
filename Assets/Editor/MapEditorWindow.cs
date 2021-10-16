using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow
{
    static MapEditorWindow mapEditorWindow;
    private Vector2 scrollPosition;

    private const string MAP_EDITOR_NAME = "MapEditor";
    private const string GENERATE_MAP_NAME = "Generated Map";

    private GameObject mapEditorGameObject = null;
    private GameObject generatedMapGameObject = null;
    private Vector2 mapSize;

    private int selectBlockIndex = -1;
    private int[,] cell;
    private Transform[,] cellGameObjectTransform;
    private string[] blockNames;

    [MenuItem("Window/Map Editor Window")]
    private static void Init()
    {
        mapEditorWindow = GetWindow<MapEditorWindow>();
        mapEditorWindow.InitMap();
    }

    private void InitMap()
    {
        blockNames = new string[System.Enum.GetValues(typeof(BlockManager.BlockName)).Length];

        for(int i = 0; i < blockNames.Length; i ++)
        {
            blockNames[i] = System.Enum.GetValues(typeof(BlockManager.BlockName)).GetValue(i).ToString();
        }

        LoadMapSize();
        BlockManager.Instance.LoadAll();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Load Map Size"))
        {
            LoadMapSize();
            BlockManager.Instance.LoadAll();
        }

        GUILayout.Space(20f);
        selectBlockIndex = EditorGUILayout.Popup(selectBlockIndex, blockNames);
        GUILayout.Space(20f);

        using (var scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition))
        {
            scrollPosition = scrollViewScope.scrollPosition;

            for (int x = 0; x < mapSize.x; x++)
            {
                GUILayout.BeginHorizontal();
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (GUILayout.Button(GetCellString(x, y), GUILayout.Width(60), GUILayout.Height(60)) && cell[y, x] == 0)
                    {
                        OnCellButton(x, y);
                    }
                }
                GUILayout.EndHorizontal();
            }
        } 
    }

    private void OnCellButton(int x, int y)
    {
        if (selectBlockIndex == -1)
            return;

        cell[y, x] = selectBlockIndex;
        
        GameObject blockGameObject = BlockManager.Instance.GetBlock((BlockManager.BlockName)selectBlockIndex);
        if (blockGameObject == null)
            return;

        cellGameObjectTransform[y,x] = Instantiate(blockGameObject).transform;
        cellGameObjectTransform[y, x].parent = generatedMapGameObject.transform;
    }

    private string GetCellString(int x, int y)
    {
        return x.ToString() + " ," + y.ToString() + " ( " + cell[y, x] + " )";
    }

    private void LoadMapSize()
    {
        if (mapEditorGameObject == null)
        {
            mapEditorGameObject = GameObject.Find(MAP_EDITOR_NAME);
            generatedMapGameObject = GameObject.Find(GENERATE_MAP_NAME);
        }
        else
        {
            mapSize = mapEditorGameObject.GetComponent<MapEditorController>().mapSize;
            cell = new int[(int)mapSize.y, (int)mapSize.x];
            cellGameObjectTransform = new Transform[(int)mapSize.y, (int)mapSize.x];
            return;
        }

        mapSize = Vector2.zero;
    }
}
