using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public int lifeLeft = 5;
    public Text lifeText;

    public void LifeEarn()
    {
        lifeLeft++;
        lifeText.text = lifeLeft.ToString();
    }

    public void LifeLoss()
    {
        if(lifeLeft == 0)
        {
            // 게임 종료
        }
        else
        {
            lifeLeft--;
            lifeText.text = lifeLeft.ToString();
        }
    }
}
