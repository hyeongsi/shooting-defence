using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Locomotion : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Transform destinationTransform;
    [SerializeField] NavMeshAgent agent;

    [Header("Enemy Stats")]
    public int hp;
    public int moveSpeed;
    public int attackDamage;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(destinationTransform.position);
        Enemy_Die();
    }

    public void Enemy_Hit(int damage)
    {
        if(hp > 0)
        {
            hp -= damage;

            if(animator == null)
            {
                return;
            }
            // 피격 애니메이션, 사운드 재생
            animator.CrossFade("Hit Reaction", 0.1f);
        }
    }

    void Enemy_Die()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
