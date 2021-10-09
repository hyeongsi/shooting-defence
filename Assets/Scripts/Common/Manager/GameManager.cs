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

    #region Property
    public PlayStates PlayeState { set { playState = value; }  get { return playState; } }
    #endregion

    public void LoadScecne(PlayStates state)
    {

    }

    public void InitGame()
    {
        // 게임 시작 시, 게임에 필요한 데이터 모두 로딩 작업, (블럭, 타워, 몬스터, 플레이어, ui 등)
        TurretManager.Instance.LoadTurretData();
        EnemyManager.Instance.LoadEnemyData();
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


    #region EnumStorage
    public enum PlayStates
    {
        MAIN_MENU = 0,
        IN_GAME = 1,
        MAP_EDIT = 2,
        LOADING = 3,
    }
    #endregion
}
