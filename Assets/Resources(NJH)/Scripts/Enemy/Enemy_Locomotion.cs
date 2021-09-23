using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Locomotion : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Enemy Stats")]
    public int hp;
    public int moveSpeed;
    public int attackDamage;

    private void FixedUpdate()
    {
        Enemy_Die();
    }

    public void Enemy_Hit(int damage)
    {
        if(hp > 0)
        {
            hp -= damage;

            // 피격 애니메이션, 사운드 재생
            animator.CrossFade("Hit Reaction", 0.1f);
        }
    }

    void Enemy_Die()
    {
        if(hp <= 0)
        {
            Destroy(gameObject, 1f);
        }
    }
}
