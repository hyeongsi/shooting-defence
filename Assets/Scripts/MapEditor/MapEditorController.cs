using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapEditorController : MonoBehaviour
{
    public AssetReference tilePrefab;
    public Vector2 mapSize;

    void Start()
    {
        GameManager.Instance.LoadMapEditorData();
        GenerateMap();
    }

    public void GenerateMap()
    {
        //string generateMapParentName = "Generated Map";
        //Transform generateMapParent = transform.Find(generateMapParentName);
        //if(generateMapParent == null)
        //{
        //    GameObject newGenerateMapParent = new GameObject();
        //    newGenerateMapParent.transform.parent = 
        //}


        for (int x = 0; x < mapSize.x; x++) 
        {
            for(int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
                tilePrefab.InstantiateAsync(tilePosition, Quaternion.identity).Completed +=
                    (AsyncOperationHandle<GameObject> obj) =>
                    {
                        GameObject newTile = obj.Result;
                    };
            }
        }
    }
}
