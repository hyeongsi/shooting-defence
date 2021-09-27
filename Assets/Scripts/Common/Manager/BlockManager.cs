using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blockPrefabArray;
    private List<Block> blockArray = new List<Block>();

    public GameObject[] BlockPrefabArray { get { return blockPrefabArray; } }
    public List<Block> BlockArray { get { return blockArray; } }

    #region Singleton
    static BlockManager instance = null;
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
    public static BlockManager Instance { get { return instance; } }
    #endregion

    private void Init()
    {
        for (int i = 0; i < blockPrefabArray.Length; i++)
        {
            blockArray.Add(blockPrefabArray[i].GetComponent<Block>());
        }
    }
}
