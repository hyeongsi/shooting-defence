using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public TurretSpawnUI turretSpawnUI;

    void Start()
    {
        turretSpawnUI = GameObject.Find("TurretSpawnUI").GetComponent<TurretSpawnUI>();
    }

    public void CreateTurret(int i)
    {
        GameObject createTurretGameObject = null;

        // 터렛 추가하면 여기에 터렛 등록하면 됨
        switch(i-1)
        {
            case (int)TurretManager.TurretName.rifleTurret:
                createTurretGameObject = TurretManager.Instance.InstantiateTurret(TurretManager.TurretName.rifleTurret);
                break;
            case (int)TurretManager.TurretName.sniperTurret:
                createTurretGameObject = TurretManager.Instance.InstantiateTurret(TurretManager.TurretName.sniperTurret);
                break;
        }

        if(createTurretGameObject == null)
        {
            return; 
        }

        createTurretGameObject.transform.position = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 추후에 가시성 위해서 터렛스포너 색 변경하는거 추가해도 괜찮음

        if (turretSpawnUI == null)
            return;

        turretSpawnUI.PrintInitUI();
    }

    private void OnTriggerStay(Collider other)
    {
        // 무식하게 터렛 추가하면 여기 버튼 추가해서 등록하면 됨
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateTurret(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateTurret(2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (turretSpawnUI == null)
            return;

        turretSpawnUI.DeleteUI();
    }
    void Update()
    {
        
      
    }
}
