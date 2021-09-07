using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Gun : MonoBehaviour
{
    Camera camera;
    Transform shootPoint;

    Cinemachine.CinemachineImpulseSource impulseSource;

    Animator animator;

    [SerializeField] Text bulletText;
    [SerializeField] Text reloadText;

    [Header("Weapon Info")]
    public int weaponType;  // 0: 기본, 1: 점사, 2: 산탄
    public bool isAutomatic;
    public int magazineSize;
    public int bulletsLeft;
    public int bulletsPerShot;
    public int burstBulletCount;
    public float reloadTime;
    public float nextShotDelay;
    public float burstFireDelay;

    [Header("Weapon Status")]
    public bool isShooting; // 키 입력
    public bool isReadyToShoot; // 다음 사격 준비
    public bool isburstShot;
    public bool isReloading;

    private void Start()
    {
        camera = GetComponentInParent<Player_Locomotion>().camera;
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        isReadyToShoot = true;

    }

    private void Update()
    {
        bulletText.text = bulletsLeft.ToString();
    }

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

        // 탄창의 모든 탄 소모 or 남은 탄 전체의 20퍼센트
        if(bulletsLeft <= Mathf.Round(magazineSize * 0.2f))
        {
            if(bulletsLeft <= 0)
            {
                reloadText.text = "재장전";
            }
            else
            {
                reloadText.text = "탄약 적음";
            }
            reloadText.gameObject.SetActive(true);
        }
        else
        {
            reloadText.gameObject.SetActive(false);
        }
    }

    void Weapon_Shoot()
    {
        burstBulletCount = bulletsPerShot;
        StartCoroutine(Co_Shooting());
    }

    void Weapon_Reload()
    {
        if (isReloading || bulletsLeft == magazineSize)
        {
            return;
        }
        StartCoroutine(Co_Reloading());
    }

    IEnumerator Co_Shooting()
    {
        isReadyToShoot = false;

        if (weaponType < 2)
        {
            StartCoroutine(Co_CommonShot());
        }
        else
        {
            StartCoroutine(Co_PelletShot());
        }

        yield return new WaitForSeconds(nextShotDelay);

        isReadyToShoot = true;
    }

    IEnumerator Co_CommonShot() // 슬라이드 후퇴 한 번에 한 발
    {
        isburstShot = false;
        
        while(burstBulletCount > 0 && bulletsLeft > 0)
        {
            impulseSource.GenerateImpulse(transform.forward);

            animator.SetTrigger("Shoot");

            bulletsLeft--;
            burstBulletCount--;
            
            yield return new WaitForSeconds(burstFireDelay);
        }

        isburstShot = true;
    }

    IEnumerator Co_PelletShot() // 슬라이드 후퇴 한 번에 여러발(샷건)
    {
        isburstShot = false;

        bulletsLeft--;

        animator.SetTrigger("Shoot");

        while (burstBulletCount > 0)
        {
            burstBulletCount--;
            Debug.Log("산탄" + burstBulletCount);
            yield return new WaitForSeconds(burstFireDelay);
        }

        isburstShot = true;
    }

    IEnumerator Co_Reloading()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = magazineSize;
        isReloading = false;
    }
}
