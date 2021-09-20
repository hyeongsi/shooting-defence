using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    private List<Enemy> monsterList = new List<Enemy>();

    static MonsterManager instance = null;

    public static MonsterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MonsterManager();
            }

            return instance;
        }
    }

    public void ClearEnemy()
    {
        for(int i = monsterList.Count-1; i >= 0; i --)
        {
            monsterList[i].DestroyEnemy();
        }

        monsterList.Clear();
    }

    public void AddEnemy(Enemy enemy)
    {
        monsterList.Add(enemy);
    }

    public void DeleteEnemy(Enemy enemy)
    {
        monsterList.Remove(enemy);
    }

    public void LoadMonsterData()
    {
        FileManager.Instance.LoadJsonFile<Enemy>();
    }
}
