using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mapObjectPrefab;

    [SerializeField]
    private Material[] blockMaterialArray;

    [SerializeField]
    private GameObject[] blockPrefabArray;
    [SerializeField]
    private GameObject[] barricadePrefabArray;
    [SerializeField]
    private GameObject[] turretPrefabArray;

    [SerializeField]
    private GameObject[] enemyPrefabArray;

    #region Property
    public GameObject MapObjectPrefab { get { return mapObjectPrefab; } }
    public Material[] BlockMaterialArray { get { return blockMaterialArray; } }

    public GameObject[] BlockPrefabArray { get { return blockPrefabArray; } }
    public GameObject[] BarricadePrefabArray { get { return barricadePrefabArray; } }
    public GameObject[] TurretPrefabArray { get { return turretPrefabArray; } }
    public GameObject[] EnemyPrefabArray { get { return enemyPrefabArray; } }
    #endregion
    #region Singleton
    private static PrefabManager instance = null;

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
    public static PrefabManager Instance
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


}
