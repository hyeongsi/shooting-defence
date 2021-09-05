using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : Block
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float mp;

    #region property
    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public float MP
    {
        get { return mp; }
        set { mp = value; }
    }
    #endregion

    private void Awake()
    {
        blockType = BlockType.BARRICADE;
    }
}
