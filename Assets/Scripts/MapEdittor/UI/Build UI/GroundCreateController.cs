using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GroundCreateController : MonoBehaviour
{
    [SerializeField]
    private InputField xInputField;
    [SerializeField]
    private InputField yInputField;
    [SerializeField]
    private MapGenerator mapGenerator;

    private int getX;
    private int getY;

    private Vector3 createPosition;

    public void InitEditBlock()
    {
        const int INIT_BLOCK_INDEX = 0;

        DestroyAllBlocks();
        Instantiate(MapManager.Instance.prefabList[(int)MapType.BLOCK][INIT_BLOCK_INDEX], Vector3.zero, Quaternion.Euler(Vector3.zero)).parent =
            mapGenerator.ParentGameObject[(int)MapType.BLOCK].transform;
    }

    public void DestroyAllBlocks()
    {
        if (mapGenerator == null)
            return;

        for (int i = 0; i < mapGenerator.ParentGameObject.Length; i ++)
        {
            for(int j = mapGenerator.ParentGameObject[i].transform.childCount-1; j >= 0; j-- )
            {
                Destroy(mapGenerator.ParentGameObject[i].transform.GetChild(j).gameObject);
            }
        }
    }

    public void CreateGound()
    {
        if(!int.TryParse(xInputField.text, out getX))
        {
            xInputField.text = string.Empty;
            return;
        }

        if (!int.TryParse(yInputField.text, out getY))
        {
            yInputField.text = string.Empty;
            return;
        }

        DestroyAllBlocks(); // 모든 블럭 삭제

        const int LOOP_COUNT = 2;
        bool isWhiteBlock = false;

        for (int y = 0; y < getY; y++)      // 지형 생성
        {
            for (int x = 0; x < getX; x++)
            {
                createPosition.z = y;
                createPosition.x = x;

                Instantiate(MapManager.Instance.prefabList[(int)MapType.BLOCK][1 + (isWhiteBlock ? 1 : 0)], createPosition, Quaternion.Euler(Vector3.zero)).parent =
                    mapGenerator.ParentGameObject[(int)MapType.BLOCK].transform;

                if ((x + 1) % LOOP_COUNT == 0)
                    isWhiteBlock = !isWhiteBlock;
            }

            if ((y + 1) % LOOP_COUNT == 0)
                isWhiteBlock = !isWhiteBlock;
        }

        xInputField.text = string.Empty;
        yInputField.text = string.Empty;
    }

    public void Cancel()
    {
        if(xInputField != null)
        {
            xInputField.text = string.Empty;
        }

        if (yInputField != null)
        {
            yInputField.text = string.Empty;
        }
    }
}
