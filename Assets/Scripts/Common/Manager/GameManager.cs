using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public delegate void PauseGameDelegate();
    public PauseGameDelegate pauseGameDelegate;

    public bool IsPause { get; private set; } = false;
    private PlayStates playState = PlayStates.MAIN_MENU;
    private Scene nextScene;
    public string stageName;

    #region Property
    public PlayStates PlayeState { set { playState = value; }  get { return playState; } }
    public Scene NextScene { set { nextScene = value; } get { return nextScene; } }
    #endregion

    public void LoadMapEditorData()
    {
        BlockManager.Instance.LoadAll();
        BarricadeManager.Instance.LoadAll();
        TurretManager.Instance.LoadAll();
    }

    public void LoadScene(PlayStates nextstate, int stage = -1)
    {
        const string INIT_STAGE_NAME = "TestStage_2";

        if (stage < 0)
        {
            StartCoroutine(LoadAsyncSceneCourtine(nextstate, INIT_STAGE_NAME));
        }
        else
        {
            switch ((StageName)stage)
            {
                case StageName.TestStage_2:
                    StartCoroutine(LoadAsyncSceneCourtine(nextstate, ((StageName)stage).ToString()));
                    break;
                default:
                    StartCoroutine(LoadAsyncSceneCourtine(nextstate, INIT_STAGE_NAME));
                    break;
            }
        }
    }

    public void SwitchIsPause()
    {
        if (IsPause)
            ContinueGame();
        else
            PauseGame();
    }
    public void PauseGame()
    {
        IsPause = true;
        pauseGameDelegate?.Invoke();
    }
    public void ContinueGame()
    {
        IsPause = false;

        // esc 메뉴 치우는 함수 등록하기
    }
    public void ExitGame()
    {
        playState = PlayStates.MAIN_MENU;

        // 메인 메뉴로 씬 이동
    }

    IEnumerator LoadAsyncSceneCourtine(PlayStates next, string stageName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);

        while(!operation.isDone) 
        {
            yield return null;
        }

        operation = SceneManager.UnloadSceneAsync(((int)playState));
        while(!operation.isDone)
        {
            yield return null;
        }

        playState = next;

        UI_Scene_Loading loadingUI = null;
        loadingUI = UIManager.Instance.SceneUIData.ui_scene as UI_Scene_Loading;

        loadingUI.LoadScene(stageName);
    }

    #region EnumStorage
    public enum PlayStates
    {
        MAIN_MENU = 0,
        IN_GAME = 1,
        LOADING_SCENE = 2,
        MAP_EDIT = 3,
    }

    public enum StageName
    {
        TestStage_2 = 0,
    }
    #endregion
}
