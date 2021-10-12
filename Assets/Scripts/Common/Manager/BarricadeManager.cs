using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BarricadeManager : Singleton<BarricadeManager>
{
    private GameObject tempInstantiateGameObject;

    private AsyncOperationHandle<GameObject> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<GameObject>> barricadeDictionary = new Dictionary<int, AsyncOperationHandle<GameObject>>();

    private AsyncOperationHandle<TextAsset> barricadeStaticDataAsyncOperationHandle;
    private BarricadeStaticData loadBarricadeStaticData;
    private Dictionary<int, BarricadeStaticData> barricadeStaticDataDictionary = new Dictionary<int, BarricadeStaticData>();

    private void Awake()
    {
        if (null == instance)
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
        LoadBarricadeStaticData();
        foreach (BarricadeName barricadeName in Enum.GetValues(typeof(BarricadeName)))
        {
            LoadBarricade(barricadeName);
        }
    }

    public void LoadBarricadeStaticData()  // 바리게이트 데이터 로드
    {
        const float CORRECTION_VALUE = 0.1f;

        Addressables.LoadAssetAsync<TextAsset>("BarricadeData").Completed +=
            (AsyncOperationHandle<TextAsset> asyncOperationHandle) =>
            {
                barricadeStaticDataAsyncOperationHandle = asyncOperationHandle;

                BarricadeStaticData newBarricadeStaticData;
                string barricadeCsvString = asyncOperationHandle.Result.text;
                List<string[]> csvString = FileManager.Instance.ConvertCsvToString(barricadeCsvString);

                try
                {
                    for (int i = 0; i < csvString.Count; i++)
                    {
                        newBarricadeStaticData = new BarricadeStaticData(
                            int.Parse(csvString[i][(int)BarricadeCsvColumn.HP]) * CORRECTION_VALUE);

                        barricadeStaticDataDictionary.Add(int.Parse(csvString[i][(int)BarricadeCsvColumn.INDEX]), newBarricadeStaticData);
                    }
                }
                catch
                {
                    Debug.Log("Barricade static data load error");
                    return;
                }
            };
    }

    public void LoadBarricade(BarricadeName barricadeName)  // 바리게이트 오브젝트 로딩
    {
        loadAsyncOperationHandle = default;

        if (!barricadeDictionary.TryGetValue((int)barricadeName, out loadAsyncOperationHandle))
        {
            Addressables.LoadAssetAsync<GameObject>(barricadeName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    barricadeDictionary.Add((int)barricadeName, asyncOperationHandle);
                    Debug.Log(barricadeName.ToString() + " Barricade 로드 완료");
                };
        }
        else
        {
            if (loadAsyncOperationHandle.Result != null)
                return;

            Addressables.LoadAssetAsync<GameObject>(barricadeName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    barricadeDictionary[(int)barricadeName] = asyncOperationHandle;
                };
        }
    }

    public void ClearDictionary()   // 초기화
    {
        // 터렛 오브젝트 메모리 초기화
        foreach (KeyValuePair<int, AsyncOperationHandle<GameObject>> barricades in barricadeDictionary)
        {
            Addressables.Release(barricadeDictionary[barricades.Key]);
        }
        barricadeDictionary.Clear();

        // 터렛 데이터 메모리 초기화
        Addressables.Release(barricadeStaticDataAsyncOperationHandle);
        barricadeStaticDataDictionary.Clear();
    }

    public GameObject GetBarricade(BarricadeName barricadeName)  // 바리게이트 오브젝트 리턴
    {
        loadAsyncOperationHandle = default;

        if (barricadeDictionary.TryGetValue((int)barricadeName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
            {
                Debug.Log("로딩된 바리게이트가 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadAsyncOperationHandle.Result;
        }

        Debug.Log("로딩된 바리게이트가 없어서 가져올 수 없습니다.");
        return null;
    }

    public BarricadeStaticData GetBarricadeStaticData(BarricadeName barricadeName)  // 바리게이트 데이터 리턴
    {
        loadBarricadeStaticData = null;

        if (barricadeStaticDataDictionary.TryGetValue((int)barricadeName, out loadBarricadeStaticData))
        {
            if (loadBarricadeStaticData == null)
            {
                Debug.Log("로딩된 바리게이트 데이터가 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadBarricadeStaticData;
        }

        Debug.Log("로딩된 바리게이트 데이터가 없어서 가져올 수 없습니다.");
        return null;
    }

    public GameObject InstantiateBarricade(BarricadeName barricadeName)
    {
        loadAsyncOperationHandle = default;
        loadBarricadeStaticData = null;
        tempInstantiateGameObject = null;

        if (barricadeDictionary.TryGetValue((int)barricadeName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
                return null;

            if (barricadeStaticDataDictionary.TryGetValue((int)barricadeName, out loadBarricadeStaticData))
            {
                if (loadBarricadeStaticData == null)
                    return null;

                tempInstantiateGameObject = Instantiate(loadAsyncOperationHandle.Result);
                tempInstantiateGameObject.GetComponent<Barricade>().Init(loadBarricadeStaticData);

                return tempInstantiateGameObject;
            }
        }

        return null;
    }

    public enum BarricadeName
    {
        Barrier1 = 0,
    }
    private enum BarricadeCsvColumn
    {
        INDEX = 0,
        NAME = 1,
        HP = 2,
    }
}

public class BarricadeStaticData
{
    public float maxHp = 0.0f;

    public BarricadeStaticData(float maxHp)
    {
        this.maxHp = maxHp;
    }
}