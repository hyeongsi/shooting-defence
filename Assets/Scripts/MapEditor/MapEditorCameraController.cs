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

    private void Start()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = originPos;
        }

        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        transform.position += (transform.right * horizontal + transform.up * vertical).normalized * moveSpeed * Time.deltaTime;
    }

    private void ZoomCamera()
    {
        float mouseScrollDeltaY = Input.mouseScrollDelta.y;

        transform.position += transform.forward * mouseScrollDeltaY * zoomSpeed * Time.deltaTime;
    }

}
