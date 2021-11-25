using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class InGameMapController : MonoBehaviour
{
    //public GameObject player;
    public Transform playerPrefab;
    public CinemachineVirtualCamera cvm;
    MapEditorController.CustomTileMap customTileMap = new MapEditorController.CustomTileMap();

    void Start()
    {
        BlockManager.Instance.LoadAll();
        EnemyManager.Instance.LoadAll();
        ObjManager.Instance.LoadAll();
    }

    public void LoadCustomMap(string mapName)
    {
        TextAsset text = Resources.Load(mapName) as TextAsset;
        customTileMap = JsonUtility.FromJson<MapEditorController.CustomTileMap>(text.ToString());

        // 체크포인트 등록, (적 이동 경로)
        // 적 스폰지점
        customTileMap.CreateCustomMap();

        Transform newPlayer = Instantiate(playerPrefab, customTileMap.spawnPosition.placeGameTransform, Quaternion.identity);

        cvm.Follow = newPlayer;
        cvm.LookAt = newPlayer;
        //player.transform.position = customTileMap.spawnPosition.placeGameTransform;
    }

}
