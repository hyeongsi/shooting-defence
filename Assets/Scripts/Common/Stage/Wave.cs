using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<waveSpawnEnemyStruct> enemyGameObject;
}

[System.Serializable]
public struct waveSpawnEnemyStruct
{
    public GameObject enemyGameObject;
    public int spawnSize;
    public float spawnTime;
}
