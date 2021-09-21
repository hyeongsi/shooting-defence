using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int hp;
    private float attackDamage;
    private float attackRange;
    private float attackDelay;
    private AttackType attackType = AttackType.MELEE;   
    private EnemyType enemyType = EnemyType.TEST_TYPE;   // 스크립트 마다 다르도록 할 예정, 이걸로 몬스터 종류 구분
    
    public void Attack()
    {
        // 플레이어나 타워 공격하도록 하기, 
    }

    public void TakeDamage(int damage)    // 피격
    {
        hp -= damage;

        if (hp < 1)
            DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        Destroy(this);
    }

    public enum AttackType
    {
        MELEE = 0,  // 근접
        RANGED = 1, // 원거리
        MAGIC = 2,  // 마법
    }

    public enum EnemyType
    {
        TEST_TYPE = 0,
    }
}
