using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Barricade
{
    protected float attackDamage;
    protected float attackRange;
    protected float attackDelay;
    protected float spinSpeed;

    public Transform rotationGunBody;
    protected float fireRate = 0.0f;

    protected Quaternion lookRotation;
    protected LayerMask enemyLayerMask;
    protected Transform target = null;

    #region property
    public float AttackDamage
    {
        get { return attackDamage; }
    }
    public float AttackRange
    {
        get { return attackRange; }
    }
    public float AttackDelay
    {
        get { return AttackDelay; }
    }
    public float SpinSpeed
    {
        get { return SpinSpeed; }
    }
    #endregion

    public Turret(float hp, ref float attackDamage, ref float attackRange, ref float attackDelay, ref float spinSpeed, ref Vector3 blockSize)
    {
        this.hp = hp;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        this.attackDelay = attackDelay;
        this.spinSpeed = spinSpeed;
        this.blockSize = blockSize;
    }

    public virtual GameObject Spawn()
    {
        //GameObject newTurretGameObject = Instantiate(prefab);
        //newTurretGameObject.GetComponent<Turret>().Init(this);

        //return newTurretGameObject;

        return default;
    }

    private void SearchEnemy()
    {
        Collider[] findEnemyCollider = Physics.OverlapSphere(transform.position, AttackRange, enemyLayerMask);

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

    private void RotationGunToEnemy()
    {
        lookRotation = Quaternion.LookRotation(target.transform.position);
        Vector3 rotationAngle = Quaternion.RotateTowards(
                rotationGunBody.rotation,
                lookRotation,
                spinSpeed * Time.deltaTime).eulerAngles;
        rotationGunBody.rotation = Quaternion.Euler(0, rotationAngle.y, 0);
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

    public void DestroyTurret()
    {
        Destroy(this);
    }

    private void Awake()
    {
        const string enemyLayerMaskName = "Enemy";

        blockType = BlockType.TURRET;
        enemyLayerMask = 1 << LayerMask.NameToLayer(enemyLayerMaskName);
    }

    public virtual void UpdateTurret()
    {
        // 게임 시작(스테이지 시작) 유무를 게임 매니저 통해서 받아서 시작 전엔 업데이트 하지 않도록 하기,

        if (target == null)
        {
            SearchEnemy();  // 가장 가까운 적을 찾음
        }
        else
        {
            RotationGunToEnemy();
            AttackEnemy();  // 적 존재하면 공격
            if (!TargetDisatanceCheck()) // 사거리 벗어나면
                target = null;
        }
    }
}
