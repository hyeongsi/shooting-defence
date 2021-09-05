using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    BLOCK = 0,
    BARRICADE = 1,
    TURRET = 2,
}

public class Block : MonoBehaviour
{
    [SerializeField]
    protected Vector3 blockSize;
    protected BlockType blockType = BlockType.BLOCK;

    #region property
    public Vector3 BlockSize
    {
        get { return blockSize; }
        set { blockSize = value; }
    }

    public BlockType BlockType
    {
        get { return blockType; }
    }

    #endregion
}
