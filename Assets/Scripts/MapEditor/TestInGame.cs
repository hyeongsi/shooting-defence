using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInGame : MonoBehaviour
{
    void Start()
    {
        if(!(BlockManager.Instance.isLoadAll && EnemyManager.Instance.isLoadAll && EnemyManager.Instance.isLoadStaticData && ObjManager.Instance.isLoadAll))
        {
            BlockManager.Instance.LoadAll();
            EnemyManager.Instance.LoadAll();
            TurretManager.Instance.LoadAll();
            ObjManager.Instance.LoadAll();

            EnemyManager.Instance.LoadEnemyStaticData();
        }
    }
}
