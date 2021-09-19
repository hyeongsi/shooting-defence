using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelectEventAdder : MonoBehaviour
{
    public MapGenerator mapGenerator;
    const string INPUT_GROUND_PANEL_NAME = "Input Ground Size Select Panel";

    void Start()
    {
        for(int i = 0; i < transform.childCount; i ++)
        {
            if (transform.GetChild(i).name == INPUT_GROUND_PANEL_NAME)    // create ground button
                return;
            
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                int selectObjectType = new int();
                selectObjectType = i;
                int selectPrefab = new int();
                selectPrefab = j;

                transform.GetChild(i).GetChild(j).GetComponent<Button>().onClick.AddListener(() => mapGenerator.SelectObjctType = selectObjectType);
                transform.GetChild(i).GetChild(j).GetComponent<Button>().onClick.AddListener(() => mapGenerator.SelectPrefab = selectPrefab);
            }
        }
    }
}
