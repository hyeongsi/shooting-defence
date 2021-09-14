using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelectEventAdder : MonoBehaviour
{
    MapGenerator mapGenerator;
    void Start()
    {
        mapGenerator = MapManager.Instance.GetComponent<MapGenerator>();

        for(int i = 0; i <transform.childCount; i ++)
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
