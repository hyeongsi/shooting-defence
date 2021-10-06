using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트 풀링으로 바꿔야함


public class BulletProjectile : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;

    public float bulletSpeed;
    public float bulletDamage;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Weapon_HitEffect(other.transform);

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

    void Weapon_HitEffect(Transform hitPoint)
    {
        Instantiate(hitEffect, hitPoint.position, Quaternion.identity);
    }
}
