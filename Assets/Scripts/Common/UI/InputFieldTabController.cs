using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldTabController : MonoBehaviour
{
    public List<InputField> inputFieldList;

    public void Update()
    {
        for(int i = 0; i < inputFieldList.Count; i ++)
        {
            if (inputFieldList[i].isFocused != true)
                continue;

            if(Input.GetKeyDown(KeyCode.Tab))
            {
                if(i == inputFieldList.Count-1)
                {
                    inputFieldList[0].Select();
                }
                else
                {
                    inputFieldList[i + 1].Select();
                }

                return;
            }
        }
    }
}
