using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelectEventAdder : MonoBehaviour
{
    public MapGenerator mapGenerator;
    private const int SELECT_CHILD = 3;

    void Start()
    {
        for(int i = 0; i < SELECT_CHILD; i ++)
        {
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
