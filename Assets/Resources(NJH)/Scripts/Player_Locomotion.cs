using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Locomotion : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Transform groundChecker;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Player_CameraFunction cameraFunction;

    [SerializeField] LayerMask groundLayer;

    [SerializeField] Weapon_Gun weapon;

    public float smoothingSpeed = 5f;

    [Header("플레이어 상태")]
    public float horizontal;
    public float vertical;
    public bool sprintFlag;
    public bool aimFlag;
    public bool isGrounded;

    [Header("플레이어 값")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float aimMoveSpeed = 2f;
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
        Loco_CharacterRotate();
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
        moveDirection.y = 0f;   // 반드시 먼저 0으로 만들고 정규화 해야함
        moveDirection.Normalize();
    }

    void Loco_CharacterRotate()
    {
        // moveDirection이 Vector3.zero면 입력이 없음 => 방향을 그대로 유지
        if (moveDirection == Vector3.zero)
        {
            moveDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothingSpeed * Time.deltaTime);
        transform.rotation = playerRotation;

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

        if (moveDirection.magnitude > 0f)
        {
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
