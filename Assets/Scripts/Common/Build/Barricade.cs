using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    protected float hp = 1;
    private BarricadeStaticData barricadeStaticData;

    #region property
    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }
    #endregion

    public void Init(BarricadeStaticData barricadeStaticData)
    {
        this.barricadeStaticData = barricadeStaticData;
        hp = barricadeStaticData.maxHp;
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        hp = barricadeStaticData.maxHp;
    }
}
