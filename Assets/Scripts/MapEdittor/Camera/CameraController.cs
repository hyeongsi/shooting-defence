using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float moveSpeed = 4.0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;

    #region RotationCameraAngleVariable
    private float turnSpeed = 130.0f;
    private float yRotationSize = 0.0f;
    private float yRotate = 0.0f;
    private float xRotationSize = 0.0f;
    private float xRotate = 0.0f;

    private Vector3 rotationAngle = Vector3.zero;
    #endregion

    private void MoveCamera()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        transform.position += (transform.right * horizontal + transform.forward * vertical).normalized * moveSpeed * Time.deltaTime;
    }

    private void RotationCameraAngle()
    {
        yRotationSize = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        yRotate = transform.eulerAngles.y + yRotationSize;

        xRotationSize = Input.GetAxisRaw("Mouse Y") * turnSpeed * Time.deltaTime;
        xRotate = Mathf.Clamp(xRotate - xRotationSize, -45, 80);

        rotationAngle.x = xRotate;
        rotationAngle.y = yRotate;
        rotationAngle.z = 0;

        transform.eulerAngles = rotationAngle;
    }

    private void Update()
    {
        if (GameManager.Instance.IsStop)
            return;

        MoveCamera();
        RotationCameraAngle();
    }

}
