using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform eye;
    Animator animator;
    NavMeshAgent navMeshAgent;
    LayerMask playerLayer;

    protected float hp = 1000; // 임시
    protected float damage = 10;
    EnemyStaticData enemyStaticData;

    bool isAttackalbe;
    bool damageFlag;

    public float HP { get { return hp; }  set { hp = value; } }
    public float DAMAGE { get { return damage; } set { damage = value; } }

    public virtual void Init(EnemyStaticData enemyStaticData)
    {
        this.enemyStaticData = enemyStaticData;
        hp = enemyStaticData.maxHp;
    }

    public virtual void Attack(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        float targetDistance = Vector3.SqrMagnitude(transform.position - targetObject.transform.position);

        if (targetDistance <= 2.5f && isAttackalbe == true) // 임시로 
        {
            StartCoroutine(Co_Attack(2f, targetObject)); // 임시로 딜레이 넣음
        }
    }

    IEnumerator Co_Attack(float attackDelay, GameObject targetObject)
    {
        isAttackalbe = false;

        float delay = attackDelay;
        animator.CrossFade("Attack", 0.3f);

        // 여기에 데미지 주는 코드 입력
        targetObject.GetComponent<Player_Locomotion>().TakeDamage(damage);

        while (delay > 0)
        {
            delay -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        isAttackalbe = true;
    }

    public virtual void TakeDamage(float damage)    // 피격
    {
        hp -= damage;
        animator.CrossFade("Hit", 0f);

        if (hp <= 0)
        {
            Destroy(gameObject, 3f);
            // 삭제 말고, 캐싱해서 메모리 아끼자,
            // 비활성화 시켜놓고 돌려쓰자, 오브젝트풀링
        }
    }

    public GameObject FindAttackObject()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, 5f, playerLayer);
        GameObject targetPlayer = null;
        GameObject targetBarricade = null;

        Vector3 playerPosCorrection = new Vector3(0, 1, 0);    // 플레이어 위치 보정

        // 하나 이상 검출 되었을 때
        if (detectedObjects.Length > 0)
        {
            // 검출 된 것마다 거리 비교
            float shortestDistance = Mathf.Infinity;
            foreach (Collider collider in detectedObjects)
            {
                float distance = Vector3.SqrMagnitude(transform.position - collider.transform.position);
                if (shortestDistance > distance)
                {
                    shortestDistance = distance;
                    targetPlayer = collider.gameObject;
                }
            }

            // 적(자신)을 기준으로 타겟 오브젝트의 좌표를 로컬 좌표로 변환
            Vector3 inverseTransform = transform.InverseTransformPoint(targetPlayer.transform.position);
            if (inverseTransform.z > 0) // 0보다 크면 앞에 있음
            {
                // 레이캐스트로 가려졌는지 확인 후 가려졌으면 목표물을 다시 null로 바꿈
                if (Physics.Raycast(eye.position, (targetPlayer.transform.position + playerPosCorrection) - eye.position, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                    {
                        targetPlayer = null;
                    }
                }
            }
        }

        //if (targetPlayer != null)
        //{
        //    switch (enemyStaticData.attackType)
        //    {
        //        case (int)AttackType.PLAYER:
        //            // 공격 범위에 플레이어 있는지 검사하고 탐지된 플레이어 오브젝트 리턴
        //            break;
        //        case (int)AttackType.BARRICADE:
        //            // 공격 범위에 장애물 있는지 검사하고 탐지된 터렛 오브젝트 리턴
        //            break;
        //        default:
        //            // 공격 범위에 플레이어든 터렛이든 탐지된 오브젝트 리턴
        //            break;
        //    }
        //}

        // enemyStaticData 제대로 사용하면 수정하기
        if (targetPlayer == null)
            return null;
        else
            return targetPlayer;
    }

    public virtual Vector3 FindAWay()
    {
        // 맵 정보를 토대로 이동해야할 방향 구해서 이동하기 
        // astar 알고리즘 통해서 이동방향 구하기, 이동가능하면 해당 방향 리턴
        // 이동방향을 벽으로 다 막지 못하도록 건축에서 제어 할거라 이동 못하는 곳은 없음
        return Vector3.zero;   
    }

    public virtual void Move(GameObject targetObject)
    {
        // 방향 입력받고, 해당 방향으로 이동 처리 하도록 구현, 이동할 땐, 1칸씩 이동하도록 구현하기
        // 길막하면 
        // 이속, deltatime으로 이동 처리

        if(targetObject != null) // 이동
        {
            navMeshAgent.SetDestination(targetObject.transform.position);
        }

        if(navMeshAgent.velocity == Vector3.zero)
        {
           animator.SetBool("isMove", false);
        }
        else
        {
            animator.SetBool("isMove", true);
        }
    }

    private void Awake()
    {
        eye = GetComponentInChildren<EyePosition>().transform;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        playerLayer = 1 << LayerMask.NameToLayer("Player");

        isAttackalbe = true;
    }

    private void Update()
    {
        Move(FindAttackObject());
        Attack(FindAttackObject());

        Debug.Log(damageFlag);

        if (GameManager.Instance == null || GameManager.Instance.IsPause)
            return;
        
        // if(!Attack(FindAttackObject()));     // 공격 못했따면 이동하도록 처리
        //  FindAWay()?Move();   // FindAWay()로 길 찾아서 Move로 이동하기   
    }

    public enum AttackType
    {
        NONE = 0,       // 공격 안하고 무시
        PLAYER = 1,     // 플레이어 만 공격
        BARRICADE = 2      // 터렛만 공격
    }

    public void DamageOn()
    {
        damageFlag = true;
    }
    public void DamageOff()
    {
        damageFlag = false;
    }

    private void OnMouseEnter()
    {
        // 마우스가 들어오면 강조 표시 하기
        // 십자선을 고정
    }
}
