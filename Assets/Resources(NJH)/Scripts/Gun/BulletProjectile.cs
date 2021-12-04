using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] TrailRenderer bulletTrail;

    public float bulletSpeed;
    public float bulletDamage;

    public Vector3 bulletDir;

    private void Start()
    {
        Debug.DrawRay(transform.position, bulletDir, Color.green, 0.1f);
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.Translate(bulletDir.normalized * bulletSpeed * Time.deltaTime, Space.World); // 총알 이동
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Debug.Log("hit");

            Weapon_HitEffect();

            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(bulletDamage);

            Destroy(gameObject);
        }
    }

    public void SetBulletInfo(float speed, float damage)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
    }

    void Weapon_HitEffect()
    {
        Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-transform.forward));    // 피격 이펙트 생성
    }
}
