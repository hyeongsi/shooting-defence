using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Scene_Loading : UI_Scene
{
    [SerializeField]
    Image progressBar;

    private void Start()
    {
        Init();
        ShowSceneUI();
    }

    private void OnEnable()
    {
        progressBar.fillAmount = 0f;
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        if(GameManager.Instance.PlayeState == GameManager.PlayStates.IN_GAME)
        {
            //EnemyManager.Instance.LoadEnemyData();
            //yield return null;
            //TurretManager.Instance.LoadTurretData();
            //yield return null;

        }

        AsyncOperation op = SceneManager.LoadSceneAsync((int)GameManager.Instance.PlayeState, LoadSceneMode.Additive);     // LoadSceneAsync() 로 불러오면 비동기로 씬 로딩 이동할건지 설정하는 옵션

        while (!op.isDone)
        {
            yield return null;
        }

        //op = SceneManager.UnloadSceneAsync("Loading");
        //while (!op.isDone)
        //{
        //    yield return null;
        //}
    }
}
