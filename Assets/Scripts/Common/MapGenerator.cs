using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private GameObject childBlockGameObject;
    private const string BLOCK_TAG_NAME = "NormalBlock";

    public Transform[] tilePrefab;
    public uint selectPrefab;

    private Vector3 FindDirection(Vector3 point)
    {
        Vector3 pointVector = Vector3.zero;

        if(point.x == 0)
        {
            pointVector = Vector3.left;
        }
        else if(point.y == 0)
        {
            pointVector = Vector3.down;
        }
        else if(point.z == 0)
        {
            pointVector = Vector3.back;
        }
        else if(point.x == 1)
        {
            pointVector = Vector3.right;
        }
        else if (point.y == 1)
        {
            pointVector = Vector3.up;
        }
        else if (point.z == 1)
        {
            pointVector = Vector3.forward;
        }

        return pointVector;
    }

    public Transform GenerateBlock(RaycastHit hit)         // 블럭 생성
    {
        Vector3 createPosition;

        if (selectPrefab >= 0 && selectPrefab < tilePrefab.Length)    // 투명 블럭은 생성하지 못하도록, 없는 블럭은 생성하지 못하도록
        {
            createPosition = FindDirection(hit.point - hit.transform.position);

            Transform newBlock = Instantiate(tilePrefab[selectPrefab], hit.transform.position + createPosition, Quaternion.Euler(Vector3.zero)) as Transform;
            newBlock.parent = childBlockGameObject.transform;

            return newBlock;
        }

        return null;
    }

    private void Start()
    {
        childBlockGameObject = new GameObject("Blocks");
        childBlockGameObject.transform.parent = transform;

        selectPrefab = 0;
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
                return;

            if (hit.transform.CompareTag(BLOCK_TAG_NAME))
            {
                GenerateBlock(hit);
            }
        }
    }
}
