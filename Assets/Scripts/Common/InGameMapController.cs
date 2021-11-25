using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMapController : MonoBehaviour
{
    MapEditorController.CustomTileMap customTileMap = new MapEditorController.CustomTileMap();

    void Start()
    {
        BlockManager.Instance.LoadAll();
        EnemyManager.Instance.LoadAll();
        ObjManager.Instance.LoadAll();

        LoadCustomMap("test4");
    }

    public void LoadCustomMap(string mapName)
    {
        TextAsset text = Resources.Load(mapName) as TextAsset;
        customTileMap = JsonUtility.FromJson<MapEditorController.CustomTileMap>(text.ToString());

        customTileMap.CreateCustomMap();
    }

}
