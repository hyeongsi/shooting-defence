using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blockPrefabArray;

    public GameObject[] BlockPrefabArray { get { return blockPrefabArray; } }

    #region Singleton
    static BlockManager instance = null;
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
    public static BlockManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BlockManager();
            }

            return instance;
        }
    }
    #endregion
}
