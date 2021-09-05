using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject childBlockGameObject = null;
    [SerializeField]
    private GameObject childBarricadeGameObject = null;
    [SerializeField]
    private GameObject childTurretGameObject = null;

    private Ray ray;
    private RaycastHit hit;
    private int rayermask;

    public Transform[] tilePrefab;
    private uint selectPrefab = 1;

    private bool FindBlocks(Vector3 point)
    {
        Vector3 newBlockSize = tilePrefab[selectPrefab].GetComponent<Block>().BlockSize;
        Collider[] collider = new Collider[1];  // 콜라이더 부분 null 하면 OverlapSphereNonAlloc 결과가 무조건 0만 나오게 되어 무조건 만들어 넣어주어 함
        Vector3 centerPoint = new Vector3();

        for(int y = 0; y < newBlockSize.y; y++)
        {
            centerPoint.y = point.y + newBlockSize.y / 2 + y;
            for (int x = 0; x < newBlockSize.x; x++)
            {
                centerPoint.x = point.x + newBlockSize.x / 2 + x;
                for (int z = 0; z < newBlockSize.z; z++)
                {
                    centerPoint.z = point.z + newBlockSize.z / 2 + z;
                    if (Physics.OverlapSphereNonAlloc(centerPoint, newBlockSize.y/3, collider, rayermask) != 0)
                        return true; 
                }
            }
        }

        return false;
    }
    private Vector3 FindDirection(Vector3 point, Vector3 blockSize)
    {
        Vector3 pointVector = Vector3.zero;
        Vector3 newBlockSize = tilePrefab[selectPrefab].GetComponent<Block>().BlockSize;

        // 왼쪽 하단 기준으로 생성되기 때문에, 크기가 큰 오브젝트의 경우, 해당 크기만큼 이동한 뒤 생성해야 함
        // float 계산 시 오차가 발생 하기 때문에, 0.01을 오차 범위로 두고 계산
        if (point.x >= -0.09f && point.x <= 0.01f)
        {
            pointVector.x = Vector3.left.x * newBlockSize.x;
            pointVector.y = Vector3.left.y * newBlockSize.y;
            pointVector.z = Vector3.left.z * newBlockSize.z;
        }
        else if (point.y >= -0.09f && point.y <= 0.01f)
        {
            pointVector.x = Vector3.down.x * newBlockSize.x;
            pointVector.y = Vector3.down.y * newBlockSize.y;
            pointVector.z = Vector3.down.z * newBlockSize.z;
        }
        else if (point.z >= -0.09f && point.z <= 0.01f)
        {
            pointVector.x = Vector3.back.x * newBlockSize.x;
            pointVector.y = Vector3.back.y * newBlockSize.y;
            pointVector.z = Vector3.back.z * newBlockSize.z;
        }
        else if(point.x >= (blockSize.x - 0.01f) && point.x <= (blockSize.x + 0.01f))
        {
            pointVector.x = Vector3.right.x * blockSize.x;
            pointVector.y = Vector3.right.y * blockSize.y;
            pointVector.z = Vector3.right.z * blockSize.z;
        }
        else if (point.y >= (blockSize.y - 0.01f) && point.y <= (blockSize.y + 0.01f))
        {
            pointVector.x = Vector3.up.x * blockSize.x;
            pointVector.y = Vector3.up.y * blockSize.y;
            pointVector.z = Vector3.up.z * blockSize.z;
        }
        else if (point.z >= (blockSize.z - 0.01f) && point.z <= (blockSize.z + 0.01f))
        {
            pointVector.x = Vector3.forward.x * blockSize.x;
            pointVector.y = Vector3.forward.y * blockSize.y;
            pointVector.z = Vector3.forward.z * blockSize.z;
        }

        return pointVector;
    }

    public void GenerateBlock(RaycastHit hit, Block block)         // 블럭 생성
    {
        Vector3 createPosition;

        if (selectPrefab >= 0 && selectPrefab < tilePrefab.Length)    // 없는 블럭은 생성하지 못하도록
        {
            createPosition = FindDirection(hit.point - hit.transform.position, block.BlockSize);
            if (FindBlocks(hit.transform.position + createPosition )) // 설치 위치에 블럭이 이미 존재하면 생성 X
                return;

            Transform newBlock = Instantiate(tilePrefab[selectPrefab], hit.transform.position + createPosition, Quaternion.Euler(Vector3.zero)) as Transform;
            switch(block.BlockType)     // 블록 유형에 따라 부모 오브젝트 설정, (정리)
            {
                case BlockType.BLOCK:
                    newBlock.parent = childBlockGameObject.transform;
                    break;
                case BlockType.BARRICADE:
                    newBlock.parent = childTurretGameObject.transform;
                    break;
                case BlockType.TURRET:
                    newBlock.parent = childBarricadeGameObject.transform;
                    break;
                default:
                    newBlock.parent = childBlockGameObject.transform;
                    return;
            } 
        }
    }
    private void Start()
    {
        rayermask = 1 << LayerMask.NameToLayer("Block");
    }

    private void Update()
    {  
        if (Input.GetMouseButtonUp(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayermask))
            {
                GenerateBlock(hit, hit.transform.GetComponent<Block>());
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayermask))
            {
                DestroyImmediate(hit.transform.gameObject);
            }
        }
    }
}
