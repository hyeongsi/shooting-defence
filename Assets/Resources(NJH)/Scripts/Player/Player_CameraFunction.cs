using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CameraFunction : MonoBehaviour
{
    [SerializeField] Animator cameraSelector;

    private int cameraBlendParameterID;

    public float cameraState;
    public bool aimCamFlag;
    public bool deadCamFlag;
    public bool spectateCamFlag;
    public bool tempJumpFlag;

    public void Initialize()
    {
        cameraSelector = GetComponent<Animator>();
        SetAnimationID();
    }

    public void CameraUpdtate()
    {
        Cam_Select();
        Cam_Lock();
    }

    private void SetAnimationID()
    {
        cameraState = 0f;
        cameraBlendParameterID = Animator.StringToHash("CameraParameter");
    }

    private void Cam_Select()
    {
        aimCamFlag = Input.GetButton("Aim");

        if(aimCamFlag == true)  // 조준
        {
            cameraState = 1f;
        }
        else if(deadCamFlag == true) // 사망
        {
            cameraState = 2f;
        }
        else if (spectateCamFlag == true) // 관전
        {
            cameraState = 3f;
        }
        else    // 기본
        {
            cameraState = 0f;
        }

        cameraSelector.SetFloat(cameraBlendParameterID, cameraState);
    }

    private void Cam_Lock()
    {
        // 특정 상황에서 카메라 움직이지 못하게 함
    }
}
