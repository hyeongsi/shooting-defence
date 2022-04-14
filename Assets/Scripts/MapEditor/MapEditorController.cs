using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Ookii.Dialogs;
using System.Windows.Forms;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SpawnEnemyInfo
{
    [Tooltip("소환될 적 리스트")]
    public List<int> spawnEnemyList = new List<int>();                  // 소환될 적 리스트
    [Tooltip("스폰 전 대기시간")]
    public List<float> waitTimeBeforeSpawnList = new List<float>();     // 스폰 전 대기시간
    [Tooltip("스폰 딜레이")]
    public List<float> spawnDelayList = new List<float>();              // 스폰 딜레이
    [Tooltip("소환될 적의 수")]
    public List<int> spawnCountList = new List<int>();                  // 소환될 적의 수
}

public class MapEditorController : MonoBehaviour
{
    private static MapEditorController instance = null;
    private BlockManager.BlockName selectBlockIndex = BlockManager.BlockName.Block1_Gray;
    private EnemyManager.EnemyName selectEnemyIndex = EnemyManager.EnemyName.zom1;
    private ObjManager.ObjName selectObjectIndex = ObjManager.ObjName.Water_Tower;
    private bool isSelectingObject = false;
    [HideInInspector]
    public GameObject previewObject = null;
    private int spawnObjectAngle = 0;

    private int xValue;
    private int yValue;

    private List<SpawnEnemyInfo> spawnEnemyInfoList = new List<SpawnEnemyInfo>();   // 스테이지, 스폰정보 저장
    
    public delegate void SetSelectEnemyIndexDelegate();
    public SetSelectEnemyIndexDelegate setSelectEnemyIndexDelegate;

    private CustomTileMap customTileMap = new CustomTileMap();    // 생성된 맵의 정보 저장
    private bool isEditGuideLine = false;

    VistaOpenFileDialog openFileDialog;
    VistaSaveFileDialog saveFileDialog;

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
        TurretManager.Instance.LoadAll();
        ObjManager.Instance.LoadAll();
        UIManager.Instance.EnrollUI(UIManager.PopUpUIEnums.MapEditPopUpUI);

