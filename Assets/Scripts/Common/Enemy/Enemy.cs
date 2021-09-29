using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
        // 여기에 목표물 탐지 코드 작성


        switch(enemyStaticData.attackType)
        {
            case (int)AttackType.PLAYER:
                // 공격 범위에 플레이어 있는지 검사하고 탐지된 플레이어 오브젝트 리턴
                break;
            case (int)AttackType.TURRET:
                // 공격 범위에 터렛 있는지 검사하고 탐지된 터렛 오브젝트 리턴
                break;
            default:
                // 공격 범위에 플레이어든 터렛이든 탐지된 오브젝트 리턴
                return null;
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

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsPause)
            return;

        // if(!Attack(FindAttackObject()));     // 공격 못했따면 이동하도록 처리
        //  FindAWay()?Move();   // FindAWay()로 길 찾아서 Move로 이동하기   
    }

    public enum AttackType
    {
        NONE = 0,       // 공격 안하고 무시
        PLAYER = 1,     // 플레이어 만 공격
        TURRET = 2      // 터렛만 공격
    }
}
