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

    private int getX;
    private int getY;

    private Vector3 createPosition;

    public void InitEditBlock()
    {
        const int INIT_BLOCK_INDEX = 0;

        DestroyAllBlocks();
        GameObject[] blockPrefabArray = BlockManager.Instance.BlockPrefabArray;
        if (blockPrefabArray.Length >= 1)
        {
            Instantiate(blockPrefabArray[INIT_BLOCK_INDEX].transform, Vector3.zero, Quaternion.Euler(Vector3.zero)).parent =
            MapManager.Instance.ParentGameObject[(int)MapType.BLOCK].transform;
        }
    }

    public void DestroyAllBlocks()
    {
        GameObject[] parentGameObject = MapManager.Instance.ParentGameObject;

        for (int i = 0; i < parentGameObject.Length; i ++)
        {
            for(int j = parentGameObject[i].transform.childCount-1; j >= 0; j-- )
            {
                Destroy(parentGameObject[i].transform.GetChild(j).gameObject);
            }
        }
    }

    public void CreateGround()
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

        try 
        {
            for (int y = 0; y < getY; y++)      // 지형 생성
            {
                for (int x = 0; x < getX; x++)
                {
                    createPosition.z = y;
                    createPosition.x = x;

                    Instantiate(BlockManager.Instance.BlockPrefabArray[1 + (isWhiteBlock ? 1 : 0)].transform, createPosition, Quaternion.Euler(Vector3.zero)).parent =
                    MapManager.Instance.ParentGameObject[(int)MapType.BLOCK].transform;

                    if ((x + 1) % LOOP_COUNT == 0)
                        isWhiteBlock = !isWhiteBlock;
                }

                if ((y + 1) % LOOP_COUNT == 0)
                    isWhiteBlock = !isWhiteBlock;
            }
        }
        catch
        {
            Debug.Log("GroundCreateController, CreateGound(), 76, PefabManager BlockPrefabArray out of range error");
        }

        xInputField.text = string.Empty;
        yInputField.text = string.Empty;
    }
}
