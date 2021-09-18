using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : Block
{
    [SerializeField]
    protected float hp;

    #region property
    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }
    #endregion

    private void Awake()
    {
        blockType = BlockType.BARRICADE;
    }
}
