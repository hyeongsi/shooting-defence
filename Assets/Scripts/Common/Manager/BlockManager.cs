using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BlockManager : Singleton<BlockManager>
{
    private AsyncOperationHandle<GameObject> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<GameObject>> blockDictionary = new Dictionary<int, AsyncOperationHandle<GameObject>>();

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
        foreach(BlockName blockName in Enum.GetValues(typeof(BlockName)))
        {
            LoadBlock(blockName);
        }
    }

    public void LoadBlock(BlockName blockName)  // 블럭 리소스 로딩
    {
        loadAsyncOperationHandle = default;

        if(!blockDictionary.TryGetValue((int)blockName, out loadAsyncOperationHandle))
        {
            Addressables.LoadAssetAsync<GameObject>(blockName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    blockDictionary.Add((int)blockName, asyncOperationHandle);
                    Debug.Log(blockName.ToString()+ " Block 로드 완료");

                    if (Enum.GetValues(typeof(BlockName)).Length == blockDictionary.Count)
                    {
                        isLoadAll = true;
                    }
                };
        }
        else
        {
            if (loadAsyncOperationHandle.Result != false)
                return;

            Addressables.LoadAssetAsync<GameObject>(blockName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    blockDictionary[(int)blockName] = asyncOperationHandle;
                };
        }
    }

    public void ClearDictionary()   // 초기화
    {
        foreach(KeyValuePair<int, AsyncOperationHandle<GameObject>> blocks in blockDictionary)
        {
            Addressables.Release(blockDictionary[blocks.Key]);
        }

        blockDictionary.Clear();
    }

    public GameObject GetBlock(BlockName blockName)
    {
        loadAsyncOperationHandle = default;

        if (blockDictionary.TryGetValue((int)blockName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == false)
            {
                Debug.Log("로딩된 블럭이 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadAsyncOperationHandle.Result;
        }

        Debug.Log("로딩된 블럭이 없어서 가져올 수 없습니다.");
        return null;
    }

    public enum BlockName
    {
        Block1_Gray = 0,
        Block1_White = 1,
        Block1_Yellow = 2,
        Block1_Green = 3,
    }
}
