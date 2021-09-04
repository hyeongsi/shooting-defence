using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 불러오기, 저장, 현재 맵 정보
public class MapManager : MonoBehaviour
{
    // 건축물 타입
    // 맵 크기 (x,y)
    // 맵 크기에 따른 건축물 배열 (x,y,z)
    // 

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
    // 맵 저장
    public void SaveMap()
    {

    }

    // 맵 불러오기
    public void LoadMap()
    {

    }
    #endregion


}
