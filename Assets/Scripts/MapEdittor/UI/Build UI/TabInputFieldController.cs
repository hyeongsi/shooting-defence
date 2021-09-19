using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInputFieldController : MonoBehaviour
{
    [SerializeField]
    private List<InputField> inputFieldList;

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab))
            return;

        for (int i = 0; i < inputFieldList.Count; i++)
        {
            if (inputFieldList[i].isFocused != true)
                continue;

            if (i + 1 >= inputFieldList.Count)
                inputFieldList[0].Select();
            else
                inputFieldList[i + 1].Select();
        }
    }
}
