using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageManager : Singleton<StageManager>
{
    private GameObject tempInstantiateGameObject;

    private AsyncOperationHandle<GameObject> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<GameObject>> stageDictionary = new Dictionary<int, AsyncOperationHandle<GameObject>>();

    private AsyncOperationHandle<TextAsset> stageStaticDataAsyncOperationHandle;
    private StageStaticData loadStageStaticData;
    private Dictionary<int, StageStaticData> stageStaticDataDictionary = new Dictionary<int, StageStaticData>();

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
        LoadStageStaticData();
        foreach (StageName stageName in Enum.GetValues(typeof(StageName)))
        {
            LoadStage(stageName);
        }
    }

    public void LoadStageStaticData()  // 스테이지 데이터 로드
    {
        //const float CORRECTION_VALUE = 0.1f;

        Addressables.LoadAssetAsync<TextAsset>("StageData").Completed +=
            (AsyncOperationHandle<TextAsset> asyncOperationHandle) =>
            {
                stageStaticDataAsyncOperationHandle = asyncOperationHandle;

                StageStaticData newStageStaticData;
                string stageCsvString = asyncOperationHandle.Result.text;
                List<string[]> csvString = FileManager.Instance.ConvertCsvToString(stageCsvString);

                try
                {
                    for (int i = 0; i < csvString.Count; i++)
                    {
                        //newTurretStaticData = new TurretStaticData(
                        //    int.Parse(csvString[i][(int)TurretCsvColumn.HP]) * CORRECTION_VALUE,
                        //    int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DAMAGE]) * CORRECTION_VALUE,
                        //    int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_RANGE]) * CORRECTION_VALUE,
                        //    int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DELAY]) * CORRECTION_VALUE,
                        //    int.Parse(csvString[i][(int)TurretCsvColumn.SPIN_SPEED]) * CORRECTION_VALUE);

                        //stageStaticDataDictionary.Add(int.Parse(csvString[i][(int)TurretCsvColumn.INDEX]), newTurretStaticData);
                    }
                }
                catch
                {
                    Debug.Log("Stage static data load error");
                    return;
                }
            };
    }

    public void LoadStage(StageName stageName)  // 스테이지 오브젝트 로딩
    {
        loadAsyncOperationHandle = default;

        if (!stageDictionary.TryGetValue((int)stageName, out loadAsyncOperationHandle))
        {
            Addressables.LoadAssetAsync<GameObject>(stageName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    stageDictionary.Add((int)stageName, asyncOperationHandle);
                    Debug.Log(stageName.ToString() + " Stage 로드 완료");
                };
        }
        else
        {
            if (loadAsyncOperationHandle.Result != null)
                return;

            Addressables.LoadAssetAsync<GameObject>(stageName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    stageDictionary[(int)stageName] = asyncOperationHandle;
                };
        }
    }

    public void ClearDictionary()   // 초기화
    {
        // 스테이지 오브젝트 메모리 초기화
        foreach (KeyValuePair<int, AsyncOperationHandle<GameObject>> turrets in stageDictionary)
        {
            Addressables.Release(stageDictionary[turrets.Key]);
        }
        stageDictionary.Clear();

        // 터렛 데이터 메모리 초기화
        Addressables.Release(stageStaticDataAsyncOperationHandle);
        stageStaticDataDictionary.Clear();
    }

    public GameObject GetStage(StageName stageName)  // 스테이지 오브젝트 리턴
    {
        loadAsyncOperationHandle = default;

        if (stageDictionary.TryGetValue((int)stageName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
            {
                Debug.Log("로딩된 스테이지가 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadAsyncOperationHandle.Result;
        }

        Debug.Log("로딩된 스테이지가 없어서 가져올 수 없습니다.");
        return null;
    }

    public StageStaticData GetStageStaticData(StageName stageName)  // 스테이지 데이터 리턴
    {
        loadStageStaticData = null;

        if (stageStaticDataDictionary.TryGetValue((int)stageName, out loadStageStaticData))
        {
            if (loadStageStaticData == null)
            {
                Debug.Log("로딩된 스테이지 데이터가 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadStageStaticData;
        }

        Debug.Log("로딩된 스테이지 데이터가 없어서 가져올 수 없습니다.");
        return null;
    }

    public GameObject InstantiateStage(StageName stageName)
    {
        loadAsyncOperationHandle = default;
        loadStageStaticData = null;
        tempInstantiateGameObject = null;

        if (stageDictionary.TryGetValue((int)stageName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
                return null;

            if (stageStaticDataDictionary.TryGetValue((int)stageName, out loadStageStaticData))
            {
                if (loadStageStaticData == null)
                    return null;

                tempInstantiateGameObject = Instantiate(loadAsyncOperationHandle.Result);
                //tempInstantiateGameObject.GetComponent<Stage>().Init(loadTurretStaticData);

                return tempInstantiateGameObject;
            }
        }

        return null;
    }

    public enum StageName
    {
        //TempTurret1 = 0,
    }

    private enum StageCsvColumn
    {
        INDEX = 0,
        NAME = 1,
        //HP = 2,
        //ATTACK_DAMAGE = 3,
        //ATTACK_RANGE = 4,
        //ATTACK_DELAY = 5,
        //SPIN_SPEED = 6,
    }
}
public class StageStaticData
{
    //public float maxHp = 0.0f;
    //public float attackDamage = 0.0f;
    //public float attackRange = 0.0f;
    //public float attackDelay = 0.0f;
    //public float spinSpeed = 0.0f;

    //public StageStaticData(float maxHp, float attackDamage, float attackRange, float attackDelay, float spinSpeed) 
    //{
    //    this.maxHp = maxHp;
    //    this.attackDamage = attackDamage;
    //    this.attackRange = attackRange;
    //    this.attackDelay = attackDelay;
    //    this.spinSpeed = spinSpeed;
    //}
}