using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Barricade
{
    protected TurretStaticData turretStaticData = new TurretStaticData();

    public Transform rotationGunBody;
    protected float fireRate = 0.0f;

    protected Quaternion lookRotation;
    protected LayerMask enemyLayerMask;
    protected Transform target = null;

    #region property
    public float AttackDamage
    {
        get { return turretStaticData.attackDamage; }
    }
    public float AttackRange
    {
        get { return turretStaticData.attackRange; }
    }
    public float AttackDelay
    {
        get { return turretStaticData.attackDelay; }
    }
    public float SpinSpeed
    {
        get { return turretStaticData.spinSpeed; }
    }
    #endregion

    public Turret(float hp, float attackDamage, float attackRange, float attackDelay, float spinSpeed, Vector3 blockSize)
    {
        this.hp = hp;
        TurretStaticData staticData = new TurretStaticData
        {
            attackDamage = attackDamage,
            attackRange = attackRange,
            attackDelay = attackDelay,
            spinSpeed = spinSpeed
        };

        turretStaticData = staticData;

        this.blockSize = blockSize;
    }

    public void Init(Turret turret)
    {
        hp = turret.hp;
        blockSize = turret.blockSize;

        turretStaticData = turret.turretStaticData;
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
                SpinSpeed * Time.deltaTime).eulerAngles;
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
                fireRate = AttackDelay;
                Debug.Log("공격!!");
            }
        }
    }

    private bool TargetDisatanceCheck()
    {
        if (Vector3.Distance(transform.position, target.position) > AttackRange)
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

    private void Update()
    {
        // 대기시간이나, 게임 시작하기 전엔 return 하도록 구현하기

        if (gameObject.layer != (int)LayerNumbering.BLOCK)  // 타워 생성 전 상태 -> 종료
            return;

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
