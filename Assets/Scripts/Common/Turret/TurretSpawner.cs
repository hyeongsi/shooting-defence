using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public TurretSpawnUI turretSpawnUI;
    private bool isPrintTurretSpawnUI = false;

    void Start()
    {
        turretSpawnUI = GameObject.Find("TurretSpawnUI").GetComponent<TurretSpawnUI>();
    }

    public void CreateTurret(int i)
    {
        //i번째 터렛 생성하면 됨 
    }

    private void OnTriggerEnter(Collider other)
    {
        // 추후에 가시성 위해서 터렛스포너 색 변경하는거 추가해도 괜찮음

        if (turretSpawnUI == null)
            return;

        turretSpawnUI.PrintInitUI();
    }

    private void OnTriggerExit(Collider other)
    {
        if (turretSpawnUI == null)
            return;

        turretSpawnUI.DeleteUI();
    }
    void Update()
    {
        if (turretSpawnUI == null)
            return;

        // UI 출력 + F 키 : 시간 멈추고 UI 선택 진행 해야 할듯
        if(!isPrintTurretSpawnUI)
        {
            return; 
        }

        for (int i = 0; i < TurretManager.Instance.GetTurretSize(); i++)
        {
            if (Input.GetKeyDown((KeyCode)(48 + i)))    // 48 : KeyCode.Alpha0 의미
            {
                CreateTurret(i);
            }
        }
      
    }
}
