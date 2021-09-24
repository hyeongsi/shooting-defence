using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private List<Enemy> EnemyList = new List<Enemy>();
    private List<Enemy> spawnEnemyList = new List<Enemy>();

    static EnemyManager instance = null;

    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyManager();
            }

            return instance;
        }
    }
    public List<Enemy> SpawnEnemyList
    {
        get { return spawnEnemyList; }
    }

    public void ClearEnemy()
    {
        for(int i = spawnEnemyList.Count-1; i >= 0; i --)
        {
            spawnEnemyList[i].DestroyEnemy();
        }

        spawnEnemyList.Clear();
    }

    public void SpawnEnemy(int enemyIndex, Vector3 position)
    {
        if (enemyIndex < 0 && enemyIndex >= EnemyList.Count)
            return;

        spawnEnemyList.Add(EnemyList[enemyIndex]);
        EnemyList[enemyIndex].Spawn().transform.position = position;

        // 부모 오브젝트 설정해서 스폰 몹들 정리하도록 하기
    }

    public void DeleteEnemy(Enemy enemy)
    {
        spawnEnemyList.Remove(enemy);
    }

    public void LoadEnemyData()
    {
        TextAsset textAsset = Resources.Load("Turret") as TextAsset;
        string enemyCsvString = textAsset.text;

        if (enemyCsvString == default)     // 로딩 데이터 없으면 종료
            return;


    }

    public void UpdateSpawnEnemyList()
    {
        for(int i = 0; i < spawnEnemyList.Count; i ++)
        {
            spawnEnemyList[i].UpdateEnemy();
        }
    }
}
