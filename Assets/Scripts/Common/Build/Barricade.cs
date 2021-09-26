using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    protected float hp = 1;

    #region property
    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }
    #endregion
}
