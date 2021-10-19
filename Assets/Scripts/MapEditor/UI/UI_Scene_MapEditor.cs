using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_MapEditor : UI_Scene
{
    private BlockManager.BlockName selectBlockIndex = BlockManager.BlockName.Block1_Gray;
    private InputField xInputField;
    private InputField yInputField;

    public BlockManager.BlockName SelectBlockIndex
    {
        set { selectBlockIndex = value; }
        get { return selectBlockIndex; }
    }

    private void Start()
    {
        Init();
        BlockManager.Instance.LoadAll();
    }

    public void SetSelectBlockIndex(int value)
    {
        if(value >= 0 && value < Enum.GetValues(typeof(BlockManager.BlockName)).Length)
        {
            SetSelectBlockIndex((BlockManager.BlockName)value);
        }
    }

    public void SetSelectBlockIndex(BlockManager.BlockName value)
    {
        selectBlockIndex = value;
    }

    public void SwitchActiveCavnas(Canvas canvas)
    {
        if (canvas == null)
            return;

        if(canvas.gameObject.activeSelf == true)
        {
            canvas.gameObject.SetActive(false);
        }
        else
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void SetInputFieldX(InputField xInputField)
    {
        this.xInputField = xInputField;
    }

    public void SetInputFieldY(InputField yInputField)
    {
        this.yInputField = yInputField;
    }

    public void GenerateMap()
    {
        const string GENERATE_MAP_PARENT_NAME = "Generated Map";

        int xValue = Int32.Parse(xInputField.text);
        int yValue = Int32.Parse(yInputField.text);

        if (xValue < 0)
            xValue = 0;

        if (yValue < 0)
            yValue = 0;

        GameObject generateBlockGameObject = BlockManager.Instance.GetBlock(selectBlockIndex);
        GameObject generateMapParent = GameObject.Find(GENERATE_MAP_PARENT_NAME);

        if (generateBlockGameObject == null)
            return;

        if (generateMapParent != null)
        {
            DestroyImmediate(generateMapParent);
        }

        generateMapParent = new GameObject(GENERATE_MAP_PARENT_NAME);

        for (int x = 0; x < xValue; x++)
        {
            for (int y = 0; y < yValue; y++)
            {
                Vector3 tilePosition = new Vector3(x, 0, -y);
                Transform newTile = Instantiate(generateBlockGameObject.transform, tilePosition, Quaternion.identity);
                newTile.parent = generateMapParent.transform;
            }
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
