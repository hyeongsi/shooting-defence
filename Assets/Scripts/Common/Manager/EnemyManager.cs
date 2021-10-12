using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyManager : Singleton<EnemyManager>
{
    private GameObject tempInstantiateGameObject;

    private AsyncOperationHandle<GameObject> loadAsyncOperationHandle;
    private Dictionary<int, AsyncOperationHandle<GameObject>> enemyDictionary = new Dictionary<int, AsyncOperationHandle<GameObject>>();

    private AsyncOperationHandle<TextAsset> enemyStaticDataAsyncOperationHandle;
    private EnemyStaticData loadEnemyStaticData;
    private Dictionary<int, EnemyStaticData> enemyStaticDataDictionary = new Dictionary<int, EnemyStaticData>();

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

    public void LoadEnemyStaticData()   // 적 데이터 로드
    {
        const float CORRECTION_VALUE = 0.1f;

        Addressables.LoadAssetAsync<TextAsset>("EnemyData").Completed +=
            (AsyncOperationHandle<TextAsset> asyncOperationHandle) =>
            {
                enemyStaticDataAsyncOperationHandle = asyncOperationHandle;

                EnemyStaticData newEnemyStaticData;
                string enemyCsvString = asyncOperationHandle.Result.text;
                List<string[]> csvString = FileManager.Instance.ConvertCsvToString(enemyCsvString);

                try
                {
                    for (int i = 0; i < csvString.Count; i++)
                    {
                        newEnemyStaticData = new EnemyStaticData(
                           int.Parse(csvString[i][(int)EnemyCsvColumn.ATTACK_TYPE]),
                           int.Parse(csvString[i][(int)EnemyCsvColumn.HP]) * CORRECTION_VALUE,
                           int.Parse(csvString[i][(int)EnemyCsvColumn.ATTACK_DAMAGE]) * CORRECTION_VALUE,
                           int.Parse(csvString[i][(int)EnemyCsvColumn.ATTACK_RANGE]) * CORRECTION_VALUE,
                           int.Parse(csvString[i][(int)EnemyCsvColumn.ATTACK_DELAY]) * CORRECTION_VALUE,
                           int.Parse(csvString[i][(int)EnemyCsvColumn.SPIN_SPEED]) * CORRECTION_VALUE);

                        enemyStaticDataDictionary.Add(int.Parse(csvString[i][(int)EnemyCsvColumn.INDEX]), newEnemyStaticData);
                    }
                }
                catch
                {
                    Debug.Log("Enemy static data load error");
                    return;
                }
            };
    }

    public void LoadEnemy(EnemyName enemyName)  // 적 오브젝트 로딩
    {
        loadAsyncOperationHandle = default;

        if (!enemyDictionary.TryGetValue((int)enemyName, out loadAsyncOperationHandle))
        {
            Addressables.LoadAssetAsync<GameObject>(enemyName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    enemyDictionary.Add((int)enemyName, asyncOperationHandle);
                    Debug.Log(enemyName.ToString() + " Enemy 로드 완료");
                };
        }
        else
        {
            if (loadAsyncOperationHandle.Result != null)
                return;

            Addressables.LoadAssetAsync<GameObject>(enemyName.ToString()).Completed +=
                (AsyncOperationHandle<GameObject> asyncOperationHandle) =>
                {
                    enemyDictionary[(int)enemyName] = asyncOperationHandle;
                };
        }
    }

    public void ClearDictionary()   // 초기화
    {
        // 터렛 오브젝트 메모리 초기화
        foreach (KeyValuePair<int, AsyncOperationHandle<GameObject>> enemies in enemyDictionary)
        {
            Addressables.Release(enemyDictionary[enemies.Key]);
        }
        enemyDictionary.Clear();

        // 터렛 데이터 메모리 초기화
        Addressables.Release(enemyStaticDataAsyncOperationHandle);
        enemyStaticDataDictionary.Clear();
    }

    public GameObject GetEnemy(EnemyName enemyName)  // 적 오브젝트 리턴
    {
        loadAsyncOperationHandle = default;

        if (enemyDictionary.TryGetValue((int)enemyName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
            {
                Debug.Log("로딩된 적이 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadAsyncOperationHandle.Result;
        }

        Debug.Log("로딩된 적이 없어서 가져올 수 없습니다.");
        return null;
    }

    public EnemyStaticData GetEnemyStaticData(EnemyName enemyName)  // 적 데이터 리턴
    {
        loadEnemyStaticData = null;

        if (enemyStaticDataDictionary.TryGetValue((int)enemyName, out loadEnemyStaticData))
        {
            if (loadEnemyStaticData == null)
            {
                Debug.Log("로딩된 적 데이터가 없어서 가져올 수 없습니다.");
                return null;
            }

            return loadEnemyStaticData;
        }

        Debug.Log("로딩된 적 데이터가 없어서 가져올 수 없습니다.");
        return null;
    }

    public GameObject InstantiateEnemy(EnemyName enemyName)
    {
        loadAsyncOperationHandle = default;
        loadEnemyStaticData = null;
        tempInstantiateGameObject = null;

        if (enemyDictionary.TryGetValue((int)enemyName, out loadAsyncOperationHandle))
        {
            if (loadAsyncOperationHandle.Result == null)
                return null;

            if (enemyStaticDataDictionary.TryGetValue((int)enemyName, out loadEnemyStaticData))
            {
                if (loadEnemyStaticData == null)
                    return null;

                tempInstantiateGameObject = Instantiate(loadAsyncOperationHandle.Result);
                tempInstantiateGameObject.GetComponent<Enemy>().Init(loadEnemyStaticData);

                return tempInstantiateGameObject;
            }  
        }

        return null;
    }

    public enum EnemyName
    {
        Zombie1 = 0,
    }

    private enum EnemyCsvColumn
    {
        INDEX = 0,
        NAME = 1,
        ATTACK_TYPE = 2,
        HP = 3,
        ATTACK_DAMAGE = 4,
        ATTACK_RANGE = 5,
        ATTACK_DELAY = 6,
        SPIN_SPEED = 7,
    }
}

public class EnemyStaticData
{
    public int attackType = 0;
    public float maxHp = 0.0f;
    public float attackDamage = 0.0f;
    public float attackRange = 0.0f;
    public float attackDelay = 0.0f;
    public float moveSpeed = 0.0f;

    public EnemyStaticData(int attackType, float maxHp, float attackDamage, float attackRange, float attackDelay, float moveSpeed)
    {
        this.maxHp = maxHp;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        this.attackDelay = attackDelay;
        this.moveSpeed = moveSpeed;
    }
}
