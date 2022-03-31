using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurretSpawnUI : MonoBehaviour
{
    public GameObject turretSpawnUI;

    public void PrintInitUI()
    {
        turretSpawnUI.SetActive(true);
    }

    public void DeleteUI()
    {
        turretSpawnUI.SetActive(false);
    }
}
