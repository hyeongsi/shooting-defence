using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

// 맵 불러오기, 저장, 현재 맵 정보
public class MapManager : MonoBehaviour
{
    public GameObject mapObject;
   
    private BinaryFormatter bf;
    private FileStream file;
    private string path;
    private EditMapDataCollection editMapDataCollection = new EditMapDataCollection();

    private Dictionary<string, int> mapTypeNameParser;    // 프리팹 이름을 int 형으로 바꿔주기 위해 사용

    #region Singleton
    private static MapManager instance = null;

    private void Awake()
    {
        if( null == instance )
        {
            instance = this;

            // 씬 전환 시 이 오브젝트가 파괴되지 않도록 함, (싱글톤을 유지하도록)
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 씬 전환 시 동일한 오브젝트가 존재할 수 있다.
            // 그럴 경우 이전 씬에서 사용하던 오브젝트를 그대로 사용하기 때문에
            // 전환된 씬의 오브젝트 삭제 처리
            Destroy(this.gameObject);
        }
    }

    // 맵 매니저 접근 프로퍼티
    public static MapManager Instance
    {
        get
        {
            if( null == instance )
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    #region MapLoader

    public void SaveEditMapData()  
    {
        EditMapData editMapData;
        Transform childGameObjectTransform;

        // 설치된 맵 데이터 개수 계산
        for(int i = 0; i < mapObject.transform.childCount - 1; i ++)  // preview 데이터는 빼고 계산 (-1 처리)
        {
            for(int j = 0;  j < mapObject.transform.GetChild(i).childCount; j++)   // 각 맵 데이터의 정보 저장
            {
                childGameObjectTransform = mapObject.transform.GetChild(i).GetChild(j);

                editMapData = new EditMapData(
                    childGameObjectTransform.position.x, childGameObjectTransform.position.y, childGameObjectTransform.position.z,
                    childGameObjectTransform.GetChild(0).rotation.eulerAngles.x, childGameObjectTransform.GetChild(0).rotation.eulerAngles.y, childGameObjectTransform.GetChild(0).rotation.eulerAngles.z,
                    i, 
                    mapTypeNameParser[childGameObjectTransform.name]
                    );
                
                editMapDataCollection.editmapData.Add(editMapData);
            }
        }
    }

    public void CreateEditMapData() 
    {
        if (editMapDataCollection.editmapData.Count == 0)
            return;

        Transform instantiateGameObjectTransform;
        // 이전에 설치되어 있던 블럭들 삭제

        if (GameObject.Find("MapGameObject(Clone)") != null)
        {
            Destroy(mapObject);
        }

        mapObject = Instantiate(PrefabManager.Instance.MapObjectPrefab);

        // 저장되어 있는 데이터로 블럭들 생성
        for (int i = 0; i < editMapDataCollection.editmapData.Count; i ++)
        {
            switch(editMapDataCollection.editmapData[i].objectType)
            {
                case (int)MapType.BLOCK:
                    instantiateGameObjectTransform = Instantiate(PrefabManager.Instance.BlockPrefabArray[editMapDataCollection.editmapData[i].mapNumber].transform);
                    break;
                case (int)MapType.TURRET:
                    instantiateGameObjectTransform = Instantiate(PrefabManager.Instance.TurretPrefabArray[editMapDataCollection.editmapData[i].mapNumber].transform);
                    break;
                case (int)MapType.BARRICADE:
                    instantiateGameObjectTransform = Instantiate(PrefabManager.Instance.BarricadePrefabArray[editMapDataCollection.editmapData[i].mapNumber].transform);
                    break;
                default:
                    return;
            }

            instantiateGameObjectTransform.position = new Vector3(editMapDataCollection.editmapData[i].positionX, editMapDataCollection.editmapData[i].positionY, editMapDataCollection.editmapData[i].positionZ);  // 위치 설정
            instantiateGameObjectTransform.parent = mapObject.transform.GetChild(editMapDataCollection.editmapData[i].objectType);     // 부모 객체 설정
            instantiateGameObjectTransform.GetChild(0).localRotation = Quaternion.Euler(editMapDataCollection.editmapData[i].rotationX, editMapDataCollection.editmapData[i].rotationY, editMapDataCollection.editmapData[i].rotationZ);
        }
    }

    // 맵 저장
    public void SaveMap()
    {
        if (!Directory.Exists(path))            // 위의 폴더가 없다면
        {
            Directory.CreateDirectory(path);    // 해당 경로의 폴더 생성
        }

        string selectPath = EditorUtility.SaveFilePanel("create dat file", path, "", "dat");    // 저장 파일 이름 선택
        if (string.IsNullOrEmpty(selectPath))   // 선택 취소 시 종료
            return;

        file = new FileStream(selectPath, FileMode.OpenOrCreate);
        SaveEditMapData();                      // 설치된 맵데이터를 기준으로 해당 내용들 클래스에 저장
        bf.Serialize(file, editMapDataCollection);        // 미리 만들어 둔 MapData 클래스의 데이터들을 이용해 데이터 저장

        file.Close();
    }

    // 맵 불러오기
    public void LoadMap()
    {
        if (!Directory.Exists(path))            // 위의 폴더가 없다면
        {
            Directory.CreateDirectory(path);    // 해당 경로의 폴더 생성
        }

        string selectPath = EditorUtility.OpenFilePanel("select dat file", path, "dat");    // 불러오기 경로 선택
        if (string.IsNullOrEmpty(selectPath))   // 선택 취소 시 종료
            return;

        file = File.Open(selectPath, FileMode.Open);
        editMapDataCollection = (EditMapDataCollection)bf.Deserialize(file);    // 역직렬화 수행하여 데이터 로드
        CreateEditMapData();                    // 불러온 맵데이터를 기준으로 오브젝트 생성

        file.Close();
    }
    #endregion

    private void Start()
    {
        bf = new BinaryFormatter();
        path = Application.persistentDataPath + "/Data" +"/mapData";    // 이거 경로 다른곳으로 설정하기

        if (GameObject.Find("MapGameObject(Clone)") == null)
        {
            mapObject = Instantiate(PrefabManager.Instance.MapObjectPrefab);
            if(mapObject.transform.childCount >= 1 && PrefabManager.Instance.BlockPrefabArray.Length >= 1)
            {
                Instantiate(PrefabManager.Instance.BlockPrefabArray[0].transform).parent = mapObject.transform.GetChild(0);  // 시작과 동시에 기본 블럭 생성함 그래야 다른 블럭을 생성할 수 있으니
            }
        }

        mapTypeNameParser = new Dictionary<string, int>();

        // name parser 초기화
        for (int i = 0; i < PrefabManager.Instance.BlockPrefabArray.Length; i++)
            mapTypeNameParser.Add(PrefabManager.Instance.BlockPrefabArray[i].name + "(Clone)", i);
        for (int i = 0; i < PrefabManager.Instance.BarricadePrefabArray.Length; i++)
            mapTypeNameParser.Add(PrefabManager.Instance.BarricadePrefabArray[i].name + "(Clone)", i);
        for (int i = 0; i < PrefabManager.Instance.TurretPrefabArray.Length; i++)
            mapTypeNameParser.Add(PrefabManager.Instance.TurretPrefabArray[i].name + "(Clone)", i);
    }
}

[System.Serializable]
public class EditMapData
{
    public float positionX = 0.0f;
    public float positionY = 0.0f;
    public float positionZ = 0.0f;

    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float rotationZ = 0.0f;

    public int objectType = 0;     // 블럭, 타워, 장애물 구분
    public int mapNumber = 0;   // 선택 맵데이터 종류, type 블럭이면 : 블럭1, 블럭2, type 타워면 : 타워1, 타워2 등등

    public EditMapData()
    { 
    
    }
    public EditMapData(float x, float y, float z, float rX, float rY, float rZ, int objectType, int mapNumber)
    {
        positionX = x;
        positionY = y;
        positionZ = z;

        rotationX = rX;
        rotationY = rY;
        rotationZ = rZ;

        this.objectType = objectType;
        this.mapNumber = mapNumber;    }
}

[System.Serializable]
public class EditMapDataCollection
{
    public List<EditMapData> editmapData = new List<EditMapData>();
}

public enum TransparentMaterialColor
{
    NONE = -1,
    GREEN_COLOR_MATERIAL = 0,
    RED_COLOR_MATERIAL = 1,
    YELLOW_GRID_COLOR_MATERIAL = 2,
}

public enum MapType
{
    BLOCK = 0,
    TURRET = 1,
    BARRICADE = 2,
    PREVIEW = 3,
}