        openFileDialog = new VistaOpenFileDialog();
        openFileDialog.Filter = "Json (*.json) |*.json";
        openFileDialog.DefaultExt = "*.json";
        saveFileDialog = new VistaSaveFileDialog();
        saveFileDialog.Filter = "Json (*.json) |*.json";
        saveFileDialog.DefaultExt = "*.json";
    }

    private void Update()
    {
        if (UIManager.Instance.PopupList.Count != 0)
            return;

        CreateCustomMapGameObject();
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
    #region property
    public bool IsSelectingObject { get { return isSelectingObject; } }
    public bool IsEditGuideLine { get { return isEditGuideLine; } }
    public int SpawnObjectAngle { get { return spawnObjectAngle; } }
    #endregion

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

    public void SetSelectObjectIndex(int value)
    {
        if (value >= 0 && value < Enum.GetValues(typeof(ObjManager.ObjName)).Length)
        {
            SetSelectObjectIndex((ObjManager.ObjName)value);
        }
    }
    public void SetSelectObjectIndex(ObjManager.ObjName value)
    {
        selectObjectIndex = value;
    }
    #endregion
    #region SelectObjectProperty
    public ObjManager.ObjName SelectObjectIndex { get { return selectObjectIndex; }  set { selectObjectIndex = value; } }
    #endregion

    #region CustomTileMapProperty
    public CustomTileMap GetCustomTileMap { get { return customTileMap; } }
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

    #region UpdateMap
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

        customTileMap.ClearBlockList(); // 생성된 블럭들 모두 초기화

        generateMapParent = new GameObject(GENERATE_MAP_PARENT_NAME);

        for (int x = 0; x < xValue; x++)
        {
            for (int y = 0; y < yValue; y++)
            {
                Vector3 tilePosition = new Vector3(x, 0, -y);
                Transform newTile = Instantiate(generateBlockGameObject.transform, tilePosition, Quaternion.identity);  // 블럭 생성
                newTile.parent = generateMapParent.transform;   // 생성 블럭 부모 설정
                customTileMap.AddBlockList(newTile.gameObject, (int)selectBlockIndex);  // 생성된 블럭들 등록
            }
        }
    }

    public void CreateCustomMapGameObject()
    {

    }
    #endregion

    #region CanvasSetup

    public void ToggleIsSelectObject()
    {
        if (isSelectingObject == true)
            isSelectingObject = false;
        else
            isSelectingObject = true;

        isEditGuideLine = false;
    }
    public void ToggleIsEditGuideLine()
    {
        if (isEditGuideLine == true)
            isEditGuideLine = false;
        else
            isEditGuideLine = true;

        isSelectingObject = false;
    }
    public void SetDefaultIsSelect()
    {
        isSelectingObject = false;
        isEditGuideLine = false;
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
        GameManager.Instance.LoadScene(GameManager.PlayStates.MAIN_MENU, -1);
    }
    #endregion

    public void SaveGameData()
    {
        customTileMap.spawnEnemyInfoList = spawnEnemyInfoList;

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            string toJsonData = JsonUtility.ToJson(customTileMap);
            File.WriteAllText(saveFileDialog.FileName, toJsonData);
        }
    }

    public void LoadGameData()
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string readJsonString = File.ReadAllText(openFileDialog.FileName);
            customTileMap = JsonUtility.FromJson<CustomTileMap>(readJsonString);

            customTileMap.CreateCustomMapInMapEditor();
        }
    }

    public void IncreaseSpawnObjectAngle()
    {
        spawnObjectAngle += 90;
    }


    //==============================================================================


    [System.Serializable]
    public class CustomTileMap
    {
        [System.Serializable]
        public struct CustomTileMapStructData
        {
            public GameObject placeGameObject;
            public Vector3 placeGameTransform;
            public Vector3 placeGameRotation; 
            public int index;
        }

        public enum CustomTileMapListEnum
        {
            BLOCK_LIST = 0,
            OBJECT_LIST = 1,
        }

        [SerializeField] private List<CustomTileMapStructData> blockList = new List<CustomTileMapStructData>();              // 스폰 블록 리스트
        [SerializeField] private List<CustomTileMapStructData> objectList = new List<CustomTileMapStructData>();             // 스폰 오브젝트 리스트
        [SerializeField] public CustomTileMapStructData spawnPosition = new CustomTileMapStructData();                       // 플레이어 스폰 장소
        [SerializeField] public CustomTileMapStructData spawnEnemyPosition = new CustomTileMapStructData();                  // 적 스폰 장소
        [SerializeField] public List<CustomTileMapStructData> enemyGuideLineList = new List<CustomTileMapStructData>();      // 적 이동 경로
        [SerializeField] public List<CustomTileMapStructData> spawnTurretList = new List<CustomTileMapStructData>();         // 터렛 스폰 장소
        private GameObject parentGameObject;
        // ====================

        [SerializeField]
        public List<SpawnEnemyInfo> spawnEnemyInfoList;

        public List<CustomTileMapStructData> GetCustomTileMapList(CustomTileMapListEnum customTileMapListEnum)
        {
            switch (customTileMapListEnum)
            {
                case CustomTileMapListEnum.BLOCK_LIST:
                    return blockList;
                case CustomTileMapListEnum.OBJECT_LIST:
                    return objectList;
                default:
                    return null;
            }
        }

        #region AddCustomTileMapListData
        public bool AddBlockList(GameObject gameobject, int index)
        {
            if (gameobject == null)
                return false;

            bool isIncludeIndexRange = false;
            Array blockEnumArray = System.Enum.GetValues(typeof(BlockManager.BlockName));
            foreach (BlockManager.BlockName block in blockEnumArray)
            {
                if ((int)block == index)
                {
                    isIncludeIndexRange = true;
                }
            }

            if (isIncludeIndexRange == false)
                return false;

            CustomTileMapStructData customTileMapStructData;
            customTileMapStructData.placeGameObject = gameobject;
            customTileMapStructData.placeGameTransform = gameobject.transform.position;
            customTileMapStructData.placeGameRotation = gameobject.transform.rotation.eulerAngles;
            customTileMapStructData.index = index;

            blockList.Add(customTileMapStructData);
            return true;
        }

        public bool AddObjectList(GameObject gameobject, int index)
        {
            if (gameobject == null)
                return false;

            bool isIncludeIndexRange = false;
            Array objEnumArray = System.Enum.GetValues(typeof(ObjManager.ObjName));
            foreach (ObjManager.ObjName obj in objEnumArray)
            {
                if ((int)obj == index)
                {
                    isIncludeIndexRange = true;
                }
            }

            if (isIncludeIndexRange == false)
                return false;

            CustomTileMapStructData customTileMapStructData;
            customTileMapStructData.placeGameObject = gameobject;
            customTileMapStructData.placeGameTransform = gameobject.transform.position;
            customTileMapStructData.placeGameRotation = gameobject.transform.eulerAngles;
            customTileMapStructData.index = index;

            objectList.Add(customTileMapStructData);
            return true;
        }
        public bool SetPlayerSpawner(GameObject gameobject, int index)
        {
            if (gameobject == null)
                return false;

            CustomTileMapStructData customTileMapStructData;
            customTileMapStructData.placeGameObject = gameobject;
            customTileMapStructData.placeGameTransform = gameobject.transform.position;
            customTileMapStructData.placeGameRotation = gameobject.transform.rotation.eulerAngles;
            customTileMapStructData.index = index;

            if (spawnPosition.placeGameObject == null)
            {
                spawnPosition = customTileMapStructData;
            }
            else
            {
                Destroy(spawnPosition.placeGameObject);
                spawnPosition = customTileMapStructData;
            }

            return true;
        }
        public bool SetEnemySpawner(GameObject gameobject, int index)
        {
            if (gameobject == null)
                return false;

            CustomTileMapStructData customTileMapStructData;
            customTileMapStructData.placeGameObject = gameobject;
            customTileMapStructData.placeGameTransform = gameobject.transform.position;
            customTileMapStructData.placeGameRotation = gameobject.transform.rotation.eulerAngles;
            customTileMapStructData.index = index;

            if (spawnEnemyPosition.placeGameObject == null)
            {
                spawnEnemyPosition = customTileMapStructData;
            }
            else
            {
                Destroy(spawnEnemyPosition.placeGameObject);
                spawnEnemyPosition = customTileMapStructData;
            }

            return true;
        }

        public bool AddEnemyGuideLine(GameObject gameobject, int index)
        {
            if (gameobject == null)
                return false;

            CustomTileMapStructData customTileMapStructData;
            customTileMapStructData.placeGameObject = gameobject;
            customTileMapStructData.placeGameTransform = gameobject.transform.position;
            customTileMapStructData.placeGameRotation = gameobject.transform.rotation.eulerAngles;
            customTileMapStructData.index = index;

            enemyGuideLineList.Add(customTileMapStructData);

            return true;
        }

        public bool AddSpawnTurret(GameObject gameobject, int index)
        {
            if (gameobject == null)
                return false;

            CustomTileMapStructData customTileMapStructData;
            customTileMapStructData.placeGameObject = gameobject;
            customTileMapStructData.placeGameTransform = gameobject.transform.position;
            customTileMapStructData.placeGameRotation = gameobject.transform.rotation.eulerAngles;
            customTileMapStructData.index = index;

            spawnTurretList.Add(customTileMapStructData);

            return true;
        }
        
        #endregion
        #region DeleteCustomTileMapListData
        public void ClearBlockList()
        {
            blockList.Clear();
        }
        public bool DeletePlayerSpawner(GameObject gameobject)
        {
            if (gameobject == null)
                return false;

            spawnPosition = new CustomTileMapStructData();
            return true;
        }
        public bool DeleteEnemySpawner(GameObject gameobject)
        {
            if (gameobject == null)
                return false;

            spawnEnemyPosition = new CustomTileMapStructData();
            return true;
        }
        public bool DeleteEnemyGuideLine(GameObject gameobject)
        {
            if (gameobject == null)
                return false;

            for (int i = 0; i < enemyGuideLineList.Count; i++)
            {
                if (enemyGuideLineList[i].placeGameObject == gameobject)
                {
                    enemyGuideLineList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public bool DeleteSpawnTurret(GameObject gameobject)
        {
            if (gameobject == null)
                return false;

            for (int i = 0; i < spawnTurretList.Count; i++)
            {
                if (spawnTurretList[i].placeGameObject == gameobject)
                {
                    spawnTurretList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public bool DeleteObjectList(GameObject gameobject)
        {
            if (gameobject == null)
                return false;

            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i].placeGameObject == gameobject)
                {
                    objectList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        #endregion

        public void CreateCustomMap()   // 맵 생성
        {
            const string OBJECT_NAME = "Map_Objects";

            if(GameObject.Find(OBJECT_NAME) == null)
            {
                parentGameObject = new GameObject(OBJECT_NAME);
            }else
            {
                DestroyImmediate(parentGameObject);
            }
            
            for (int i = 0; i < blockList.Count; i++)
            {
                GameObject generateBlockGameObject = BlockManager.Instance.GetBlock((BlockManager.BlockName)blockList[i].index);
                Transform newTransform = Instantiate(generateBlockGameObject.transform);  // 블럭 생성
                newTransform.transform.position = blockList[i].placeGameTransform;
                newTransform.rotation = Quaternion.Euler(0, blockList[i].placeGameRotation.y, 0);
                newTransform.parent = parentGameObject.transform;
            }
            for (int i = 0; i < objectList.Count; i++)
            {
                GameObject generateGameObject = ObjManager.Instance.GetObject((ObjManager.ObjName)objectList[i].index);
                Transform newTransform = Instantiate(generateGameObject.transform);  // 오브젝트 생성
                newTransform.transform.position = objectList[i].placeGameTransform;
                newTransform.rotation = Quaternion.Euler(0, objectList[i].placeGameRotation.y, 0);
                newTransform.parent = parentGameObject.transform;
            }
            for(int i = 0; i < spawnTurretList.Count; i++)
            {
                GameObject generateGameObject = ObjManager.Instance.GetObject(ObjManager.ObjName.Turret_Spawner);
                Transform newTransform = Instantiate(generateGameObject.transform);  // 터렛설치장소 생성
                newTransform.transform.position = spawnTurretList[i].placeGameTransform;
                newTransform.rotation = Quaternion.Euler(0, spawnTurretList[i].placeGameRotation.y, 0);
                newTransform.parent = parentGameObject.transform;
            }

            SceneManager.MoveGameObjectToScene(parentGameObject, SceneManager.GetSceneByBuildIndex(1));
        }

        public void CreateCustomMapInMapEditor()   // 맵에디터에서 맵 생성
        {
            const string GENERATE_MAP_PARENT_NAME = "Generated Map";
            const string GENERATE_OBJECTS_PARENT_NAME = "mapObjectsParent";

            GameObject generateMapParent = GameObject.Find(GENERATE_MAP_PARENT_NAME);
            GameObject generateObjectsParent = GameObject.Find(GENERATE_OBJECTS_PARENT_NAME);

            if(generateMapParent == null)
                generateMapParent = new GameObject(GENERATE_MAP_PARENT_NAME);
            else
            {
                DestroyImmediate(generateMapParent);
                generateMapParent = new GameObject(GENERATE_MAP_PARENT_NAME);
            }
                
            if (generateObjectsParent == null)
                generateObjectsParent = new GameObject(GENERATE_OBJECTS_PARENT_NAME);
            else
            {
                DestroyImmediate(generateObjectsParent);
                generateObjectsParent = new GameObject(GENERATE_OBJECTS_PARENT_NAME);
            }
                
            for (int i = 0; i < blockList.Count; i++)
            {
                GameObject generateBlockGameObject = BlockManager.Instance.GetBlock((BlockManager.BlockName)blockList[i].index);
                Transform newTransform = Instantiate(generateBlockGameObject.transform);  // 블럭 생성
                newTransform.transform.position = blockList[i].placeGameTransform;
                newTransform.rotation = Quaternion.Euler(0, blockList[i].placeGameRotation.y, 0);
                newTransform.parent = generateMapParent.transform;

                newTransform.gameObject.layer = LayerMask.NameToLayer("Block");
            }
            for (int i = 0; i < objectList.Count; i++)
            {
                GameObject generateGameObject = ObjManager.Instance.GetObject((ObjManager.ObjName)objectList[i].index);
                Transform newTransform = Instantiate(generateGameObject.transform);  // 오브젝트 생성
                newTransform.transform.position = objectList[i].placeGameTransform;
                newTransform.rotation = Quaternion.Euler(0, objectList[i].placeGameRotation.y, 0);
                newTransform.parent = generateObjectsParent.transform;
                newTransform.gameObject.layer = LayerMask.NameToLayer("Object");

                CustomTileMapStructData customTileMapStructData = objectList[i];
                customTileMapStructData.placeGameObject = newTransform.gameObject;

                objectList[i] = customTileMapStructData;
            }

            // 플레이어 스폰 오브젝트 생성
            GameObject spawnPositionObject = Instantiate(ObjManager.Instance.GetObject(ObjManager.ObjName.Player_Spawner));
            spawnPositionObject.transform.position = spawnPosition.placeGameTransform;
            spawnPositionObject.transform.rotation = Quaternion.Euler(0, spawnPosition.placeGameRotation.y, 0);
            spawnPositionObject.transform.parent = generateObjectsParent.transform;
            spawnPosition.placeGameObject = spawnPositionObject;
            spawnPositionObject.layer = LayerMask.NameToLayer("Object");

            // 적 스폰 오브젝트 생성
            GameObject spawnEnemyPositionObject = Instantiate(ObjManager.Instance.GetObject(ObjManager.ObjName.Enemy_Spawner));
            spawnEnemyPositionObject.transform.position = spawnEnemyPosition.placeGameTransform;
            spawnEnemyPositionObject.transform.rotation = Quaternion.Euler(0, spawnEnemyPosition.placeGameRotation.y, 0);
            spawnEnemyPositionObject.transform.parent = generateObjectsParent.transform;
            spawnEnemyPosition.placeGameObject = spawnEnemyPositionObject;
            spawnEnemyPositionObject.layer = LayerMask.NameToLayer("Object");

            // 적 이동경로 오브젝트 생성
            for (int i = 0; i < enemyGuideLineList.Count; i++)
            {
                GameObject spawnEnemyGuideLinePositionObject = Instantiate(ObjManager.Instance.GetObject(ObjManager.ObjName.Enemy_GuideLine));
                spawnEnemyGuideLinePositionObject.transform.position = enemyGuideLineList[i].placeGameTransform;
                spawnEnemyGuideLinePositionObject.transform.rotation = Quaternion.Euler(0, enemyGuideLineList[i].placeGameRotation.y, 0);
                spawnEnemyGuideLinePositionObject.transform.parent = generateObjectsParent.transform;
                spawnEnemyGuideLinePositionObject.layer = LayerMask.NameToLayer("Object");

                CustomTileMapStructData customTileMapStructData = enemyGuideLineList[i];
                customTileMapStructData.placeGameObject = spawnEnemyGuideLinePositionObject;
                enemyGuideLineList[i] = customTileMapStructData;
            }

            // 터렛 오브젝트 생성
            for (int i = 0; i < spawnTurretList.Count; i++)
            {
                GameObject spawnTurretPositionObject = Instantiate(ObjManager.Instance.GetObject(ObjManager.ObjName.Turret_Spawner));
                spawnTurretPositionObject.transform.position = spawnTurretList[i].placeGameTransform;
                spawnTurretPositionObject.transform.rotation = Quaternion.Euler(0, spawnTurretList[i].placeGameRotation.y, 0);
                spawnTurretPositionObject.transform.parent = generateObjectsParent.transform;
                spawnTurretPositionObject.layer = LayerMask.NameToLayer("Object");

                CustomTileMapStructData customTileMapStructData = spawnTurretList[i];
                customTileMapStructData.placeGameObject = spawnTurretPositionObject;
                spawnTurretList[i] = customTileMapStructData;
            }    
        }

        public void CreateTurretSpawner()
        {
            //ObjName
        }
    }
}