using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Barricade
{
    protected TurretStaticData turretStaticData;

    public Transform rotationGunBody;
    protected float fireRate = 0.0f;

    protected Quaternion lookRotation;
    protected LayerMask enemyLayerMask;
    protected Enemy target = null;

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

    public void Init(TurretStaticData turretStaticData)
    {
        this.turretStaticData = turretStaticData;

        hp = turretStaticData.maxHp;
    }

    private void SearchEnemy()
    {
        Collider[] findEnemyCollider = Physics.OverlapSphere(transform.position, turretStaticData.attackRange, enemyLayerMask);

        if(findEnemyCollider.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            foreach(Collider enemyCollider in findEnemyCollider)
            {
                float distance = Vector3.SqrMagnitude(transform.position - enemyCollider.transform.position);
                if(shortestDistance > distance)
                {
                    shortestDistance = distance;
                    target = enemyCollider.GetComponent<Enemy>();
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
                turretStaticData.spinSpeed * Time.deltaTime).eulerAngles;
        rotationGunBody.rotation = Quaternion.Euler(0, rotationAngle.y, 0);
    }

    private void AttackEnemy()
    {
        Quaternion fireRotation = Quaternion.Euler(0, Quaternion.LookRotation(target.transform.position).eulerAngles.y, 0);
 
        if (Quaternion.Angle(rotationGunBody.rotation, fireRotation) < 5.0f)
        {
            fireRate -= Time.deltaTime;
            if(fireRate <= 0)
            {
                fireRate = AttackDelay;
                target.TakeDamage(AttackDamage);
            }
        }
    }

    private bool TargetDisatanceCheck()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > turretStaticData.attackRange)
            return false;

        return true;
    }

    public void DestroyTurret()
    {
        Destroy(gameObject);    // this로 destory() 해버리면, 스크립트만 없어짐,
    }

    private void Awake()
    {
        const string enemyLayerMaskName = "Enemy";
       
        enemyLayerMask = 1 << LayerMask.NameToLayer(enemyLayerMaskName);
    }

    private void Update()
    {
        // 대기시간이나, 게임 시작하기 전엔 return 하도록 구현하기
        if (GameManager.Instance != null && GameManager.Instance.IsPause)
            return;

        if (gameObject.layer != (int)LayerNumbering.BLOCK)  // 타워 생성 전 상태 -> 종료
            return;

        if (turretStaticData == null)
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
