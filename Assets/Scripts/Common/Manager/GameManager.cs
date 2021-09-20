using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void PauseGameDelegate();
    public PauseGameDelegate pauseGameDelegate;

    public bool IsPause { get; private set; } = false;
    private PlayState playState = PlayState.MAIN_MENU;

    #region EnumStorage
    public enum PlayState
    {
        MAIN_MENU = 0,
        SINGLE_PLAY = 1,
        MULTY_PLAY = 2,
        MAP_EDIT = 3,
    }
    #endregion

    #region Singleton
    private static GameManager instance = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            InitGame();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public void InitGame()
    {
        // 게임 시작 시, 게임에 필요한 데이터 모두 로딩 작업, (블럭, 타워, 몬스터, 플레이어, ui 등)
        FileManager.Instance.LoadJsonFile<Object>("test", "test");
    }

    public void PauseGame()
    {
        IsPause = true;
        pauseGameDelegate?.Invoke();

        //pauseGameDelegate 에 esc 메뉴 관련해서 함수 등록하기
    }

    public void ContinueGame()
    {
        IsPause = false;

        // esc 메뉴 치우는 함수 등록하기
    }

    public void ExitGame()
    {
        playState = PlayState.MAIN_MENU;

        // 메인 메뉴로 씬 이동
    }
}
