using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Material[] blockMaterialArray;

    private GameObject transparentObject = null;

    private Ray ray;
    private RaycastHit hit;
    private int rayermask;
    private Vector3 scrennCenter;
    private Block block;
    private Material originMaterial;

    private int selectObjctType = 0;
    private int selectPrefab = 0;
    private float currentRotationAngle = 0;
    private const float rotationAngle = 90.0f;

    private bool isBuildMode = true;
    private bool isEditMode = true;

    private const string layerName = "Block";

    #region property
    public int SelectObjctType
    {
        set
        {
            if (MapManager.Instance.MapGameObject.transform.childCount-1 <= value && 0 > value)
                return;

            selectObjctType = value;
        }
    }
    public int SelectPrefab
    {
        set
        {
            int prefabSize = FindPrefabLength();

            if (prefabSize == 0)
                return;

            if (prefabSize <= value && 0 > value)
                return;

            selectPrefab = value;
        }
    }
    public bool IsBuildMode
    {
        get { return isBuildMode; }
        set { isBuildMode = value; }
    }
    
    public bool IsEditMode
    {
        get { return isEditMode; }
        set { isEditMode = value; }
    }
    #endregion

    private Block GetBlockObject()
    {
        switch (selectObjctType)
        {
            case (int)MapType.BLOCK:
                return BlockManager.Instance.BlockPrefabArray[selectPrefab].GetComponent<Block>();
            case (int)MapType.TURRET:
                return TurretManager.Instance.TurretPrefabArray[selectPrefab].GetComponent<Block>();
            case (int)MapType.BARRICADE:
                return BarricadeManager.Instance.BarricadePrefabArray[selectPrefab].GetComponent<Block>();
        }

        return default;
    }

    private GameObject GetMapTypeObject()
    {
        switch (selectObjctType)
        {
            case (int)MapType.BLOCK:
                return BlockManager.Instance.BlockPrefabArray[selectPrefab];
            case (int)MapType.TURRET:
                return TurretManager.Instance.TurretPrefabArray[selectPrefab];
            case (int)MapType.BARRICADE:
                return BarricadeManager.Instance.BarricadePrefabArray[selectPrefab];
        }

        return default;
    }

    private bool FindBlocks(Vector3 point, Vector3 newBlockSize)
    {
        const float CHECK_RADIUS_SIZE = 0.47f;
        const float GAP = 0.5f;     // 1칸 블럭의 중앙에서 판단해야 하기 때문에 +0.5 수행
        Vector3 findBlocksVector;
        Collider[] collider = new Collider[1];  // 콜라이더 부분 null 하면 OverlapSphereNonAlloc 결과가 무조건 0만 나오게 되어 무조건 만들어 넣어주어 함

        findBlocksVector = point;

        for (int y = (int)point.y; y < (int)point.y + newBlockSize.y; y++)
        {
            findBlocksVector.y = y + GAP;
            for (int z = (int)point.z; z < (int)point.z + newBlockSize.z; z++)
            {
                findBlocksVector.z = z + GAP;
                for (int x = (int)point.x; x < (int)point.x + newBlockSize.x; x++)
                {
                    findBlocksVector.x = x + GAP;

                    if (Physics.OverlapSphereNonAlloc(findBlocksVector, CHECK_RADIUS_SIZE, collider, rayermask) != 0)
                        return true;
                }
            }
        }

        return false;
    }
    private Vector3 FindDirection(Vector3 point, Vector3 blockSize)
    {
        Vector3 pointVector = Vector3.zero;

        // float 계산 시 오차가 발생 하기 때문에, 0.01을 오차 범위로 두고 계산
        if (point.x >= (blockSize.x - 0.01f) && point.x <= (blockSize.x + 0.01f))
        {
            pointVector = Vector3.right;
        }
        else if (point.y >= (blockSize.y - 0.01f) && point.y <= (blockSize.y + 0.01f))
        {
            pointVector = Vector3.up;
        }
        else if (point.z >= (blockSize.z - 0.01f) && point.z <= (blockSize.z + 0.01f))
        {
            pointVector = Vector3.forward;
        }
        else if (point.x >= -0.01f && point.x <= 0.01f)
        {
            pointVector = Vector3.left;
        }
        else if (point.y >= -0.01f && point.y <= 0.01f)
        {
            pointVector = Vector3.down;
        }
        else if (point.z >= -0.01f && point.z <= 0.01f)
        {
            pointVector = Vector3.back;
        }

        return pointVector;
    }

    private Vector3 FindSpawnPosition(Vector3 position, Vector3 point, Vector3 direction, Vector3 newBlockSize)
    {
        Vector3 pointVector = Vector3.zero;

        if (direction == Vector3.right || direction == Vector3.forward || direction == Vector3.up)
        {
            pointVector.x = position.x + Mathf.Floor(point.x);
            pointVector.y = position.y + Mathf.Floor(point.y);
            pointVector.z = position.z + Mathf.Floor(point.z);
        }
        else
        {
            pointVector.x = position.x + (direction.x * newBlockSize.x) + Mathf.Floor(point.x);
            pointVector.y = position.y + (direction.y * newBlockSize.y) + Mathf.Floor(point.y);
            pointVector.z = position.z + (direction.z * newBlockSize.z) + Mathf.Floor(point.z);
        }

        return pointVector;
    }

    public void DestroyBlock(Block block)
    {
        if((int)block.BlockTypeVar == selectObjctType)
        {
            Destroy(block.gameObject);
        }
    }

    public void GenerateBlock()         // 블럭 생성
    {
        if (transparentObject == null)
            return;

        Renderer renderer = transparentObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        if (renderer.material.name == blockMaterialArray[(int)TransparentMaterialColor.RED_COLOR_MATERIAL].name + " (Instance)")   // 설치 불가 라면 생성 X
            return;

        transparentObject.transform.parent = MapManager.Instance.ParentGameObject[(int)transparentObject.GetComponent<Block>().BlockTypeVar].transform;

        if(originMaterial != null)
            renderer.material = originMaterial;

        if(selectObjctType == (int)MapType.TURRET)
            TurretManager.Instance.SettingTurretData(ref transparentObject, selectPrefab);

        transparentObject.gameObject.layer = (int)LayerNumbering.BLOCK;

        transparentObject = null;
    }
    public void GenerateTransparentBlock(RaycastHit hit, Block block)         // 블럭 생성
    {
        Vector3 createDirection;
        Vector3 createBlockPosition;
        Vector3 point;

        if (selectPrefab >= 0 && selectPrefab < FindPrefabLength())    // 없는 블럭은 생성하지 못하도록
        {
            point = hit.point - hit.transform.position;

            createDirection = FindDirection(point, block.BlockSize);

            Vector3 newBlockSize;
            Block getBlockObject = GetBlockObject();
            if (getBlockObject == default)
                return;

            newBlockSize = getBlockObject.BlockSize;
            createBlockPosition = FindSpawnPosition(hit.transform.position, point, createDirection, newBlockSize);

            Transform newBlock;
            GameObject getMapTypeObject = GetMapTypeObject();

            if (getMapTypeObject == default)
                return;

            newBlock = Instantiate(getMapTypeObject.transform, createBlockPosition, Quaternion.Euler(Vector3.zero)) as Transform;
            transparentObject = newBlock.gameObject;
            transparentObject.transform.parent = MapManager.Instance.ParentGameObject[(int)MapType.PREVIEW].transform;

            newBlock.gameObject.layer = (int)LayerNumbering.DEFAULT;
            newBlock.GetChild(0).localRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            if (FindBlocks(createBlockPosition, newBlockSize)) // 설치 위치에 블럭이 이미 존재하면 붉은 오브젝트 출력
            {
                originMaterial = newBlock.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
                newBlock.GetChild(0).GetChild(0).GetComponent<Renderer>().material = blockMaterialArray[(int)TransparentMaterialColor.RED_COLOR_MATERIAL];
            }
            else
            {
                originMaterial = newBlock.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
                newBlock.GetChild(0).GetChild(0).GetComponent<Renderer>().material= blockMaterialArray[(int)TransparentMaterialColor.GREEN_COLOR_MATERIAL];
            }
        }
    }

    public void SetTransparentBlockTransform(RaycastHit hit, Block block)
    {
        Vector3 createDirection;
        Vector3 createBlockPosition;
        Vector3 point;

        if (selectPrefab >= 0 && selectPrefab < FindPrefabLength())    // 없는 블럭은 생성하지 못하도록
        {
            point = hit.point - hit.transform.position;

            createDirection = FindDirection(point, block.BlockSize);

            Block getBlockObject = GetBlockObject();
            if (getBlockObject == default)
                return;
            Vector3 newBlockSize = getBlockObject.BlockSize;

            createBlockPosition = FindSpawnPosition(hit.transform.position, point, createDirection, newBlockSize);

            if (FindBlocks(createBlockPosition, newBlockSize)) // 설치 위치에 블럭이 이미 존재하면 붉은 오브젝트 출력
            {
                transparentObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = blockMaterialArray[(int)TransparentMaterialColor.RED_COLOR_MATERIAL];
            }
            else
            {
                transparentObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = blockMaterialArray[(int)TransparentMaterialColor.GREEN_COLOR_MATERIAL];
            }

            if (transparentObject.transform.position != createBlockPosition)
                transparentObject.transform.position = createBlockPosition;

            transparentObject.transform.GetChild(0).localRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        }
    }

    public int FindPrefabLength()
    {
        switch(selectObjctType)
        {
            case (int)MapType.BLOCK:
                return BlockManager.Instance.BlockPrefabArray.Length;
            case (int)MapType.TURRET:
                return TurretManager.Instance.TurretPrefabArray.Length;
            case (int)MapType.BARRICADE:
                return BarricadeManager.Instance.BarricadePrefabArray.Length;
        }

        return 0;
    }

    public void ClearTransparentBlock()
    {
        if (transparentObject != null)
        {
            Destroy(transparentObject);
            transparentObject = null;
        }
    }

    private void Start()
    {
        rayermask = 1 << LayerMask.NameToLayer(layerName);
        scrennCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        GameManager.Instance.pauseGameDelegate += ClearTransparentBlock;
    }

    private void Update()
    {
        if (GameManager.Instance == null && GameManager.Instance.IsPause)
            return;

        if (!isBuildMode)
            return;

        if (Input.GetKeyDown(KeyCode.R))     // 블럭 회전
            currentRotationAngle += rotationAngle;

        ray = Camera.main.ScreenPointToRay(scrennCenter);
        if (Physics.Raycast(ray, out hit, 100.0f, rayermask))
        {
            if (Input.GetMouseButtonUp(0))           // 좌클릭, 블럭 생성
            {
                GenerateBlock();
            }
            else if (Input.GetMouseButtonUp(1))      // 우클릭, 블럭 삭제
            {
                DestroyBlock(hit.transform.GetComponent<Block>());
            }
            else
            {
                block = hit.transform.GetComponent<Block>();
                if (transparentObject == null)
                    GenerateTransparentBlock(hit, block);
                else
                    SetTransparentBlockTransform(hit, block);
            }
        }
        else
        {
            ClearTransparentBlock();
        }
    }

    private enum TransparentMaterialColor
    {
        NONE = -1,
        GREEN_COLOR_MATERIAL = 0,
        RED_COLOR_MATERIAL = 1,
    }
}

public enum LayerNumbering
{
    DEFAULT = 0,
    BLOCK = 8,
}
