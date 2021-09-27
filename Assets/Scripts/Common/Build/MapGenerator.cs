using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Material[] blockMaterialArray;

    private bool isCreateAble = true;

    private GameObject transparentObject = null;
    private SwitchMaterial switchMaterial = new SwitchMaterial();

    private Ray ray;
    private RaycastHit hit;
    private int rayermask;
    private Vector3 scrennCenter;
    private Block block;

    private int selectObjctType = 0;
    private int selectPrefab = 0;
    private float currentRotationAngle = 0;
    private const float rotationAngle = 90.0f;

    private bool isBuildMode = true;
    private bool isEditMode = true;

    private const string layerName = "Block";
    private const float ERROR_RANGE = 0.02f;
    #region Property
    public int SelectObjctType
    {
        set
        {
            if (MapManager.Instance.MapGameObject.transform.childCount-1 <= value && 0 > value)
                return;

            switchMaterial.Init();
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

            switchMaterial.Init();
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
                return BlockManager.Instance.BlockArray[selectPrefab];
            case (int)MapType.TURRET:
                return TurretManager.Instance.TurretBlockArray[selectPrefab];
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
        if (point.x >= (blockSize.x - ERROR_RANGE) && point.x <= (blockSize.x + ERROR_RANGE))
        {
            pointVector = Vector3.right;
        }
        else if (point.y >= (blockSize.y - ERROR_RANGE) && point.y <= (blockSize.y + ERROR_RANGE))
        {
            pointVector = Vector3.up;
        }
        else if (point.z >= (blockSize.z - ERROR_RANGE) && point.z <= (blockSize.z + ERROR_RANGE))
        {
            pointVector = Vector3.forward;
        }
        else if (point.x >= -ERROR_RANGE && point.x <= ERROR_RANGE)
        {
            pointVector = Vector3.left;
        }
        else if (point.y >= -ERROR_RANGE && point.y <= ERROR_RANGE)
        {
            pointVector = Vector3.down;
        }
        else if (point.z >= -ERROR_RANGE && point.z <= ERROR_RANGE)
        {
            pointVector = Vector3.back;
        }

        return pointVector;
    }

    private Vector3 FindSpawnPosition(Vector3 position, Vector3 point, Vector3 direction, Vector3 newBlockSize)
    {
        Vector3 pointVector = Vector3.zero;

        if(direction == Vector3.right)
        {
            pointVector.x = Mathf.Approximately(point.x, Mathf.Round(point.x)) ?
                position.x + Mathf.Round(point.x) * direction.x :
                position.x + Mathf.Floor(point.x) * direction.x;

            pointVector.y = position.y;
            pointVector.z = position.z;
        }
        else if(direction == Vector3.forward)
        {
            pointVector.z = Mathf.Approximately(point.z, Mathf.Round(point.z)) ?
                position.z + Mathf.Round(point.z) * direction.z :
                position.z + Mathf.Floor(point.z) * direction.z;

            pointVector.x = position.x;
            pointVector.y = position.y;
        }
        else if (direction == Vector3.up)
        {
            pointVector.y = Mathf.Approximately(point.y, Mathf.Round(point.y)) ?
                position.y + Mathf.Round(point.y) * direction.y :
                position.y + Mathf.Floor(point.y) * direction.y;

            pointVector.x = position.x;
            pointVector.z = position.z;
        }
        else if (direction == Vector3.left)
        {
            pointVector.x = Mathf.Approximately(point.x, Mathf.Round(point.x)) ?
                position.x + (direction.x * newBlockSize.x) + Mathf.Round(point.x) :
                position.x + (direction.x * newBlockSize.x) + Mathf.Floor(point.x);

            pointVector.y = position.y;
            pointVector.z = position.z;
        }
        else if (direction == Vector3.back)
        {
            pointVector.z = Mathf.Approximately(point.z, Mathf.Round(point.z)) ?
                position.z + (direction.z * newBlockSize.z) + Mathf.Round(point.z) :
                position.z + (direction.z * newBlockSize.z) + Mathf.Floor(point.z);

            pointVector.x = position.x;
            pointVector.y = position.y;
        }
        else if (direction == Vector3.down)
        {
            pointVector.y = Mathf.Approximately(point.y, Mathf.Round(point.y)) ?
               position.y + (direction.y * newBlockSize.y) + Mathf.Round(point.y) :
               position.y + (direction.y * newBlockSize.y) + Mathf.Floor(point.y);

            pointVector.x = position.x;
            pointVector.z = position.z;
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

        if (!isCreateAble)
            return;

        transparentObject.transform.parent = MapManager.Instance.ParentGameObject[(int)transparentObject.GetComponent<Block>().BlockTypeVar].transform;

        switchMaterial.SwitchSaveMaterial(transparentObject);   // 원래의 색상 복구

        if (selectObjctType == (int)MapType.TURRET)
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
            
            if (switchMaterial.IsEmptySaveRenderer())
                switchMaterial.SaveMaterial(transparentObject);

            if (FindBlocks(createBlockPosition, newBlockSize)) // 설치 위치에 블럭이 이미 존재하면 붉은 오브젝트 출력
            {
                isCreateAble = false;
                switchMaterial.SwitchOtherMaterial(transparentObject, blockMaterialArray[(int)TransparentMaterialColor.RED_COLOR_MATERIAL]);
            }
            else
            {
                isCreateAble = true;
                switchMaterial.SwitchOtherMaterial(transparentObject, blockMaterialArray[(int)TransparentMaterialColor.GREEN_COLOR_MATERIAL]);
            }
        }
    }

    public void SetTransparentBlockTransform(RaycastHit hit, Block block)
    {
        Vector3 createDirection;
        Vector3 createBlockPosition;
        Vector3 point;

        point = hit.point - hit.transform.position;
        Debug.Log("point : " + point);

        createDirection = FindDirection(point, block.BlockSize);
        Debug.Log("createDirection : " + createDirection);

        Block getBlockObject = GetBlockObject();
        if (getBlockObject == default)
            return;

        Vector3 newBlockSize = getBlockObject.BlockSize;

        createBlockPosition = FindSpawnPosition(hit.transform.position, point, createDirection, newBlockSize);
        Debug.Log("createblockposition : " + createBlockPosition);

        if (FindBlocks(createBlockPosition, newBlockSize)) // 설치 위치에 블럭이 이미 존재하면 붉은 오브젝트 출력
        {
            if (isCreateAble == true)
                switchMaterial.SwitchOtherMaterial(transparentObject, blockMaterialArray[(int)TransparentMaterialColor.RED_COLOR_MATERIAL]);
        }
        else
        {
            if (isCreateAble == false)
                switchMaterial.SwitchOtherMaterial(transparentObject, blockMaterialArray[(int)TransparentMaterialColor.GREEN_COLOR_MATERIAL]);
        }

        if (transparentObject.transform.position != createBlockPosition)
            transparentObject.transform.position = createBlockPosition;

        transparentObject.transform.GetChild(0).localRotation = Quaternion.Euler(0, currentRotationAngle, 0);
    }

    public int FindPrefabLength()
    {
        switch (selectObjctType)
        {
            case (int)MapType.BLOCK:
                return BlockManager.Instance.BlockPrefabArray.Length;
            case (int)MapType.TURRET:
                return TurretManager.Instance.TurretPrefabArray.Length;
            case (int)MapType.BARRICADE:
                return BarricadeManager.Instance.BarricadePrefabArray.Length;
            default:
                break;
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
        if (GameManager.Instance == null || GameManager.Instance.IsPause)
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

    private class SwitchMaterial
    {
        private List<Renderer> saveRenderer = new List<Renderer>();
        private List<Material> saveMaterial = new List<Material>();

        public bool IsEmptySaveRenderer()
        {
            if (saveRenderer.Count == 0)
                return true;
            else
                return false;
        }

        public void Init()
        {
            saveRenderer.Clear();
            saveMaterial.Clear();
        }

        public void SwitchSaveMaterial(GameObject gameobject, int index = 0)
        {
            if (saveRenderer[index] != null)
            {
                saveRenderer[index].material = saveMaterial[index];
            }

            for (int i = 0; i < gameobject.transform.childCount; i++)
            {
                index++;
                SwitchSaveMaterial(gameobject.transform.GetChild(i).gameObject, index);
            }

            if(saveRenderer.Count != 0)
            {
                Init();
            }
        }

        public void SaveMaterial(GameObject gameobject) // 게임 오브젝트의 material 저장
        {
            Renderer renderer = gameobject.GetComponent<Renderer>();
            if(renderer == null)
            {
                saveRenderer.Add(null);
                saveMaterial.Add(null);
            }
            else
            {
                saveRenderer.Add(renderer);
                saveMaterial.Add(renderer.material);
            }

            for (int i = 0; i < gameobject.transform.childCount; i++)
            {
                SaveMaterial(gameobject.transform.GetChild(i).gameObject);
            }
        }

        public void SwitchOtherMaterial(GameObject gameobject, Material material)
        {
            Renderer renderer = gameobject.GetComponent<Renderer>();

            if(renderer != null)
                renderer.material = material;

            for (int i = 0; i < gameobject.transform.childCount; i++)
            {
                SwitchOtherMaterial(gameobject.transform.GetChild(i).gameObject, material);
            }
        }
    }
}

public enum LayerNumbering
{
    DEFAULT = 0,
    BLOCK = 8,
}
