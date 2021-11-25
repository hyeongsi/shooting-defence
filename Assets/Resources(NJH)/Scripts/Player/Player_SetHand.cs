using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SetHand : MonoBehaviour
{
    Player_Manager player_Manager;

    void Start()
    {
        player_Manager = GetComponentInParent<Player_Manager>();
    }

    private void Update()
    {
        SetHand();
    }

    void SetHand()
    {
        if(player_Manager == null || player_Manager.weapon == null)
        {
            return;
        }

        if (gameObject.name == "GripPosition")
        {
            gameObject.transform.position = player_Manager.weapon.GetComponent<Weapon_Gun>().gripTransform.position;
        }
        if(gameObject.name == "HandGuardPosition")
        {
            gameObject.transform.position = player_Manager.weapon.GetComponent<Weapon_Gun>().handGuardTransform.position;
        }
    }
}
