using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapSize
{
    SMALL = 0,
    MEDIUM = 1,
    LARGE = 2,
}

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2 mapSize;

    [Range(0,1)]
    public float outlinePercent;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        string holderName = "Generated Map";
        if(transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

       
        for(int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, mapSize.y / 2 + 0.5f - y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }    
        }
    }

    public void SetMapSize(int i)
    {
        switch(i)
        {
            case (int)MapSize.SMALL:
                mapSize = new Vector2(10, 10);
                break;
            case (int)MapSize.MEDIUM:
                mapSize = new Vector2(20, 15);
                break;
            case (int)MapSize.LARGE:
                mapSize = new Vector2(30, 20);
                break;
            default:
                return;
        }

        GenerateMap();
        MapManager.Instance.InitMapData(mapSize);
    }
}
