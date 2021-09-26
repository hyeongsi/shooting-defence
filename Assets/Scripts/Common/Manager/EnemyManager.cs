using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefabArray;
    private readonly List<EnemyStaticData> enemyStaticDataList = new List<EnemyStaticData>();

    #region Property
    public GameObject[] EnemyPrefabArray { get { return enemyPrefabArray; } }
    #endregion
    #region Singleton
    static EnemyManager instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static EnemyManager Instance { get { return instance; } }

    #endregion
    public void SettingEnemyData(ref GameObject enemyObject, int enemyIndex) // 적 생성 후 데이터 초기화
    {
        if (enemyIndex < 0 || enemyStaticDataList.Count <= enemyIndex)
            return;

        enemyObject.GetComponent<Enemy>().Init(enemyStaticDataList[enemyIndex]);
    }
    public void LoadEnemyData()
    {
        TextAsset textAsset = Resources.Load("Enemy") as TextAsset;
        string enemyCsvString = textAsset.text;

        if (enemyCsvString == default)     // 로딩 데이터 없으면 종료
            return;

        List<string[]> csvString = FileManager.Instance.ConvertCsvToString(enemyCsvString);
        EnemyStaticData newEnemyStaticData;

        const float CORRECTION_VALUE = 0.1f;
        enemyStaticDataList.Clear();

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

                enemyStaticDataList.Add(newEnemyStaticData);
            }
        }
        catch
        {
            enemyStaticDataList.Clear();
            return;
        }

    }

    private enum EnemyCsvColumn
    {
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
