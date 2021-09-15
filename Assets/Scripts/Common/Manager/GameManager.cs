using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void StopGameDelegate();
    public StopGameDelegate stopGameDelegate;

    private bool isStop = false;

    public bool IsStop
    {
        get
        {
            return isStop;
        }
        set
        {
            isStop = value;

            if (!isStop)
                return;

            stopGameDelegate?.Invoke();
        }
    }

    #region Singleton
    private static GameManager instance = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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

}
