using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnEnemyInfo> spawnEnemyInfoList; // 스테이지에서 스폰될 적 정보들
    public IEnumerator myCoroutine;
    public Text waveHelpText;   // 웨이브 안내 텍스트

    public void Init(Vector3 position, List<SpawnEnemyInfo> spawnEnemyInfoList)
    {
        transform.position = position;
        this.spawnEnemyInfoList = spawnEnemyInfoList;
    }

    public void StartStage(int stage)
    {
        myCoroutine = SpawnEnemy(stage);
        StartCoroutine(myCoroutine);
    }

    public IEnumerator SpawnEnemy(int stage)
    {
        int currentWave = -1;

        if(spawnEnemyInfoList.Count <= stage)
        {
            waveHelpText.text = "스테이지가 존재하지 않습니다.\n\n 잠시 후 게임이 종료됩니다.";
            yield return new WaitForSeconds(3f);
            GameManager.Instance.LoadScene(GameManager.PlayStates.MAIN_MENU, -1);
            yield break;
        }
        if(spawnEnemyInfoList[stage].spawnEnemyList.Count <= 0)
        {
            waveHelpText.text = "스테이지에 소환될 적들의 정보가 없습니다.\n\n 잠시 후 게임이 종료됩니다.";
            yield return new WaitForSeconds(3f);
            GameManager.Instance.LoadScene(GameManager.PlayStates.MAIN_MENU, -1);
            yield break;
        }

        for (int i = 0; i < spawnEnemyInfoList[stage].spawnEnemyList.Count; i++)
        {
            currentWave++;
            Debug.Log("몹 소환 전 대기");
            yield return new WaitForSeconds(spawnEnemyInfoList[stage].waitTimeBeforeSpawnList[currentWave]);    // 스테이지 시작 전 대기

            for(int j = 0; j < spawnEnemyInfoList[stage].spawnCountList[currentWave]; j ++)
            {
                // 실제 몹 소환
                GameObject spawnEnemy = EnemyManager.Instance.InstantiateEnemy((EnemyManager.EnemyName)spawnEnemyInfoList[stage].spawnEnemyList[currentWave], transform.position, transform);
                Debug.Log("몹 소환");
                yield return new WaitForSeconds(spawnEnemyInfoList[stage].spawnDelayList[i]);    // 몹 소환 딜레이 적용
            }
        }

        while (transform.childCount != 0)
        {
            if (gameObject == null)
                yield break;
            yield return null;
        }

        if (spawnEnemyInfoList.Count <= (stage+1))
        {
            waveHelpText.text = "스테이지 클리어\n\n 잠시 후 게임이 종료됩니다.";
            yield return new WaitForSeconds(3f);
            GameManager.Instance.LoadScene(GameManager.PlayStates.MAIN_MENU, -1);
            // 클리어 관련 정보 저장하던지, 추가로 처리 필요
            yield break;
        }
        if (spawnEnemyInfoList[(stage + 1)].spawnEnemyList.Count <= 0)
        {
            waveHelpText.text = "스테이지 클리어\n\n 잠시 후 게임이 종료됩니다.";
            yield return new WaitForSeconds(3f);
            GameManager.Instance.LoadScene(GameManager.PlayStates.MAIN_MENU, -1);
            // 클리어 관련 정보 저장하던지, 추가로 처리 필요
            yield break;
        }

        for (int i = 10; i > 0; i--)
        {
            waveHelpText.text = i.ToString() + "초 후에 새로운 웨이브가 시작됩니다.";
            yield return new WaitForSeconds(1f);
        }
        waveHelpText.text = "";
        myCoroutine = SpawnEnemy(++stage);
        StartCoroutine(myCoroutine);
        yield break;
    }
}
