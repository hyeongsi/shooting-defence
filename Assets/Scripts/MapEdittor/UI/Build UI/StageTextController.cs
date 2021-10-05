using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTextController : MonoBehaviour
{
    public Text stageText;
    public Text stagePowerText;

    public void UpdateStageText()
    {
        if (MapManager.Instance.StageData != null)
            stageText.text = "Wave : " + MapManager.Instance.StageData.MaxWave;
    }

    public void UpdateStagePowerText()
    {
        if (MapManager.Instance.StageData != null)
            stagePowerText.text = "Stage Power : " + MapManager.Instance.StageData.StageEnemyPower;
    }
}
