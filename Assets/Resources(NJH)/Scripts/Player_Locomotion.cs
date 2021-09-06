using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Locomotion : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;
    [SerializeField] Transform groundChecker;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform aimingTarget;
    [SerializeField] Player_CameraFunction cameraFunction;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    [SerializeField] LayerMask groundLayer;

    [SerializeField] Weapon_Gun weapon;

    public float smoothingSpeed = 5f;

    [Header("플레이어 상태")]
    public float horizontal;
    public float vertical;
    public bool sprintFlag;
    public bool aimFlag;
    public bool isGrounded;
    public bool rotateLock;

    [Header("플레이어 값")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 6.5f;
    public float aimMoveSpeed = 3f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public float stamina = 10f;

    Vector3 moveDirection;
    Vector3 velocity;

    private void Start()
    {
        cameraFunction.Initialize();
    }

    private void FixedUpdate()
    {
        Loco_Move();
        Loco_Rotate();
        GroundCheck();
        SetDirection();
        
    }

    private void Update()
    {
        cameraFunction.CameraUpdtate();
        Loco_Jump();
        Loco_UseWeapon();
    }
    
    void SetDirection()
    {
        moveDirection = cameraTransform.forward * vertical;
        moveDirection += cameraTransform.right * horizontal;
        moveDirection.y = 0f;   // 먼저 0으로 만들고 정규화 함
        moveDirection.Normalize();
    }

    void Loco_Rotate()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        aimingTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0f);

        float cameraYaxis = cameraTransform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, cameraYaxis, 0f), smoothingSpeed * Time.deltaTime);

        weapon.transform.forward = aimingTarget.forward;
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, groundLayer);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    #region 행동 관련
    void Loco_Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        sprintFlag = Input.GetButton("Sprint");
        aimFlag = cameraFunction.aimCamFlag;

        animator.SetFloat("Input_Horizontal", horizontal);
        animator.SetFloat("Input_Vertical", vertical);

        if (moveDirection.magnitude > 0f)
        {
            // 이동 시 입력 카메라 방향대로 회전하도록 잠금 해제
            rotateLock = false;

            // 조준 상태 이동
            if (aimFlag == true) 
            {
                characterController.Move(moveDirection * aimMoveSpeed * Time.deltaTime);
            }
            // 일반 이동
            else
            {
                if (sprintFlag == true)
                {
                    characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
                }
                characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            }
        }
        // 움직이지 않을 때 카메라 방향대로 회전하지 않도록 잠금
        rotateLock = true;
    }

    void Loco_Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded == true)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                // 쿨타임 돌았을 때 스태미나 소모해서 비행

            }
        }
    }

    void Loco_UseWeapon()
    {
        weapon.WeaponKeyInput();
    }

    #endregion

}
