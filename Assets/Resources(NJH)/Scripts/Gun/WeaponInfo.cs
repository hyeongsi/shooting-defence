using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInformation")]
public class WeaponInfo : ScriptableObject
{
    public bool isAutomatic;
    public WeaponType.weaponTypeID weaponType;  // 0: 단발, 1: 연발, 2: 점사, 3: 점사연발, 4: 산탄
    public int damage;
    public float bulletSpeed;
    public int magazineSize;
    public int bulletsLeft;
    public int bulletsPerShot;
    public int burstBulletCount;
    public float reloadTime;
    public float nextShotDelay;
    public float burstFireDelay;

    public GameObject bullet;
    
}

public class WeaponType
{
    public enum weaponTypeID
    {
        singleTab = 0,
        fullAuto = 1,
        burst = 2,
        burstFullAuto = 3,
        shotgun = 4,
    }
}
