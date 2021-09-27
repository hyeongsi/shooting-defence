using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] barricadePrefabArray;
    private List<Block> barricadeBlockArray = new List<Block>();
    public GameObject[] BarricadePrefabArray { get { return barricadePrefabArray; } }
    public List<Block> BarricadeBlockArray { get { return barricadeBlockArray; } }

    #region Singleton
    static BarricadeManager instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static BarricadeManager Instance { get { return instance; } }
    #endregion

    private void Init()
    {
        for(int i = 0; i < barricadePrefabArray.Length; i++)
        {
            barricadeBlockArray.Add(barricadePrefabArray[i].GetComponent<Block>());
        }
    }
}
