using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    
    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    //IEnumerator LoadSceneProcess()
    //{
    //    AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);     // LoadSceneAsync() 로 불러오면 비동기로 씬 로딩
    //    op.allowSceneActivation = false;    // 씬을 비동기로 불러들일 때 씬 로딩이 끝나면 불러온 씬으로 자동 이동할건지 설정하는 옵션
    //}
}
