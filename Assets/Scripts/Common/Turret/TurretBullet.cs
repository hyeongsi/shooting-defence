using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;

    public float bulletSpeed = 10;
    public float bulletDamage = 10;
    public float bulletDistance = 30f;
    public Vector3 distance;
    public Vector3 spawnPosition = new Vector3(0, 0, 0);
    public Enemy enemy;
    public bool setEnemy = false;

    private void Start()
    {
        spawnPosition = transform.position;
        Destroy(gameObject, 3f);
    }
    void Update()
    {
        if(!setEnemy)
            return;

        transform.Translate(transform.forward * bulletSpeed * Time.deltaTime, Space.World); // 총알 이동

        if (enemy == null)
            return;

        if (Vector3.Distance(spawnPosition, transform.position) > Vector3.Distance(spawnPosition, enemy.transform.position))
        {
            HitEffect();
            enemy.TakeDamage(bulletDamage);

            Destroy(gameObject);
        }
    }
    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        setEnemy = true;

    }

    void HitEffect()
    {
        Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-transform.forward));    // 피격 이펙트 생성
    }
}
