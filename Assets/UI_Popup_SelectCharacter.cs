using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_SelectCharacter : MonoBehaviour
{
    [SerializeField] Player_SetValues playerSetValues;

    public int stageIndex;

    private void Start()
    {
        playerSetValues = FindObjectOfType<Player_SetValues>();
    }

    public void SelectGun(int gunIndex)
    {
        playerSetValues.gunNumber = gunIndex;
        playerSetValues.skinNumber = Random.Range(0, 8);
    }

    public void StartStage(int stageIndex)
    {
        MapStageManager.Instance.LoadAll();
        BlockManager.Instance.LoadAll();
        EnemyManager.Instance.LoadAll();
        TurretManager.Instance.LoadAll();
        ObjManager.Instance.LoadAll();

        if (this.stageIndex == GameManager.CUSTOM_MAP)
        {
            GameManager.Instance.LoadScene(GameManager.PlayStates.IN_GAME, GameManager.CUSTOM_MAP);
        }
        else
        {
            stageIndex = this.stageIndex;
            GameManager.Instance.LoadScene(GameManager.PlayStates.IN_GAME, stageIndex);
        }
    }
}
