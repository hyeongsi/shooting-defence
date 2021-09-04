using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : MonoBehaviour
{
    Camera camera;
    Transform shootPoint;

    [Header("Weapon Info")]
    public bool isAutomatic;
    public int magazineSize;
    public int bulletsLeft;
    public int bulletsPerShot;
    public float reloadTime;
    public float nextShotDelay;

    [Header("Weapon Status")]
    public bool isShooting; // 키 입력
    public bool isReadyToShoot; // 다음 사격을 결정
    public bool isReloading;

    public void WeaponKeyInput()
    {
        // 자동
        if(isAutomatic == true) { isShooting = Input.GetButton("Fire1"); }
        //반자동
        else { isShooting = Input.GetButtonDown("Fire1"); }

        // 사격
        if(isReadyToShoot && isShooting && !isReloading && bulletsLeft > 0)
        {
            Weapon_Shoot();
        }
        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Weapon_Reload();
        }
    }

    void Weapon_Shoot()
    {
        bulletsLeft--;
        StartCoroutine(Co_Shooting());
    }

    void Weapon_Reload()
    {
        StartCoroutine(Co_Reloading());   
    }

    IEnumerator Co_Shooting()
    {
        Debug.Log("사격" + bulletsLeft);
        isReadyToShoot = false;
        yield return new WaitForSeconds(nextShotDelay);
        isReadyToShoot = true;
    }

    IEnumerator Co_Reloading()
    {
        Debug.Log("재장전");
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = magazineSize;
        isReloading = false;
    }
}
