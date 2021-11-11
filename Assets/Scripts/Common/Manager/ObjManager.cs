using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjManager : Singleton<ObjManager>
{
    private AsyncOperationHandle<GameObject> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<GameObject>> objectDictionary = new Dictionary<int, AsyncOperationHandle<GameObject>>();

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
        foreach (ObjName objectName in Enum.GetValues(typeof(ObjName)))
        {
            LoadObject(objectName);
        }
    }

    public void LoadObject(ObjName objectName)  // 오브젝트 리소스 로딩
    {
        loadAsyncOperationHandle = default;

        if (!objectDictionary.TryGetValue((int)objectName, out loadAsyncOperationHandle))
        {
            Addressables.LoadAssetAsync<GameObject>(objectName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    objectDictionary.Add((int)objectName, asyncOperationHandle);
                    Debug.Log(objectName.ToString() + " Object 로드 완료");
                };
        }
        else
        {
            if (loadAsyncOperationHandle.Result != false)
                return;

            Addressables.LoadAssetAsync<GameObject>(objectName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    objectDictionary[(int)objectName] = asyncOperationHandle;
                };
        }
    }

    public void ClearDictionary()   // 초기화
    {
        foreach (KeyValuePair<int, AsyncOperationHandle<GameObject>> blocks in objectDictionary)
        {
            Addressables.Release(objectDictionary[blocks.Key]);
        }

        objectDictionary.Clear();
    }

    public GameObject GetObject(ObjName objectName)
    {
        loadAsyncOperationHandle = default;

        if (objectDictionary.TryGetValue((int)objectName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == false)
            {
                Debug.Log("로딩된 오브젝트가 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadAsyncOperationHandle.Result;
        }

        Debug.Log("로딩된 오브젝트가 없어서 가져올 수 없습니다.");
        return null;
    }

    public enum ObjName
    {
        Water_Tower = 0,
        Apt_Door_Corner1 = 1,
        Player_Spawner = 2,
        //Enemy_Spawn = 3,
    }
}
