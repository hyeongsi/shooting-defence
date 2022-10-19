using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] AudioClip explosionSound;

    public Transform targetPoint;
    int checkPointNumber = 0;

    bool moveFlag = true;

    Animator animator;

    public float hp = 100;   // 임시
    public float speed = 4;  // 임시
    EnemyStaticData enemyStaticData;
    public bool isDie = false;
    GameObject moneyBoxObject;
    GameObject playerLife;
    Life life;

    public float HP { get { return hp; }  set { hp = value; } }
    public float SPEED { get { return speed; } set { speed = value; } }

    public virtual void Init(EnemyStaticData enemyStaticData)
    {
        this.enemyStaticData = enemyStaticData;

        Debug.Log("에너미 데이터 " + enemyStaticData);
        moneyBoxObject = GameObject.Find("MoneyBox");
        playerLife = GameObject.Find("Life");
        life = playerLife.GetComponent<Life>();

        hp = enemyStaticData.maxHp;
        speed = enemyStaticData.moveSpeed;
    }

    public virtual void TakeDamage(float damage)    // 피격
    {
        hp -= damage;
        animator.CrossFade("Hit", 0f);

        if (hp <= 0)
        {
            moveFlag = false;

            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            animator.applyRootMotion = true;
            animator.SetTrigger("Die");
            isDie = true;

            if (moneyBoxObject != null)
            {
                MoneyBox moneyBox = moneyBoxObject.GetComponent<MoneyBox>();
                moneyBox.SetMoneyText(moneyBox.money + Random.Range(3,10));
            }

            Destroy(gameObject, 2.5f);
            // 삭제 말고, 캐싱해서 메모리 아끼자,
            // 비활성화 시켜놓고 돌려쓰자, 오브젝트풀링
        }
    }

    public virtual void Move()
    {
        if(enemyStaticData == null)
        {
            enemyStaticData = new EnemyStaticData(HP, 3);
        }

        Vector3 moveDirection = targetPoint.position - transform.position;
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);

        transform.forward = moveDirection;

        animator.SetBool("isMove", true);

        if (CheckPoint.checkPoint.Length <= 0)
            return;

        if(Vector3.Distance(transform.position, targetPoint.position) <= 0.1f)
        {
            checkPointNumber++;

            if(checkPointNumber >= CheckPoint.checkPoint.Length) // 마지막 체크포인트에 도착
            {
                ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.LookRotation(Vector3.up));  // 폭발 이펙트 실행
                SoundManager.instance.PlaySound("Enemy Explosion", explosionSound, 1f, transform.position);
                DestroyImmediate(gameObject);

                // 라이프 감소
                life.LifeLoss();
            }
            else
            {
                targetPoint = CheckPoint.checkPoint[checkPointNumber];
            }
            
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        targetPoint = CheckPoint.checkPoint[checkPointNumber];
    }

    private void Update()
    {
        if(moveFlag == true)
        {
            Move();
        }

        if (GameManager.Instance == null || GameManager.Instance.IsPause)
            return;
    }
}