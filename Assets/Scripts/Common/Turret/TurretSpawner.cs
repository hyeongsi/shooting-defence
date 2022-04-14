using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public Player_Manager playerManager;
    public TurretSpawnUI turretSpawnUI;
    public bool isSpawn = false;
    public bool isEnter = false;
    public bool isMapEditor = false;

    void Start()
    {
        GameObject findTurretSpawnUiGameObject = GameObject.Find("TurretSpawnUI");
        if(findTurretSpawnUiGameObject == null)
        {
            isMapEditor = true;
            return;
        }
        turretSpawnUI = findTurretSpawnUiGameObject.GetComponent<TurretSpawnUI>();

        GameObject findPlayerManagerGameObject = GameObject.Find("Player");
        if (findPlayerManagerGameObject == null)
        {
            isMapEditor = true;
            return;
        }
        playerManager = findPlayerManagerGameObject.GetComponent<Player_Manager>();
    }

    public GameObject SpawnTurret(TurretManager.TurretName turretName)
    {
        GameObject collisionGameObject = playerManager.GetTurretSpawnerCollision();

        if(collisionGameObject == gameObject)
        {
            isSpawn = true;
            return TurretManager.Instance.InstantiateTurret(turretName);
        }
        else
        {
            return null;
        }
    }

    public void CreateTurret(int i)
    {
        GameObject createTurretGameObject = null;

        // 터렛 추가하면 여기에 터렛 등록하면 됨
        switch(i-1)
        {
            case (int)TurretManager.TurretName.rifleTurret:
                createTurretGameObject = SpawnTurret(TurretManager.TurretName.rifleTurret);
                break;
            case (int)TurretManager.TurretName.sniperTurret:
                createTurretGameObject = SpawnTurret(TurretManager.TurretName.sniperTurret);
                break;
            default:
                isSpawn = false;
                break;
        }

        if(createTurretGameObject == null)
        {
            return;
        }

        if(isSpawn == true)
        {
            const string OBJECT_NAME = "Map_Objects";

            GameObject parentGameObject = GameObject.Find(OBJECT_NAME);

            if(parentGameObject != null)
                createTurretGameObject.transform.parent = parentGameObject.transform;
            else
            {
                parentGameObject = new GameObject(OBJECT_NAME);
                createTurretGameObject.transform.parent = parentGameObject.transform;
            }
        }

        createTurretGameObject.transform.position = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 추후에 가시성 위해서 터렛스포너 색 변경하는거 추가해도 괜찮음
        if (turretSpawnUI == null)
            return;

        isEnter = true;

        if (isSpawn == true)
            return;

        turretSpawnUI.PrintInitUI();
    }

    private void OnTriggerExit(Collider other)
    {
        if (turretSpawnUI == null)
            return;

        isEnter = false;

        turretSpawnUI.DeleteUI();
    }
    void Update()
    {
        if (isMapEditor == true)
            return;

        if (isSpawn == true)
            return;

        if (isEnter == true)
        {
            // 무식하게 터렛 추가하면 여기 버튼 추가해서 등록하면 됨
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                isEnter = false;
                turretSpawnUI.DeleteUI();
                CreateTurret(1);
                
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                isEnter = false;
                turretSpawnUI.DeleteUI();
                CreateTurret(2);
                
            }
        }
    }
}
