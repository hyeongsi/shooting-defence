//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using UnityEditor;

//// 맵 불러오기, 저장, 현재 맵 정보
//public class MapManager : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject mapObjectPrefab;
//    [SerializeField]
//    private GameObject mapGameObject;
//    private GameObject[] parentGameObject = null;

//    private BinaryFormatter bf = new BinaryFormatter();
//    private FileStream file;
//    private string path;
//    private Stage stageData = new Stage();

//    private Dictionary<string, int> mapTypeNameParser;    // 프리팹 이름을 int 형으로 바꿔주기 위해 사용

//    #region Property
//    public GameObject MapGameObject { get { return mapGameObject; } }
//    public GameObject[] ParentGameObject { get { return parentGameObject; } }
//    public Stage StageData { get { return stageData; } }
//    #endregion

//    #region Singleton
//    private static MapManager instance = null;

//    private void Awake()
//    {
//        if( null == instance )
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    // 맵 매니저 접근 프로퍼티
//    public static MapManager Instance { get { return instance; } }
//    #endregion

//    #region MapLoader
//    public void SaveEditBlockData()  
//    {
//        EditBlockData editBlockData;
//        Transform childGameObjectTransform;
//        if (stageData == null)
//        {
//            Debug.Log("stageData null error,  MapManager.SaveEditBlockData()");
//            return;
//        }

//        // 설치된 맵 데이터 개수 계산
//        for (int i = 0; i < mapGameObject.transform.childCount - 1; i ++)  // preview 데이터는 빼고 계산 (-1 처리)
//        {
//            for(int j = 0;  j < mapGameObject.transform.GetChild(i).childCount; j++)   // 각 맵 데이터의 정보 저장
//            {
//                childGameObjectTransform = mapGameObject.transform.GetChild(i).GetChild(j);

//                editBlockData = new EditBlockData(
//                    childGameObjectTransform.position.x, childGameObjectTransform.position.y, childGameObjectTransform.position.z,
//                    childGameObjectTransform.GetChild(0).rotation.eulerAngles.x, childGameObjectTransform.GetChild(0).rotation.eulerAngles.y, childGameObjectTransform.GetChild(0).rotation.eulerAngles.z,
//                    i, 
//                    mapTypeNameParser[childGameObjectTransform.name]
//                    );
                
//                stageData.editBlockData.Add(editBlockData);
//            }
//        }
//    }

//    public void CreateEditBlockData(Stage stage) 
//    {
//        if (stage.editBlockData.Count == 0)
//            return;

//        if (GameObject.Find("MapGameObject(Clone)") != null)
//        {
//            Destroy(mapGameObject);
//        }

//        stageData = stage;

//        mapGameObject = Instantiate(mapObjectPrefab);

//        // 저장되어 있는 데이터로 블럭들 생성
//        Transform instantiateGameObjectTransform;
//        for (int i = 0; i < stageData.editBlockData.Count; i ++)
//        {
//            switch(stageData.editBlockData[i].objectType)
//            {
//                case (int)MapType.BLOCK:
//                    instantiateGameObjectTransform = Instantiate(BlockManager.Instance.BlockPrefabArray[stageData.editBlockData[i].mapNumber].transform);
//                    break;
//                case (int)MapType.TURRET:
//                    instantiateGameObjectTransform = Instantiate(TurretManager.Instance.TurretPrefabArray[stageData.editBlockData[i].mapNumber].transform);
//                    break;
//                case (int)MapType.BARRICADE:
//                    instantiateGameObjectTransform = Instantiate(BarricadeManager.Instance.BarricadePrefabArray[stageData.editBlockData[i].mapNumber].transform);
//                    break;
//                default:
//                    return;
//            }

//            instantiateGameObjectTransform.position = new Vector3(stageData.editBlockData[i].positionX, stageData.editBlockData[i].positionY, stageData.editBlockData[i].positionZ);  // 위치 설정
//            instantiateGameObjectTransform.parent = mapGameObject.transform.GetChild(stageData.editBlockData[i].objectType);     // 부모 객체 설정
//            instantiateGameObjectTransform.GetChild(0).localRotation = Quaternion.Euler(stageData.editBlockData[i].rotationX, stageData.editBlockData[i].rotationY, stageData.editBlockData[i].rotationZ);
//        }
//    }

