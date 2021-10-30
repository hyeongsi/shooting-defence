using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnEnemyInfo
{
    public List<int> spawnEnemyList = new List<int>();
    public List<float> waitTimeBeforeSpawnList = new List<float>();
    public List<float> spawnDelayList = new List<float>();
    public List<int> spawnCountList = new List<int>();
}

public class MapEditorController : MonoBehaviour
{
    private static MapEditorController instance = null;
    private BlockManager.BlockName selectBlockIndex = BlockManager.BlockName.Block1_Gray;
    private EnemyManager.EnemyName selectEnemyIndex = EnemyManager.EnemyName.zombie1;
    private int xValue;
    private int yValue;

    private List<SpawnEnemyInfo> spawnEnemyInfoList = new List<SpawnEnemyInfo>();

    public delegate void SetSelectEnemyIndexDelegate();
    public SetSelectEnemyIndexDelegate setSelectEnemyIndexDelegate;

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
        EnemyManager.Instance.LoadAll();
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

    #region SelectIndexProperty
    public BlockManager.BlockName SelectBlockIndex
    {
        set { selectBlockIndex = value; }
        get { return selectBlockIndex; }
    }
    public EnemyManager.EnemyName SelectEnemyIndex
    {
        set { selectEnemyIndex = value; }
        get { return selectEnemyIndex; }
    }
    #endregion

    #region SelectIndexSetMethod
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

    public void SetSelectEnemyIndex(int value)
    {
        if (value >= 0 && value < Enum.GetValues(typeof(EnemyManager.EnemyName)).Length)
        {
            SetSelectEnemyIndex((EnemyManager.EnemyName)value);
        }
    }
    public void SetSelectEnemyIndex(EnemyManager.EnemyName value)
    {
        selectEnemyIndex = value;
        setSelectEnemyIndexDelegate?.Invoke();
    }
    #endregion

    public void InitWaveData()
    {
        InitSpawnEnemyInfoList();
    }

    #region SetSpawnEnemyInfoList
    public List<SpawnEnemyInfo> GetSpawnEnemyInfoList()
    {
        return spawnEnemyInfoList;
    }

    public void InitSpawnEnemyInfoList()
    {
        spawnEnemyInfoList.Clear();

        SpawnEnemyInfo list = new SpawnEnemyInfo();
        spawnEnemyInfoList.Add(list);
    }

    public void SetSpawnEnemyInfoList(int waveCount)
    {
        if (spawnEnemyInfoList.Count > waveCount)
        {
            spawnEnemyInfoList.RemoveRange(waveCount, spawnEnemyInfoList.Count - waveCount);
        }
        else if (spawnEnemyInfoList.Count < waveCount)
        {
            int spawnEnemyInfoListCount = spawnEnemyInfoList.Count;
            for (int i = 0; i < waveCount - spawnEnemyInfoListCount; i++)
            {
                SpawnEnemyInfo spawnEnemyInfo = new SpawnEnemyInfo();
                spawnEnemyInfoList.Add(spawnEnemyInfo);
            }
        }
    }
    #endregion

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

    public void ToggleCanvas(Canvas canvas) // UI_POPUP을 상속받지 않은 캔버스 전용 on,off 메소드
    {
        UIManager.Instance.ToggleCanvas(canvas);
    }

    public void SwitchCanvas(Canvas canvas)
    {
        UIManager.Instance.SwitchPopUpUIActivation(canvas);
    }

    public void CloseCanvas(Canvas canvas)
    {
        UIManager.Instance.ClosePopupUI(canvas);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
