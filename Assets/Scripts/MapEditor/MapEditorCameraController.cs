using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorCameraController : MonoBehaviour
{
    private Vector3 originPos;
    private float moveSpeed = 4.0f;
    private float zoomSpeed = 4.0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float mouseScrollDeltaY = 0.0f;

    private void Start()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MapEditorController.Instance.IncreaseSpawnObjectAngle();
        }

        ObjectSpawnLoop();

        if (UIManager.Instance.PopupList.Count != 0)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = originPos;
        }

        MoveCamera();
        ZoomCamera();
    }

    private void ObjectSpawnLoop()
    {
        if (MapEditorController.Instance.IsSelectingObject)
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(cameraRay, out RaycastHit hitBlock, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block")))
            {
                PreviewObject(hitBlock.point);
                SpawnObject(hitBlock.point);
            }
            else
            {
                DeletePreviewObject();
            }

            if (Physics.Raycast(cameraRay, out RaycastHit hitObject, Mathf.Infinity, 1 << LayerMask.NameToLayer("Object")))
            {
                DeleteObject(hitObject);
            }
        }
    }

    private void DeletePreviewObject()
    {
        Destroy(MapEditorController.Instance.previewObject);
        MapEditorController.Instance.previewObject = null;
    }

    private void PreviewObject(Vector3 spawnPosition)
    {
        if(MapEditorController.Instance.previewObject == null)
        {
            MapEditorController.Instance.previewObject = Instantiate(ObjManager.Instance.GetObject(MapEditorController.Instance.SelectObjectIndex));
            MapEditorController.Instance.previewObject.transform.position = spawnPosition;
            MapEditorController.Instance.previewObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            MapEditorController.Instance.previewObject.transform.position = spawnPosition;
        }
    }

    private void SpawnObject(Vector3 spawnPosition) // 오브젝트 생성
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(MapEditorController.Instance.SelectObjectIndex == ObjManager.ObjName.Player_Spawner) // 플레이어 스포너 일 경우
            {
                GameObject spawnObject = Instantiate(ObjManager.Instance.GetObject(MapEditorController.Instance.SelectObjectIndex));
                spawnObject.transform.position = spawnPosition;
                spawnObject.transform.eulerAngles = new Vector3(0, MapEditorController.Instance.SpawnObjectAngle, 0);
                MapEditorController.Instance.GetCustomTileMap.SetPlayerSpawner(spawnObject, (int)MapEditorController.Instance.SelectObjectIndex);
            }
            else if (MapEditorController.Instance.SelectObjectIndex == ObjManager.ObjName.Enemy_Spawner) // 적 스포너
            {
                GameObject spawnObject = Instantiate(ObjManager.Instance.GetObject(MapEditorController.Instance.SelectObjectIndex));
                spawnObject.transform.position = spawnPosition;
                spawnObject.transform.eulerAngles = new Vector3(0, MapEditorController.Instance.SpawnObjectAngle, 0);
                MapEditorController.Instance.GetCustomTileMap.SetEnemySpawner(spawnObject, (int)MapEditorController.Instance.SelectObjectIndex);
            }
            else if (MapEditorController.Instance.SelectObjectIndex == ObjManager.ObjName.Enemy_GuideLine) // 적 이동경로
            {
                GameObject spawnObject = Instantiate(ObjManager.Instance.GetObject(MapEditorController.Instance.SelectObjectIndex));
                spawnObject.transform.position = spawnPosition;
                spawnObject.transform.eulerAngles = new Vector3(0, MapEditorController.Instance.SpawnObjectAngle, 0);
                MapEditorController.Instance.GetCustomTileMap.AddEnemyGuideLine(spawnObject, (int)MapEditorController.Instance.SelectObjectIndex);
            }
            else
            {
                GameObject spawnObject = Instantiate(ObjManager.Instance.GetObject(MapEditorController.Instance.SelectObjectIndex));
                spawnObject.transform.position = spawnPosition;
                spawnObject.transform.eulerAngles = new Vector3(0, MapEditorController.Instance.SpawnObjectAngle, 0);
                MapEditorController.Instance.GetCustomTileMap.AddObjectList(spawnObject, (int)MapEditorController.Instance.SelectObjectIndex);
                spawnObject.layer = LayerMask.NameToLayer("Object");
            }
        }
    }

    private void DeleteObject(RaycastHit hitObject) // 오브젝트 삭제
    {
        if (Input.GetMouseButtonDown(1))
        {
            MapEditorController.Instance.GetCustomTileMap.DeleteObjectList(hitObject.transform.gameObject);
            Destroy(hitObject.transform.gameObject);
        }  
    }

    private void MoveCamera()   // 카메라 x,z값 이동
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        transform.position += (transform.right * horizontal + transform.up * vertical).normalized * moveSpeed * Time.deltaTime;
    }

    private void ZoomCamera()   // 카메라 y값 이동
    {
        mouseScrollDeltaY = Input.mouseScrollDelta.y;

        transform.position += transform.forward * mouseScrollDeltaY * zoomSpeed * Time.deltaTime;
    }

}
