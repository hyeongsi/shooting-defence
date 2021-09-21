using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    private List<Enemy> monsterList = new List<Enemy>();
    private List<Enemy> spawnMonsterList = new List<Enemy>();

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
        for(int i = spawnMonsterList.Count-1; i >= 0; i --)
        {
            spawnMonsterList[i].DestroyEnemy();
        }

        spawnMonsterList.Clear();
    }

    public void AddEnemy(Enemy enemy)
    {
        spawnMonsterList.Add(enemy);
    }

    public void DeleteEnemy(Enemy enemy)
    {
        spawnMonsterList.Remove(enemy);
    }

    public void LoadMonsterData()
    {
        // 해당 경로에 몹 데이터들 저장해 둬야함. 그러면 그거 불러와서 사용하도록 하기
        // Enemy로는 모든 Enemy 데이터를 받을 수 없기 떄문에, 처리할 수 있는 방안으로 처리하기....
        // monsterList에 집어넣기.
        FileManager.Instance.LoadJsonFile<Enemy>(Application.dataPath+"/Data", "/monsterData.json");
    }
}
