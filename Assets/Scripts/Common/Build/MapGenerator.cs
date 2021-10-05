using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private void JudgeChangeMaterial(Vector3 createBlockPosition, Vector3 newBlockSize, Vector3 direction, Block block)
    {
        if (FindBlocks(createBlockPosition, newBlockSize)) // 설치 위치에 블럭이 이미 존재하면 붉은 오브젝트 출력
        {
            isCreateAble = false;
            switchMaterial.SwitchOtherMaterial(transparentObject, blockMaterialArray[(int)TransparentMaterialColor.RED_COLOR_MATERIAL]);
        }
        else  // 아니면 초록 오브젝트 출력
        {
            bool isSame;
            switch(GetBlockObject().BuildBlockTypeVar)
            {
                case BuildBlockType.UP:
                    isSame = (direction == Vector3.up);
                    break;
                case BuildBlockType.DOWN:
                    isSame = (direction == Vector3.down);
                    break;
                case BuildBlockType.SIDE:
                    isSame = (direction == Vector3.right || direction == Vector3.left || direction == Vector3.forward || direction == Vector3.back);
                    break;
                default:
                    isSame = true;
                    break;
            }

            if (isSame && block.BlockTypeVar == BlockType.BLOCK)
            {
                isCreateAble = true;
                switchMaterial.SwitchOtherMaterial(transparentObject, blockMaterialArray[(int)TransparentMaterialColor.GREEN_COLOR_MATERIAL]);
                return;
            }
            else
            {
                isCreateAble = false;
                switchMaterial.SwitchOtherMaterial(transparentObject, blockMaterialArray[(int)TransparentMaterialColor.RED_COLOR_MATERIAL]);
                return;
            }
        }
    }

    private Block GetBlockObject()
    {
        switch (selectObjctType)
        {
            case (int)MapType.BLOCK:
                return BlockManager.Instance.BlockArray[selectPrefab];
            case (int)MapType.TURRET:
                return TurretManager.Instance.TurretBlockArray[selectPrefab];
            case (int)MapType.BARRICADE:
                return BarricadeManager.Instance.BarricadeBlockArray[selectPrefab];
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
        const float ZERO = 0.0f;

        // float 계산 시 오차가 발생 하기 때문에, 0.01을 오차 범위로 두고 계산
        if ((point.x > blockSize.x && point.x < blockSize.x) || Mathf.Approximately(point.x ,blockSize.x))
        {
            pointVector = Vector3.right;
        }
        else if ((point.y > blockSize.y && point.y < blockSize.y) || Mathf.Approximately(point.y, blockSize.y))
        {
            pointVector = Vector3.up;
        }
        else if ((point.z > blockSize.z && point.z < blockSize.z) || Mathf.Approximately(point.z, blockSize.z))
        {
            pointVector = Vector3.forward;
        }
        else if ((point.x > ZERO && point.x < ZERO) || Mathf.Approximately(point.x, ZERO))
        {
            pointVector = Vector3.left;
        }
        else if ((point.y > ZERO && point.y < ZERO) || Mathf.Approximately(point.y, ZERO))
        {
            pointVector = Vector3.down;
        }
        else if ((point.z > ZERO && point.z < ZERO) || Mathf.Approximately(point.z, ZERO))
        {
            pointVector = Vector3.back;
        }

        return pointVector;
    }

    private Vector3 FindSpawnPosition(Block block, Vector3 point, Vector3 direction, Vector3 newBlockSize)
    {
        Vector3 pointVector = Vector3.zero;
        Vector3 position = block.transform.position;
        Vector3 notDirection = new Vector3();

        notDirection.x = Mathf.Approximately(direction.x, 1.0f) ? 0.0f : 1.0f;
        notDirection.y = Mathf.Approximately(direction.y, 1.0f) ? 0.0f : 1.0f;
        notDirection.z = Mathf.Approximately(direction.z, 1.0f) ? 0.0f : 1.0f;

        if (direction == Vector3.right || direction == Vector3.forward || direction == Vector3.up)
        {
            pointVector.x = position.x + (direction.x * block.BlockSize.x) + (notDirection.x * Mathf.Floor(point.x));
            pointVector.y = position.y + (direction.y * block.BlockSize.y) + (notDirection.y * Mathf.Floor(point.y));
            pointVector.z = position.z + (direction.z * block.BlockSize.z) + (notDirection.z * Mathf.Floor(point.z));
        }
        else if (direction == Vector3.left || direction == Vector3.back || direction == Vector3.down)
        {
            pointVector.x = position.x + (direction.x * newBlockSize.x) + (notDirection.x * Mathf.Floor(point.x));
            pointVector.y = position.y + (direction.y * newBlockSize.y) + (notDirection.x * Mathf.Floor(point.y));
            pointVector.z = position.z + (direction.z * newBlockSize.z) + (notDirection.x * Mathf.Floor(point.z));
        }
        else
            return default;

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
        switchMaterial.Init();

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
            Vector3 hitGameObjectPosition = hit.transform.position;
            point = hit.point - hitGameObjectPosition;

            createDirection = FindDirection(point, block.BlockSize);

            Vector3 newBlockSize;
            Block getBlockObject = GetBlockObject();
            if (getBlockObject == default)
                return;

            newBlockSize = getBlockObject.BlockSize;

            createBlockPosition = FindSpawnPosition(block, point, createDirection, newBlockSize);
            if (createBlockPosition == hitGameObjectPosition)
                return;

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
            {
                switchMaterial.Init();
                switchMaterial.SaveMaterial(transparentObject);
            }

            JudgeChangeMaterial(createBlockPosition, newBlockSize, createDirection, block);
        }
    }

    public void SetTransparentBlockTransform(RaycastHit hit, Block block)
    {
        Vector3 createDirection;
        Vector3 createBlockPosition;
        Vector3 point;

        Vector3 hitGameObjectPosition = hit.transform.position;
        point = hit.point - hitGameObjectPosition;

        createDirection = FindDirection(point, block.BlockSize);

        Block getBlockObject = GetBlockObject();
        if (getBlockObject == default)
            return;

        Vector3 newBlockSize = getBlockObject.BlockSize;

        createBlockPosition = FindSpawnPosition(block, point, createDirection, newBlockSize);
        if (createBlockPosition == hitGameObjectPosition)
            return;

        JudgeChangeMaterial(createBlockPosition, newBlockSize, createDirection, block);

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
            if(!switchMaterial.IsEmptySaveRenderer())
            {
                switchMaterial.SwitchSaveMaterial(transparentObject);
                switchMaterial.Init();
            }

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
        private int index = 0;

        public bool IsEmptySaveRenderer()
        {
            return (saveRenderer.Count == 0);
        }

        public void Init()
        {
            saveRenderer.Clear();
            saveMaterial.Clear();
        }

        public void SwitchSaveMaterial(GameObject gameobject, bool isFirst = true)
        {
            if (isFirst)
                index = 0;
            else
                index += 1;

            if (saveRenderer[index] != default)
            {
                saveRenderer[index].material = saveMaterial[index];
            }

            for (int i = 0; i < gameobject.transform.childCount; i++)
            {
                SwitchSaveMaterial(gameobject.transform.GetChild(i).gameObject, false);
            }
        }

        public void SaveMaterial(GameObject gameobject) // 게임 오브젝트의 material 저장
        {
            Renderer renderer = gameobject.GetComponent<Renderer>();
            Material newMaterial;
            if (renderer == null)
            {
                saveRenderer.Add(default);
                saveMaterial.Add(default);
            }
            else
            {
                newMaterial = new Material(renderer.material);
                saveRenderer.Add(renderer);
                saveMaterial.Add(newMaterial);
            }

            for (int i = 0; i < gameobject.transform.childCount; i++)
            {
                SaveMaterial(gameobject.transform.GetChild(i).gameObject);
            }
        }

        public void SwitchOtherMaterial(GameObject gameobject, Material material, bool isFirst = true)
        {
            if (isFirst)
                index = 0;
            else
                index += 1;

            if (saveRenderer[index] != default)
            {
                saveRenderer[index].material = material;
            }

            for (int i = 0; i < gameobject.transform.childCount; i++)
            {
                SwitchOtherMaterial(gameobject.transform.GetChild(i).gameObject, material, false);
            }
        }
    }
}

public enum LayerNumbering
{
    DEFAULT = 0,
    BLOCK = 8,
}
