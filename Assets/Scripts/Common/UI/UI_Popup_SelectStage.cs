using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_SelectStage : MonoBehaviour
{
    public Canvas CharacterCanvas;
    [SerializeField] UI_Popup_SelectCharacter uI_Popup_SelectCharacter;

    static int initCount = 0;
    private void Start()
    {
        initCount++;
        if (initCount >= 2)
        {
            return;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SelectCharacter(int stageIndex)
    {
        uI_Popup_SelectCharacter.stageIndex = stageIndex;
        CharacterCanvas.gameObject.SetActive(true);
    }

    /*
    public void StartStage(int stageIndex)
    {
        BlockManager.Instance.LoadAll();
        EnemyManager.Instance.LoadAll();
        ObjManager.Instance.LoadAll();

        GameManager.Instance.LoadScene(GameManager.PlayStates.IN_GAME, stageIndex);
    }
    */
}
