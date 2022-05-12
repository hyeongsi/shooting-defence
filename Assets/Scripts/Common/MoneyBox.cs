using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBox : MonoBehaviour
{
    public Text moneyText;
    public int money = 10;

    public void SetMoneyText(int dollar)
    {
        moneyText.text = ": " + ((int)dollar).ToString() + " $";
        money = (int)dollar;
    }
}
