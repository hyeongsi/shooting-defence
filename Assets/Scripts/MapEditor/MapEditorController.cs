using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorController : MonoBehaviour
{
    public Camera mainCamera;

    public Transform tilePrefab;
    public Vector2 mapSize;

    void Start()
    {
        GameManager.Instance.LoadMapEditorData();
    }

    public void GenerateMap()
    {
        const string GENERATE_MAP_PARENT_NAME = "Generated Map";
        const float HALF = 0.5f;
        const int INITIAL_Y_POSITION = 1;
        const int Y_INCREMENT = 1;

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
                Vector3 tilePosition = new Vector3(x, 0, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.parent = generateMapParent.transform;
            }
        }

        int cameraYPosition = INITIAL_Y_POSITION;

        if(mapSize.x > mapSize.y)
        {
            cameraYPosition += ((int)mapSize.x * Y_INCREMENT);
        }
        else if(mapSize.x <= mapSize.y)
        {
            cameraYPosition += ((int)mapSize.y * Y_INCREMENT);
        }

        if (cameraYPosition < INITIAL_Y_POSITION)
            cameraYPosition = INITIAL_Y_POSITION;

        mainCamera.transform.position = new Vector3(mapSize.x * HALF, cameraYPosition, mapSize.y * HALF);
    }
}
