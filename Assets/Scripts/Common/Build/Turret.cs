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

    public Transform rotationGunBody;
    private float spinSpeed = 270.0f;
    private float fireRate = 0.0f;

    private Quaternion lookRotation;
    private LayerMask enemyLayerMask;
    private Transform target = null;

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

    private void SearchEnemy()
    {
        Collider[] findEnemyCollider = Physics.OverlapSphere(transform.position, attackRange, enemyLayerMask);

        if(findEnemyCollider.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            foreach(Collider enemyCollider in findEnemyCollider)
            {
                float distance = Vector3.SqrMagnitude(transform.position - enemyCollider.transform.position);
                if(shortestDistance > distance)
                {
                    shortestDistance = distance;
                    target = enemyCollider.transform;
                }
            }
        }
    }

    private void RotationGunBody()
    {
        if (rotationGunBody == null)
            return;

        rotationGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
    }

    private void RotationGunToEnemy()
    {
        if (lookRotation != null)
        {
            lookRotation = Quaternion.LookRotation(target.transform.position);
            Vector3 rotationAngle = Quaternion.RotateTowards(
                    rotationGunBody.rotation,
                    lookRotation,
                    spinSpeed * Time.deltaTime).eulerAngles;
            rotationGunBody.rotation = Quaternion.Euler(0, rotationAngle.y, 0);
        }
    }

    private void AttackEnemy()
    {
        Quaternion fireRotation = Quaternion.Euler(0, Quaternion.LookRotation(target.position).eulerAngles.y, 0);
        if (Quaternion.Angle(rotationGunBody.rotation, fireRotation) < 5.0f)
        {
            fireRate -= Time.deltaTime;
            if(fireRate <= 0)
            {
                fireRate = attackDelay;
                Debug.Log("공격!!");
            }
        }
    }

    private bool TargetDisatanceCheck()
    {
        if (Vector3.Distance(transform.position, target.position) > attackRange)
            return false;

        return true;
    }

    private void Awake()
    {
        const string enemyLayerMaskName = "Enemy";

        blockType = BlockType.TURRET;
        enemyLayerMask = 1 << LayerMask.NameToLayer(enemyLayerMaskName);
    }

    private void Update()
    {
        // 게임 시작(스테이지 시작) 유무를 게임 매니저 통해서 받아서 시작 전엔 업데이트 하지 않도록 하기,

        if(target == null)
        {
            RotationGunBody();
            SearchEnemy();  // 가장 가까운 적을 찾음
        }
        else
        {
            // 조건문 추가, if(target.hp <= 0) {destroy(target); target = null }

            RotationGunToEnemy();
            AttackEnemy();  // 적 존재하면 공격
            if (!TargetDisatanceCheck()) // 사거리 벗어나면
                target = null;
        }
    }
}
