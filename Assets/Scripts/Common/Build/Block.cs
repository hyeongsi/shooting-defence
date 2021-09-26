using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    protected Vector3 blockSize;
    [SerializeField]
    protected BlockType blockType = BlockType.BLOCK;
    [SerializeField]
    protected BuildBlockType buildBlockType = BuildBlockType.ANY;

    #region property
    public Vector3 BlockSize
    {
        get { return blockSize; }
        set { blockSize = value; }
    }

    public BlockType BlockTypeVar
    {
        get { return blockType; }
    }

    public BuildBlockType BuildBlockTypeVar
    {
        get { return buildBlockType; }
    }


    #endregion
}

public enum BlockType
{
    BLOCK = 0,
    BARRICADE = 1,
    TURRET = 2,
}

public enum BuildBlockType
{
    ANY = 0,
    UP = 1,
    SIDE = 2,
    DOWN = 3,
}