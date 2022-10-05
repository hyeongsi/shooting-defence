using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelection : MonoBehaviour
{
    [SerializeField] Sprite[] optionImages;
    [SerializeField] Button[] button;
    [SerializeField] UpgradeButton[] upgradeButtons;

    [SerializeField] GameObject canvas;

    Player_Manager playerManager;
    
    int randomIndex;

    private void Awake()
    {
        playerManager = FindObjectOfType<Player_Manager>();
    }

    public void Initialize()
    {
        Cursor.visible = true;
        canvas.gameObject.SetActive(true);

        playerManager.rotateLock = true;
        playerManager.triggerLock = true;

        for (int i = 0; i < button.Length; i++)
        {
            randomIndex = Random.Range(0, 5);
            
            button[i].image.sprite = optionImages[randomIndex];
            upgradeButtons[i].GetValues(playerManager, canvas, randomIndex);
            upgradeButtons[i] = button[i].GetComponent<UpgradeButton>();            
        }
    }
}
