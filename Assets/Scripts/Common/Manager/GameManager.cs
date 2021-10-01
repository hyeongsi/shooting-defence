using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void PauseGameDelegate();
    public PauseGameDelegate pauseGameDelegate;

    public bool IsPause { get; private set; } = false;
    private PlayStates playState = PlayStates.MAP_EDIT;

    #region Property
    public PlayStates PlayeState { set { playState = value; }  get { return playState; } }
    #endregion

    #region Singleton
    private static GameManager instance = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static GameManager Instance { get { return instance; } }
    #endregion

    public void InitGame()
    {
        // 게임 시작 시, 게임에 필요한 데이터 모두 로딩 작업, (블럭, 타워, 몬스터, 플레이어, ui 등)
        TurretManager.Instance.LoadTurretData();
        EnemyManager.Instance.LoadEnemyData();
    }

    public void LoadMapData() 
    {
        switch(playState)
        {
            case PlayStates.SINGLE_PLAY:
                // 캠페인 맵 모두 로딩하도록
                break;
            case PlayStates.MULTY_PLAY:
                // 들어갈 방 선택하면 해당 방 데이터 통신해서 받도록 구현
                break;
            default:
                return;
        }
    }

    public void SwitchIsPause()
    {
        IsPause = !IsPause;
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
        SINGLE_PLAY = 1,
        MULTY_PLAY = 2,
        MAP_EDIT = 3,
    }
    #endregion
}
