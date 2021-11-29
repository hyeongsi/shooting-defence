using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnEnemyInfo> spawnEnemyInfoList;
    public IEnumerator myCoroutine;
    public bool isWorking = false;

    public void Init(Vector3 position, List<SpawnEnemyInfo> spawnEnemyInfoList)
    {
        transform.position = position;
        this.spawnEnemyInfoList = spawnEnemyInfoList;
    }

    public bool StartStage(int stage)
    {
        if (isWorking)
            return false;

        myCoroutine = SpawnEnemy(stage);
        StartCoroutine(myCoroutine);
        return true;
    }

    public IEnumerator SpawnEnemy(int stage)
    {
        int currentWave = -1;

        if(spawnEnemyInfoList.Count >= stage)
        {
            Debug.Log("해당 스테이지가 존재하지 않음");
            yield break;
        }
        if(spawnEnemyInfoList[stage].spawnEnemyList.Count != 0)
        {
            Debug.Log("해당 스테이지에 소환될 몹 없음");
            yield break;
        }

        for (int i = 0; i < spawnEnemyInfoList[stage].spawnEnemyList.Count; i++)
        {
            currentWave++;
            Debug.Log("몹 소환 전 대기");
            yield return new WaitForSeconds(spawnEnemyInfoList[stage].waitTimeBeforeSpawnList[currentWave]);    // 스테이지 시작 전 대기

            // 실제 몹 소환
            GameObject spawnEnemy = EnemyManager.Instance.InstantiateEnemy((EnemyManager.EnemyName)spawnEnemyInfoList[stage].spawnEnemyList[currentWave]);
            spawnEnemy.transform.parent = transform;    // 웨이브 넘어갈 때 다 잡았는지 체크할 때 사용함
            Debug.Log("몹 소환");
            yield return new WaitForSeconds(spawnEnemyInfoList[stage].spawnDelayList[i]);    // 몹 소환 딜레이 적용
        }

        isWorking = false;
    }
}
