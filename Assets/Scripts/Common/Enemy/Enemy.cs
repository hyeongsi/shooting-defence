using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    LayerMask plyaerLayer;

    protected float hp;
    EnemyStaticData enemyStaticData;

    public float HP { get { return hp; }  set { hp = value; } }

    public virtual void Init(EnemyStaticData enemyStaticData)
    {
        this.enemyStaticData = enemyStaticData;
        hp = enemyStaticData.maxHp;
    }

    public virtual bool Attack(GameObject findObject)
    {
        if (findObject == null)
            return false;

        // 플레이어를 공격
        if (findObject.gameObject.layer == 29)
        {
            findObject.GetComponent<Player_Manager>().takeDamage();
        }

        // 포탑을 공격(포탑레이어를 추가하든 태그를 달든 해서 구별)

        return true;
    }

    public virtual void TakeDamage(float damage)    // 피격
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
            // 삭제 말고, 캐싱해서 메모리 아끼자,
            // 비활성화 시켜놓고 돌려쓰자, 오브젝트풀링
        }
    }

    public GameObject FindAttackObject()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, 5f, plyaerLayer);
        GameObject targetPlayer = null;

        Vector3 temp = new Vector3(0, 1, 0);    // 플레이어 위치 보정

        // 거리 비교
        if (detectedObjects.Length > 0)
        {
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
        }

        // 벽 뒤에 있어도 검출되는 문제 발생. 아마 위에서는 targetPlayer 지정해줬는데 여기서 다시 null로 바꾸면서 생기는 문제 같음
        if (Physics.Raycast(transform.position, (targetPlayer.transform.position + temp) - transform.position, out RaycastHit hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                targetPlayer = null;
            }
        }

        // 적(자신)을 기준으로 타겟 오브젝트의 좌표를 로컬 좌표로 변환
        if (targetPlayer != null)
        {
            Vector3 inverseTransform = transform.InverseTransformPoint(targetPlayer.transform.position);
            if (inverseTransform.z > 0) // 0보다 크면 앞에 있음
            {
                // 임시로 작성한 코드
                navMeshAgent.SetDestination(targetPlayer.transform.position);

                switch (enemyStaticData.attackType)
                {
                    case (int)AttackType.PLAYER:
                        // 공격 범위에 플레이어 있는지 검사하고 탐지된 플레이어 오브젝트 리턴
                        break;
                    case (int)AttackType.BARRICADE:
                        // 공격 범위에 장애물 있는지 검사하고 탐지된 터렛 오브젝트 리턴
                        break;
                    default:
                        // 공격 범위에 플레이어든 터렛이든 탐지된 오브젝트 리턴
                        break;
                }
            }
        }
        return null;
    }

    public virtual Vector3 FindAWay()
    {
        // 맵 정보를 토대로 이동해야할 방향 구해서 이동하기 
        // astar 알고리즘 통해서 이동방향 구하기, 이동가능하면 해당 방향 리턴
        // 이동방향을 벽으로 다 막지 못하도록 건축에서 제어 할거라 이동 못하는 곳은 없음
        return Vector3.zero;   
    }

    public virtual void Move(Vector3 direction)
    {
        // 방향 입력받고, 해당 방향으로 이동 처리 하도록 구현, 이동할 땐, 1칸씩 이동하도록 구현하기
        // 길막하면 
        // 이속, deltatime으로 이동 처리
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        plyaerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        FindAttackObject();

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
}
