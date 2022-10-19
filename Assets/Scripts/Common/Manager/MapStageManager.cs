using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapStageManager : Singleton<MapStageManager>
{
    private AsyncOperationHandle<TextAsset> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<TextAsset>> mapDictionary = new Dictionary<int, AsyncOperationHandle<TextAsset>>();

    public bool isLoadAll = false;

    private void Awake()
    {
        if (false == instance)
        {
            instance = Instance;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAll()
    {
        foreach (MapName mapName in Enum.GetValues(typeof(MapName)))
        {
            LoadMap(mapName);
        }
    }

    public void LoadMap(MapName mapName)  // 맵 리소스 로딩
    {
        loadAsyncOperationHandle = default;

        if (!mapDictionary.TryGetValue((int)mapName, out loadAsyncOperationHandle))
        {
            Addressables.LoadAssetAsync<TextAsset>(mapName.ToString()).Completed +=
                (AsyncOperationHandle<TextAsset> asyncOperationHandle) =>
                {
                    mapDictionary.Add((int)mapName, asyncOperationHandle);
                    Debug.Log(mapName.ToString() + " map 로드 완료");

                    if (Enum.GetValues(typeof(MapName)).Length == mapDictionary.Count)
                    {
                        isLoadAll = true;
                    }
                };
        }
        else
        {
            if (loadAsyncOperationHandle.Result != false)
                return;

            Addressables.LoadAssetAsync<TextAsset>(mapName.ToString()).Completed +=
                (AsyncOperationHandle<TextAsset> asyncOperationHandle) =>
                {
                    mapDictionary[(int)mapName] = asyncOperationHandle;
                };
        }
    }

    public void ClearDictionary()   // 초기화
    {
        foreach (KeyValuePair<int, AsyncOperationHandle<TextAsset>> maps in mapDictionary)
        {
            Addressables.Release(mapDictionary[maps.Key]);
        }

        mapDictionary.Clear();
    }

    public string GetMap(MapName mapName)
    {
        loadAsyncOperationHandle = default;

        if (mapDictionary.TryGetValue((int)mapName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == false)
            {
                Debug.Log("로딩된 맵이 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadAsyncOperationHandle.Result.text;
        }

        Debug.Log("로딩된 맵이 없어서 가져올 수 없습니다.");
        return null;
    }

    public enum MapName
    {
        stage1 = 0,
        stage2 = 1,
        TestStage_2 = 2,
        stage3 = 3,
    }
}
