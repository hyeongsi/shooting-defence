using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private float buildDistance = 10.0f;     // 건축 가능 범위
    [SerializeField]
    private bool is3rdBuildMode = true;      // 3인칭 건축 모드 여부
    [SerializeField]
    public GameObject[] blockPrefabs;        // 생성할 블럭 프리팹
     
    public bool isBuild = false;             // 건축 중인지 체크
   

    #region Property
    public bool Is3rdBuildMode
    {
        get { return is3rdBuildMode; }
        set { is3rdBuildMode = value; }
    }
    #endregion

    #region Singleton
    private static BuildManager instance = null;

    private void Awake()
    {
        if (null == instance)
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
    public static BuildManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    private bool PredictBuild()  // 건축 전 건축가능 예측
    {
        // MapManager.Instance.     맵 매니저에서 현재 맵 정보를 가져와서 건축 가능 여부 리턴

        return false;
    }

    public void Build()     // 실제 건축
    {
        if(is3rdBuildMode)              // 3인칭 건축 모드 (건축 거리, )
        {
            if (PredictBuild())  // 건축 가능
            {
                // Instantiate()   실제 건축물 생성
                // MapManager.Instance 맵 매니저의 현재 맵 정보 갱신

                // 초록색 건축물 출력 + 한번더 클릭 시 건축물 생성

            }
            else    // 건축 불가능
            {
                // 빨간색 건축물 출력
            }
        }
        else                             // 1인칭 건축 모드
        {
            if (PredictBuild())  // 건축 가능
            {
                // Instantiate()   실제 건축물 생성
                // MapManager.Instance 맵 매니저의 현재 맵 정보 갱신

                // 초록색 건축물 출력 + 한번더 클릭 시 건축물 생성

            }
            else    // 건축 불가능
            {
                // 빨간색 건축물 출력
            }
        }
    }
}