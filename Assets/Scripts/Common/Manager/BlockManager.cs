using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BlockManager : Singleton<BlockManager>
{
    private GameObject loadBlockObject;
    private AsyncOperationHandle<GameObject> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<GameObject>> blockDictionary = new Dictionary<int, AsyncOperationHandle<GameObject>>();

    private void Awake()
    {
        if (null == instance)
        {
            instance = Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject LoadBlock(BlockName blockName)
    {
        loadBlockObject = null;
        loadAsyncOperationHandle = default;

        if(!blockDictionary.TryGetValue((int)blockName, out loadAsyncOperationHandle))
        {

        }
        else
        {
            return loadAsyncOperationHandle.Result;
        }

        if(loadBlockObject == null)
        {
            Addressables.LoadAssetAsync<GameObject>(blockName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    blockDictionary.Add((int)blockName, asyncOperationHandle);
                };
        }
        else
        {
            return loadBlockObject;
        }

        return null;
    }

    public enum BlockName
    {
        Block1_Gray = 0,
        Block1_White = 1,
        Block1_Yellow = 2,
    }
}
