using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Scene_Loading : UI_Scene
{
    [SerializeField]
    public Slider progressBar;

    private void Start()
    {
        Init();
        ShowSceneUI();
    }

    private void OnEnable()
    {
        progressBar.value = 0f;
    }

    public void LoadScene(string stageName)
    {
        StartCoroutine(LoadSceneProcess(stageName));
    }

    IEnumerator LoadSceneProcess(string stageName)
    {
        GameManager.Instance.stageName = stageName;
        AsyncOperation op = SceneManager.LoadSceneAsync((int)GameManager.Instance.PlayeState, LoadSceneMode.Additive);     // LoadSceneAsync() 로 불러오면 비동기로 씬 로딩 이동할건지 설정하는 옵션
        op.allowSceneActivation = false;
        
        while (!op.isDone)
        {
            yield return null;
            if(progressBar.value < 1f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime);
            }
            else
            {
                yield return 2f;
                op.allowSceneActivation = true;
            }
        }

        op = SceneManager.UnloadSceneAsync("Loading");
        while (!op.isDone)
        {
            yield return null;
        }
    }
}
