using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TurretManager : Singleton<TurretManager>
{
    private GameObject tempInstantiateGameObject;

    private AsyncOperationHandle<GameObject> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<GameObject>> turretDictionary = new Dictionary<int, AsyncOperationHandle<GameObject>>();

    private AsyncOperationHandle<TextAsset> turretStaticDataAsyncOperationHandle;
    private TurretStaticData loadTurretStaticData;
    private Dictionary<int, TurretStaticData> turretStaticDataDictionary = new Dictionary<int, TurretStaticData>();

    public bool isLoadStaticData = false;
    public bool isLoadData = false;

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
        LoadTurretStaticData();
        foreach (TurretName turretName in Enum.GetValues(typeof(TurretName)))
        {
            LoadTurret(turretName);
        }
    }

    public void LoadTurretStaticData()  // 터렛 데이터 로드
    {
        const float CORRECTION_VALUE = 0.1f;

        Addressables.LoadAssetAsync<TextAsset>("TurretData").Completed +=
            (AsyncOperationHandle<TextAsset> asyncOperationHandle) =>
            {
                turretStaticDataAsyncOperationHandle = asyncOperationHandle;

                TurretStaticData newTurretStaticData;
                string turretCsvString = asyncOperationHandle.Result.text;
                List<string[]> csvString = FileManager.Instance.ConvertCsvToString(turretCsvString);
                
                try
                {
                    for (int i = 0; i < csvString.Count; i++)
                    {
                        newTurretStaticData = new TurretStaticData(
                            int.Parse(csvString[i][(int)TurretCsvColumn.HP]) * CORRECTION_VALUE,
                            int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DAMAGE]) * CORRECTION_VALUE,
                            int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_RANGE]) * CORRECTION_VALUE,
                            int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DELAY]) * CORRECTION_VALUE,
                            int.Parse(csvString[i][(int)TurretCsvColumn.SPIN_SPEED]) * CORRECTION_VALUE);

                        turretStaticDataDictionary.Add(int.Parse(csvString[i][(int)TurretCsvColumn.INDEX]), newTurretStaticData);
                    }

                    isLoadStaticData = true;
                }
                catch
                {
                    Debug.Log("Turret static data load error");
                    return;
                }
            };
    }

    public int GetTurretSize()
    {
        return turretDictionary.Count;
    }

    public void LoadTurret(TurretName turretName)  // 터렛 오브젝트 로딩
    {
        loadAsyncOperationHandle = default;

        if (!turretDictionary.TryGetValue((int)turretName, out loadAsyncOperationHandle))
        {
            Addressables.LoadAssetAsync<GameObject>(turretName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    turretDictionary.Add((int)turretName, asyncOperationHandle);
                    Debug.Log(turretName.ToString() + " Turret 로드 완료");
                    isLoadData = true;
                };
        }
        else
        {
            if (loadAsyncOperationHandle.Result != null)
                return;

            Addressables.LoadAssetAsync<GameObject>(turretName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    turretDictionary[(int)turretName] = asyncOperationHandle;
                };
        }
    }

    public void ClearDictionary()   // 초기화
    {
        // 터렛 오브젝트 메모리 초기화
        foreach (KeyValuePair<int, AsyncOperationHandle<GameObject>> turrets in turretDictionary)
        {
            Addressables.Release(turretDictionary[turrets.Key]);
        }
        turretDictionary.Clear();

        // 터렛 데이터 메모리 초기화
        Addressables.Release(turretStaticDataAsyncOperationHandle);
        turretStaticDataDictionary.Clear();
    }

    public GameObject GetTurret(TurretName turretName)  // 터렛 오브젝트 리턴
    {
        loadAsyncOperationHandle = default;

        if (turretDictionary.TryGetValue((int)turretName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
            {
                Debug.Log("로딩된 터렛이 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadAsyncOperationHandle.Result;
        }

        Debug.Log("로딩된 터렛이 없어서 가져올 수 없습니다.");
        return null;
    }

    public TurretStaticData GetTurretStaticData(TurretName turretName)  // 터렛 데이터 리턴
    {
        loadTurretStaticData = null;

        if (turretStaticDataDictionary.TryGetValue((int)turretName, out loadTurretStaticData))
        {
            if (loadTurretStaticData == null)
            {
                Debug.Log("로딩된 터렛 데이터가 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadTurretStaticData;
        }

        Debug.Log("로딩된 터렛 데이터가 없어서 가져올 수 없습니다.");
        return null;
    }

    public GameObject InstantiateTurret(TurretName turretName)
    {
        loadAsyncOperationHandle = default;
        loadTurretStaticData = null;
        tempInstantiateGameObject = null;

        if (turretDictionary.TryGetValue((int)turretName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
                return null;

            if (turretStaticDataDictionary.TryGetValue((int)turretName, out loadTurretStaticData))
            {
                if (loadTurretStaticData == null)
                    return null;

                tempInstantiateGameObject = Instantiate(loadAsyncOperationHandle.Result);
                tempInstantiateGameObject.GetComponent<Turret>().Init(loadTurretStaticData);

                return tempInstantiateGameObject;
            }
        }

        return null;
    }

    public enum TurretName
    {
        rifleTurret = 0,
        sniperTurret = 1,
        singleTurret = 2,
        doubleTurret = 3,
    }

    private enum TurretCsvColumn
    {
        INDEX = 0,
        NAME = 1,
        HP = 2,
        ATTACK_DAMAGE = 3,
        ATTACK_RANGE = 4,
        ATTACK_DELAY = 5,
        SPIN_SPEED = 6,
    }
}
public class TurretStaticData
{
    public float maxHp = 0.0f;
    public float attackDamage = 0.0f;
    public float attackRange = 0.0f;
    public float attackDelay = 0.0f;
    public float spinSpeed = 0.0f;

    public TurretStaticData(float maxHp, float attackDamage, float attackRange, float attackDelay, float spinSpeed) 
    {
        this.maxHp = maxHp;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        this.attackDelay = attackDelay;
        this.spinSpeed = spinSpeed;
    }
}