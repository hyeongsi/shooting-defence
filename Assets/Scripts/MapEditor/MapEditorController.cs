using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapEditorController : MonoBehaviour
{
    private static MapEditorController instance = null;
    private BlockManager.BlockName selectBlockIndex = BlockManager.BlockName.Block1_Gray;
    private int xValue;
    private int yValue;

    public int XValue 
    { 
        set 
        {
            if (value < 0) 
            {
                xValue = 0;
                return;
            } 
            xValue = value; 
        } 
    }
    public int YValue 
    { 
        set 
        {
            if (value < 0)
            {
                yValue = 0;
                return;
            }
            yValue = value;
        } 
    }

    private void Awake()
    {
        if (false == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        BlockManager.Instance.LoadAll();
        UIManager.Instance.EnrollUI(UIManager.PopUpUIEnums.MapEditPopUpUI);
    }

    public static MapEditorController Instance
    {
        get
        {
            if (instance == false)
                return null;

            return instance;
        }
    }

    public BlockManager.BlockName SelectBlockIndex
    {
        set { selectBlockIndex = value; }
        get { return selectBlockIndex; }
    }

    public void SetSelectBlockIndex(int value)
    {
        if (value >= 0 && value < Enum.GetValues(typeof(BlockManager.BlockName)).Length)
        {
            SetSelectBlockIndex((BlockManager.BlockName)value);
        }
    }

    public void SetSelectBlockIndex(BlockManager.BlockName value)
    {
        selectBlockIndex = value;
    }

    public void GenerateMap()
    {
        const string GENERATE_MAP_PARENT_NAME = "Generated Map";

        GameObject generateBlockGameObject = BlockManager.Instance.GetBlock(selectBlockIndex);
        GameObject generateMapParent = GameObject.Find(GENERATE_MAP_PARENT_NAME);

        if (generateBlockGameObject == false)
            return;

        if (generateMapParent != false)
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

    public void SwitchCanvas(Canvas canvas)
    {
        UIManager.Instance.SwitchPopUpUIActivation(canvas);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
