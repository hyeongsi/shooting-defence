﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;

public class InGameMapController : MonoBehaviour
{
    public GameObject player;
    public CharacterController cc;
    MapEditorController.CustomTileMap customTileMap = new MapEditorController.CustomTileMap();

    public CheckPoint checkPoint;   // 적 체크포인트 설정 부모 오브젝트
    public EnemySpawner enemySpawner;   // 적 스포너
    public Text waveHelpText;   // 웨이브 안내 텍스트
    public Coroutine coroutine;

    public const string NULL_MAP_NAME = "TestStage_2";
    public const string INIT_MAP_NAME = "TestStage_2";

    bool isStart = false;

    void Start()
    {
        coroutine = StartCoroutine(LoadCustomMap(GameManager.Instance.stageName.ToString()));
    }

    public IEnumerator LoadCustomMap(string mapName)
    {
        while (!(BlockManager.Instance.isLoadAll && EnemyManager.Instance.isLoadAll && TurretManager.Instance.isLoadData && TurretManager.Instance.isLoadStaticData && ObjManager.Instance.isLoadAll && MapStageManager.Instance.isLoadAll))
        {
            yield return null;
        }

        if(mapName == null)
        {
            mapName = NULL_MAP_NAME;
        }

        // MapStageManager.Instance.GetMap((MapStageManager.MapName)mapName)

        //Text tempText = Resources.Load(mapName) as Text;
        //customTileMap = JsonUtility.FromJson<MapEditorController.CustomTileMap>(tempText.text);

        if(mapName == "CUSTOM")
        {
            customTileMap = GameManager.Instance.customTileMap;
        }
        else
        {
            string getStr = MapStageManager.Instance.GetMap((MapStageManager.MapName)System.Enum.Parse(typeof(MapStageManager.MapName), mapName));
            customTileMap = JsonUtility.FromJson<MapEditorController.CustomTileMap>(getStr);
        }

        try
        {
            customTileMap.CreateCustomMap();
            cc.enabled = false;
            player.transform.position = customTileMap.spawnPosition.placeGameTransform;
            cc.enabled = true;

            CreateCheckPointChildObject();  // 체크포인트 위치 맵 정보에 따라 새로 지정
            checkPoint.SetCheckPoint();     // 체크포인트 위치 등록함

            enemySpawner.Init(customTileMap.spawnEnemyPosition.placeGameTransform, customTileMap.spawnEnemyInfoList);
        }
        catch(Exception e)
        {
            StopAllCoroutines();
            enemySpawner.EndStage();
        }

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
        const int INIT_STAGE = 0;
        bool checkPlayState = false;

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            
            checkPlayState = true;
            MapStageManager.Instance.LoadAll();
            BlockManager.Instance.LoadAll();
            EnemyManager.Instance.LoadAll();
            TurretManager.Instance.LoadAll();
            ObjManager.Instance.LoadAll();
            coroutine = StartCoroutine(LoadCustomMap(INIT_MAP_NAME));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            checkPlayState = true;
            enemySpawner.StartStage(INIT_STAGE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            checkPlayState = true;
            GameManager.Instance.LoadScene(GameManager.PlayStates.MAIN_MENU, -1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isStart)
                return;

            checkPlayState = true;
            isStart = true;

            enemySpawner.StartStage(INIT_STAGE);
            waveHelpText.text = "";
        }

        if(checkPlayState == true)
        {
            GameManager.Instance.PlayeState = GameManager.PlayStates.IN_GAME;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            enemySpawner.EndStage();
        }
    }

}
