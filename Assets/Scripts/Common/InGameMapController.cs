using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class InGameMapController : MonoBehaviour
{
    public GameObject player;
    public CharacterController cc;
    MapEditorController.CustomTileMap customTileMap = new MapEditorController.CustomTileMap();

    public CheckPoint checkPoint;   // 적 체크포인트 설정 부모 오브젝트
    public EnemySpawner enemySpawner;   // 적 스포너

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
        cc.enabled = false;
        player.transform.position = customTileMap.spawnPosition.placeGameTransform;
        cc.enabled = true;

        CreateCheckPointChildObject();  // 체크포인트 위치 맵 정보에 따라 새로 지정
        checkPoint.SetCheckPoint();     // 체크포인트 위치 등록함

        enemySpawner.Init(customTileMap.spawnEnemyPosition.placeGameTransform, customTileMap.spawnEnemyInfoList);
    }   

    public void CreateCheckPointChildObject()
    {
        if (checkPoint == null)
            return;

        // 체크포인트 자식 삭제
        Transform[] checkPointChildTransform = new Transform[checkPoint.transform.childCount];
        if (checkPointChildTransform.Length > 0)
        {
            for (int i = 0; i < checkPoint.transform.childCount; i++)
            {
                checkPointChildTransform[i] = checkPoint.transform.GetChild(i);
            }

            int childCount = checkPoint.transform.childCount;
            for(int i = 0; i < childCount; i++)
            {
                Destroy(checkPointChildTransform[i].gameObject);
            }
        }

        // 체크포인트 자식 등록
        for(int i = 0; i < customTileMap.enemyGuideLineList.Count; i++)
        {
            GameObject newCheckPoint = new GameObject();
            newCheckPoint.transform.position = customTileMap.enemyGuideLineList[i].placeGameTransform;
            newCheckPoint.transform.parent = checkPoint.transform;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            LoadCustomMap("test4");
        }else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            enemySpawner.StartStage(0);
        }
    }

}
