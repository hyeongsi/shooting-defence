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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static BarricadeManager Instance { get { return instance; } }
    #endregion
}
