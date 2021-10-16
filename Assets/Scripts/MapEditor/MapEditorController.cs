using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorController : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2 mapSize;

    void Start()
    {
        GameManager.Instance.LoadMapEditorData();
    }

    public void GenerateMap()
    {
        const string GENERATE_MAP_PARENT_NAME = "Generated Map";

        GameObject generateMapParent = GameObject.Find(GENERATE_MAP_PARENT_NAME);

        if (generateMapParent != null)
        {
            DestroyImmediate(generateMapParent);
        }

        generateMapParent = new GameObject(GENERATE_MAP_PARENT_NAME);

        for (int x = 0; x < mapSize.x; x++) 
        {
            for(int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(x, 0, -y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.parent = generateMapParent.transform;
            }
        }
    }
}
