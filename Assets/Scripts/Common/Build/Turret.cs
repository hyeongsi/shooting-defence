using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Barricade
{
    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    protected float attackRange;        // 공격 범위를 수치로 표현하기가 애매해서 문제. 추후에 프리팹같은걸로 정하도록 할 예정
    [SerializeField]
    protected float attackDamge;

    #region property
    public float AttackDelay
    {
        get { return attackDelay; }
        set { attackDelay = value; }
    }

    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    public float AttackDamge
    {
        get { return attackDamge; }
        set { attackDamge = value; }
    }
    #endregion

    private void Awake()
    {
        blockType = BlockType.TURRET;
    }
}
