using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] barricadePrefabArray;

    public GameObject[] BarricadePrefabArray { get { return barricadePrefabArray; } }

    #region Singleton
    static BarricadeManager instance = null;
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
    public static BarricadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BarricadeManager();
            }

            return instance;
        }
    }
    #endregion
}