//    // 맵 저장
//    public void SaveMap()
//    {
//        if (!Directory.Exists(path))            // 위의 폴더가 없다면
//        {
//            Directory.CreateDirectory(path);    // 해당 경로의 폴더 생성
//        }

//        string selectPath = default;
//        selectPath = EditorUtility.SaveFilePanel("create dat file", path, "", "dat");    // 저장 파일 이름 선택

//        if (string.IsNullOrEmpty(selectPath))   // 선택 취소 시 종료
//            return;

//        file = new FileStream(selectPath, FileMode.OpenOrCreate);
//        SaveEditBlockData();                      // 설치된 맵데이터를 기준으로 해당 내용들 클래스에 저장
//        bf.Serialize(file, stageData);        // 미리 만들어 둔 MapData 클래스의 데이터들을 이용해 데이터 저장

//        file.Close();
//    }

//    // 맵 불러오기
//    public void LoadMap()
//    {
//        if (!Directory.Exists(path))            // 위의 폴더가 없다면
//        {
//            Directory.CreateDirectory(path);    // 해당 경로의 폴더 생성
//        }

//        string selectPath = default;
//        selectPath = EditorUtility.OpenFilePanel("select dat file", path, "dat");    // 불러오기 경로 선택

//        if (string.IsNullOrEmpty(selectPath))   // 선택 취소 시 종료
//            return;

//        file = File.Open(selectPath, FileMode.Open);  
//        CreateEditBlockData((Stage)bf.Deserialize(file));    // 불러온 맵데이터를 기준으로 오브젝트 생성  // 역직렬화 수행하여 데이터 로드
//        RegisterParentGameObject();

//        file.Close();
//    }
//    #endregion

//    public void RegisterParentGameObject()
//    {
//        parentGameObject = new GameObject[mapGameObject.transform.childCount];
//        for (int i = 0; i < mapGameObject.transform.childCount; i++)
//        {
//            parentGameObject[i] = mapGameObject.transform.GetChild(i).gameObject;
//        }
//    }

//    private void Start()    // 나중에 start 말고, 게임시작 ui 눌렀을 때 실행하도록 변경 해야함, 적당히 로드하던 아무튼 그런기능 추가해야함
//    {
//        if (GameObject.Find("MapGameObject(Clone)") == null)
//        {
//            mapGameObject = Instantiate(mapObjectPrefab);
//            if(GameManager.Instance.PlayeState == GameManager.PlayStates.MAP_EDIT)
//            {
//                if (mapGameObject.transform.childCount >= 1 && BlockManager.Instance.BlockPrefabArray.Length >= 1)
//                {
//                    Instantiate(BlockManager.Instance.BlockPrefabArray[0].transform).parent = mapGameObject.transform.GetChild(0);  // 시작과 동시에 기본 블럭 생성함 그래야 다른 블럭을 생성할 수 있으니
//                }
//            }
//        }

//        path = Application.streamingAssetsPath + "/Mapdata";

//        mapTypeNameParser = new Dictionary<string, int>();

//        // name parser 초기화
//        for (int i = 0; i < BlockManager.Instance.BlockPrefabArray.Length; i++)
//            mapTypeNameParser.Add(BlockManager.Instance.BlockPrefabArray[i].name + "(Clone)", i);
//        for (int i = 0; i < BarricadeManager.Instance.BarricadePrefabArray.Length; i++)
//            mapTypeNameParser.Add(BarricadeManager.Instance.BarricadePrefabArray[i].name + "(Clone)", i);
//        for (int i = 0; i < TurretManager.Instance.TurretPrefabArray.Length; i++)
//            mapTypeNameParser.Add(TurretManager.Instance.TurretPrefabArray[i].name + "(Clone)", i);

//        // 맵 생성 부모 초기화
//        RegisterParentGameObject();
//    }
//}

//public enum MapType
//{
//    BLOCK = 0,
//    TURRET = 1,
//    BARRICADE = 2,
//    PREVIEW = 3,
//}