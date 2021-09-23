using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInformation")]
public class WeaponInfo : ScriptableObject
{
    public bool isAutomatic;
    public int weaponType;  // 0: 기본, 1: 점사, 2: 산탄
    public int damage;
    public int magazineSize;
    public int bulletsLeft;
    public int bulletsPerShot;
    public int burstBulletCount;
    public float reloadTime;
    public float nextShotDelay;
    public float burstFireDelay;
}

public class WeaponType
{
    enum weaponType
    {
        singleTab = 0,
        fullAuto = 1,
        burst = 2,
        burstFullAuto = 3,
        shotgun = 4,
    }
}
