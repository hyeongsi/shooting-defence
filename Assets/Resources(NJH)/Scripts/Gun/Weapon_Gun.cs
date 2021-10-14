using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Gun : MonoBehaviour
{
    Player_Manager playerManager;

    Camera camera;
    Cinemachine.CinemachineImpulseSource impulseSource;

    Animator animator;

    [Header("UI")]
    [SerializeField] Text bulletText;
    [SerializeField] Text reloadText;

    [Header("Particle System")]
    [SerializeField] ParticleSystem muzzleFlash;

    [SerializeField] Transform muzzleFlashPosition;

    [Header("Weapon Information")]
    [SerializeField] WeaponInfo weaponInfo;
    int maxBullet;
    int burstCount;

    [Header("Weapon Status")]
    public bool isShooting; // 키 입력
    public bool isReadyToShoot; // 다음 사격 준비
    public bool isburstShot;
    public bool isReloading;

    BulletProjectile bullet;

    private void Start()
    {
        // 총기 및 총알 정보 설정
        bullet = weaponInfo.bullet.GetComponent<BulletProjectile>();
        bullet.SetBulletInfo(weaponInfo.bulletSpeed, weaponInfo.damage);
        maxBullet = weaponInfo.magazineSize;
        isReadyToShoot = true;

        // 플레이어에서 가져오는 것들
        playerManager = GetComponentInParent<Player_Manager>();
        camera = playerManager.camera;
        bulletText = playerManager.bulletText;
        reloadText = playerManager.reloadText;

        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();   // 다른 방법으로 구현하면 안 쓸수도 있음
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bulletText.text = maxBullet.ToString();
    }

    public void WeaponKeyInput()
    {
        // 자동
        if (weaponInfo.isAutomatic == true) { isShooting = Input.GetButton("Fire1"); }
        //반자동
        else { isShooting = Input.GetButtonDown("Fire1"); }

        // 사격
        if (isReadyToShoot && isShooting && !isReloading && maxBullet > 0)
        {
            Weapon_Shoot();
        }

        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Weapon_Reload();
        }

        // 탄창의 모든 탄 소모 or 남은 탄 전체의 20퍼센트
        if (maxBullet <= Mathf.Round(weaponInfo.magazineSize * 0.2f))
        {
            if (maxBullet <= 0)
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

    void FiringBullet()
    {
        Vector3 bulletDir = playerManager.GetMousePosition();
        bullet.bulletDir = bulletDir;
        Instantiate(muzzleFlash, muzzleFlashPosition.position, Quaternion.LookRotation(muzzleFlashPosition.forward)); // 총구 화염 생성
        Instantiate(bullet, muzzleFlashPosition.position, Quaternion.LookRotation(muzzleFlashPosition.forward)); // 총알 생성
    }

    // 만약에 사용한다면 레이저 무기에 사용할 수도 있어서 남겨 놓음
    void HitScan()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // 총구화염 생성
        

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 적일 때
            if (hit.collider.gameObject.layer == 28)
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(weaponInfo.damage);
            }
        }
    }

    void Weapon_Shoot()
    {
        burstCount = weaponInfo.bulletsPerShot;
        StartCoroutine(Co_Shooting());
    }

    void Weapon_Reload()
    {
        if (isReloading || maxBullet == weaponInfo.magazineSize)
        {
            return;
        }
        StartCoroutine(Co_Reloading());
    }

    IEnumerator Co_Shooting()
    {
        isReadyToShoot = false;

        if (weaponInfo.weaponType == WeaponType.weaponTypeID.shotgun)    // 샷건
        {
            StartCoroutine(Co_PelletShot());

        }
        else    // 샷건말고 다른 총들(한번에 한발씩 나가는 총)
        {
            StartCoroutine(Co_NormalShot());
        }

        yield return new WaitForSeconds(weaponInfo.nextShotDelay);

        isReadyToShoot = true;
    }

    IEnumerator Co_NormalShot() // 슬라이드 후퇴 한 번에 한 발
    {
        isburstShot = false;

        while (burstCount > 0 && maxBullet > 0)
        {
            impulseSource.GenerateImpulse();

            FiringBullet();

            animator.SetTrigger("Shoot");
            playerManager.animator.CrossFade("Firing Rifle", 0f);

            maxBullet--;
            burstCount--;

            yield return new WaitForSeconds(weaponInfo.burstFireDelay);
        }

        isburstShot = true;
    }

    IEnumerator Co_PelletShot() // 슬라이드 후퇴 한 번에 여러발(샷건)
    {
        isburstShot = false;

        maxBullet--;

        animator.SetTrigger("Shoot");
        playerManager.animator.CrossFade("Firing Rifle", 0f);

        while (burstCount > 0)
        {
            burstCount--;
            Debug.Log("산탄" + burstCount);
            yield return new WaitForSeconds(weaponInfo.burstFireDelay);
        }

        isburstShot = true;
    }

    IEnumerator Co_Reloading()
    {
        isReloading = playerManager.reloadFlag = true;
        yield return new WaitForSeconds(weaponInfo.reloadTime);
        maxBullet = weaponInfo.magazineSize;
        isReloading = playerManager.reloadFlag = false;
    }
}
