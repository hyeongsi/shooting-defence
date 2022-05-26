using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    Weapon_Gun weapon = null;
    Player_Manager playerManager;
    GameObject canvas;
    int randomIndex;

    GameObject playerLife;
    Life life;

    private void Awake()
    {
        playerLife = GameObject.Find("Life");
        life = playerLife.GetComponent<Life>();
    }

    public void GetValues(Player_Manager playerManager, GameObject canvas, int randomIndex)
    {
        this.playerManager = playerManager;
        this.canvas = canvas;
        this.randomIndex = randomIndex;

        Debug.Log("플레이어: " + this.playerManager + " 캔버스: " + this.canvas + " 랜덤인덱스: " + this.randomIndex);
    }

    public void SelectOption()
    {
        weapon = playerManager.weapon;

        // 라운드 끝났을 때 timescale 0, 패널 active
        if (gameObject.activeInHierarchy == true)
        {
            if (randomIndex == 0)
            {
                FireRateUpgrade();
            }
            else if (randomIndex == 1)
            {
                DamageUpgrade();
            }
            else if (randomIndex == 2)
            {
                ReloadSpeedUpgrade();
            }
            else if (randomIndex == 3)
            {
                MagazineUpgrade();
            }
            else
            {
                life.LifeEarn();
            }
        }
        Cursor.visible = false;
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void FireRateUpgrade()
    {
        weapon.fireRate *= 0.8f;
    }

    void DamageUpgrade()
    {
        weapon.damage = (int)(weapon.damage * 1.2f);
    }

    void ReloadSpeedUpgrade()
    {
        weapon.reloadTime *= 0.8f;
    }

    void MagazineUpgrade()
    {
        weapon.magazineSize = (int)(weapon.magazineSize * 1.2f);
    }
}
