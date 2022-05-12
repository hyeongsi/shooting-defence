using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Barricade
{
    protected TurretStaticData turretStaticData;

    public Transform rotationGunBody;
    protected float fireRate = 0.0f;

    protected LayerMask enemyLayerMask;
    public Enemy target = null;

    public GameObject bullet;
    public ParticleSystem myParticleSystem;

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
                if(shortestDistance >= distance)
                {
                    shortestDistance = distance;
                    target = enemyCollider.GetComponent<Enemy>();
                    return;
                }
            }
        }
    }

    private void RotationGunToEnemy()
    {
        Vector3 lotationVector = target.transform.position - rotationGunBody.position;
        rotationGunBody.rotation = Quaternion.Slerp(rotationGunBody.transform.rotation, Quaternion.LookRotation(lotationVector), turretStaticData.spinSpeed * Time.deltaTime);
    }

    private void AttackEnemy()
    {
        Vector3 lotationVector = target.transform.position - rotationGunBody.position;
        Quaternion fireRotation = Quaternion.LookRotation(lotationVector);

        if (Quaternion.Angle(rotationGunBody.rotation, fireRotation) < 5.0f)
        {
            fireRate -= Time.deltaTime;
            if(fireRate <= 0)
            {
                fireRate = AttackDelay;

                GameObject createBullet = Instantiate(bullet, rotationGunBody.position, Quaternion.LookRotation(rotationGunBody.forward)); // 총알 생성
                createBullet.GetComponent<TurretBullet>().SetEnemy(target);
   
                if(myParticleSystem != null)
                    myParticleSystem.Play();
                //target.TakeDamage(AttackDamage);
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
        if (GameManager.Instance == null || GameManager.Instance.IsPause)
            return;

        if (turretStaticData == null)
            return;

        if (target == null)
        {
            //rotationGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
            SearchEnemy();  // 가장 가까운 적을 찾음
        }
        else
        {
            if (target.isDie == true)
            {
                target = null;
                return;
            }
                
            RotationGunToEnemy();
            AttackEnemy();  // 적 존재하면 공격
                
            if (!TargetDisatanceCheck()) // 사거리 벗어나면
                target = null;
        }
    }
}
