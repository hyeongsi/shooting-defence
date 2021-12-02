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
        StartCoroutine(LoadCustomMap(GameManager.Instance.stageName));
    }

    public IEnumerator LoadCustomMap(string mapName)
    {
        while(!(BlockManager.Instance.isLoadAll && EnemyManager.Instance.isLoadAll  && EnemyManager.Instance.isLoadStaticData && ObjManager.Instance.isLoadAll))
        {
            yield return null;
        }

        if(mapName == null)
        {
            mapName = "test3";
        }
        TextAsset text = Resources.Load(mapName) as TextAsset;
        customTileMap = JsonUtility.FromJson<MapEditorController.CustomTileMap>(text.ToString());

        customTileMap.CreateCustomMap();
        cc.enabled = false;
        player.transform.position = customTileMap.spawnPosition.placeGameTransform;
        cc.enabled = true;

        CreateCheckPointChildObject();  // 체크포인트 위치 맵 정보에 따라 새로 지정
        checkPoint.SetCheckPoint();     // 체크포인트 위치 등록함

        enemySpawner.Init(customTileMap.spawnEnemyPosition.placeGameTransform, customTileMap.spawnEnemyInfoList);
        yield return null;
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
                DestroyImmediate(checkPointChildTransform[i].gameObject);
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
            StartCoroutine(LoadCustomMap("test3"));
        }else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            enemySpawner.StartStage(0);
        }
    }

}
