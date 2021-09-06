﻿using System.Collections;
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
    private uint selectPrefab = 0;
    private float currentRotationAngle = 0;
    private const float rotationAngle = 90.0f;

    private bool isEditMode = true;

    public bool IsEditMode
    {
        get { return isEditMode; }
        set { isEditMode = value; }
    }

    private bool FindBlocks(Vector3 point, Vector3 newBlockSize)
    {
        const float CHECK_RADIUS_SIZE = 0.33f;
        Vector3 findBlocksVector;
        Collider[] collider = new Collider[1];  // 콜라이더 부분 null 하면 OverlapSphereNonAlloc 결과가 무조건 0만 나오게 되어 무조건 만들어 넣어주어 함

        if (Physics.OverlapSphereNonAlloc(point, CHECK_RADIUS_SIZE, collider, rayermask) != 0)
            return true;

        for (int y = 1; y < (newBlockSize.y / 2); y++)
        {
            findBlocksVector = point;
            findBlocksVector.y += y;
            for (int x = 1; x < (newBlockSize.x / 2); x++)
            {
                findBlocksVector.x += x;
                for (int z = 1; z < (newBlockSize.z / 2); z++)
                {
                    findBlocksVector.z += z;
                    if (Physics.OverlapSphereNonAlloc(findBlocksVector, CHECK_RADIUS_SIZE, collider, rayermask) != 0)
                        return true;
                }
            }
        }

        for (int y = -1; y > (-newBlockSize.y / 2); y--)
        {
            findBlocksVector = point;
            findBlocksVector.y += y;
            for (int x = -1; x > (-newBlockSize.x / 2); x--)
            {
                findBlocksVector.x += x;
                for (int z = -1; z > (-newBlockSize.z / 2); z--)
                {
                    findBlocksVector.z += z;
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
        if(point.x >= (blockSize.x/2 - 0.01f) && point.x <= (blockSize.x / 2 + 0.01f))
        {
            pointVector = Vector3.right;
        }
        else if (point.y >= (blockSize.y/2 - 0.01f) && point.y <= (blockSize.y/2 + 0.01f))
        {
            pointVector = Vector3.up;
        }
        else if (point.z >= (blockSize.z / 2 - 0.01f) && point.z <= (blockSize.z / 2 + 0.01f))
        {
            pointVector = Vector3.forward;
        }
        else if (point.x >= (-blockSize.x / 2 - 0.01f) && point.x <= (-blockSize.x / 2 + 0.01f))
        {
            pointVector = Vector3.left;
        }
        else if (point.y >= (-blockSize.y / 2 - 0.01f) && point.y <= (-blockSize.y / 2 + 0.01f))
        {
            pointVector = Vector3.down;
        }
        else if (point.z >= (-blockSize.z / 2 - 0.01f) && point.z <= (-blockSize.z / 2 + 0.01f))
        {
            pointVector = Vector3.back;
        }

        return pointVector;
    }

    public void GenerateBlock(RaycastHit hit, Block block)         // 블럭 생성
    {
        Vector3 createDirection;
        Vector3 createBlockPosition;

        if (selectPrefab >= 0 && selectPrefab < tilePrefab.Length)    // 없는 블럭은 생성하지 못하도록
        {
            createDirection = FindDirection(hit.point - hit.transform.position, block.BlockSize);
            Vector3 newBlockSize = tilePrefab[selectPrefab].GetComponent<Block>().BlockSize;

            // 생성될 블럭 위치 계산
            createBlockPosition.x = hit.transform.position.x + createDirection.x * newBlockSize.x;
            createBlockPosition.y = hit.transform.position.y + createDirection.y * newBlockSize.y;
            createBlockPosition.z = hit.transform.position.z + createDirection.z * newBlockSize.z;

            if (FindBlocks(createBlockPosition, newBlockSize)) // 설치 위치에 블럭이 이미 존재하면 생성 X
                return;

            Transform newBlock = Instantiate(tilePrefab[selectPrefab], createBlockPosition, Quaternion.Euler(Vector3.zero)) as Transform;
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

            newBlock.localRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        }
    }
    private void Start()
    {
        rayermask = 1 << LayerMask.NameToLayer("Block");
    }

    private void Update()
    {  
        if(Input.GetKeyDown(KeyCode.R))     // 블럭 회전
        {
            currentRotationAngle += rotationAngle;
        }

        if (Input.GetMouseButtonUp(0))      // 좌클릭, 블럭 생성
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayermask))
            {
                GenerateBlock(hit, hit.transform.GetComponent<Block>());
            }
        }

        if (Input.GetMouseButtonUp(1))      // 우클릭, 블럭 삭제
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayermask))
            {
                DestroyImmediate(hit.transform.gameObject);
            }
        }
    }
}