using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

// 맵 불러오기, 저장, 현재 맵 정보
public class MapManager : MonoBehaviour
{
    BinaryFormatter bf;
    FileStream file;

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

    // 파일 경로 지정
    public void FileSelect()
    {
        string path;

        path = EditorUtility.OpenFilePanel("Overwrite with png", "", "dat");

        Debug.Log(path);
    }

    // 맵 저장
    public void SaveMap()
    {
        FileSelect();

        string path = Application.dataPath + "/mapData";
        string dataPath = "/test.dat";

        Debug.Log(path);

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        file = new FileStream(path + dataPath, FileMode.OpenOrCreate);

        int a = 20;

        bf.Serialize(file, a);
        file.Close();
    }

    // 맵 불러오기
    public void LoadMap()
    {
        FileSelect();

        string path = Application.dataPath + "/mapData";
        string dataPath = "/test.dat";

        if (!File.Exists(path + dataPath))
        {
            Debug.Log("불러올 파일이 존재하지 않습니다.");
            return;
        }

        file = File.Open(path + dataPath, FileMode.Open);

        int a = (int)bf.Deserialize(file);

        Debug.Log(a);

        file.Close();
    }
    #endregion

    private void Start()
    {
        bf = new BinaryFormatter();
    }
}
