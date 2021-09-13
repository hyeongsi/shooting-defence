using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Locomotion : MonoBehaviour
{
    public Camera camera;

    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;
    [SerializeField] Transform groundChecker;
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
    public bool moveFlag;
    public bool isGrounded;
    public bool rotateLock;

    [Header("플레이어 값")]
    public float moveSpeed;
    public float sprintSpeed;
    public float aimMoveSpeed;
    public float jumpHeight;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public float stamina;
    public float waitForRefillStamina;

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
        SetAnimatorParameter();
    }
    
    void SetDirection()
    {
        moveDirection = camera.transform.forward * vertical;
        moveDirection += camera.transform.right * horizontal;
        moveDirection.y = 0f;   // 먼저 0으로 만들고 정규화 함
        moveDirection.Normalize();
    }
    
    void SetAnimatorParameter()
    {
        animator.SetFloat("Input_Horizontal", horizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Input_Vertical", vertical, 0.1f, Time.deltaTime);
        animator.SetBool("SprintFlag", sprintFlag);
        animator.SetBool("AimFlag", aimFlag);
        animator.SetBool("MoveFlag", moveFlag);
        animator.SetBool("isGrounded", isGrounded);
    }

    void Loco_Rotate()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        aimingTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0f);

        float cameraYaxis = camera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, cameraYaxis, 0f), smoothingSpeed * Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, groundLayer);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f; // 땅으로 누름
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    #region 행동 관련
    void Loco_Move()
    {
        float speed = moveSpeed;
        moveFlag = false;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        sprintFlag = Input.GetButton("Sprint") && aimFlag == false && weapon.isShooting == false;
        aimFlag = cameraFunction.aimCamFlag;

        if (aimFlag == true)
        {
            speed = aimMoveSpeed;
            horizontal *= 0.5f;
            vertical *= 0.5f;
        }
        else if (sprintFlag == true && vertical > 0)
        {
            speed = sprintSpeed;
            horizontal *= 2f;
            vertical *= 2f;

        }

        if (moveDirection.magnitude > 0f)
        {
            moveFlag = true;
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        //if (moveDirection.magnitude > 0f)
        //{
        //    // 조준 상태 이동
        //    if (aimFlag == true)
        //    {
        //        characterController.Move(moveDirection * aimMoveSpeed * Time.deltaTime);
        //    }
        //    //일반 이동
        //    else
        //    {
        //        if (sprintFlag == true)
        //        {
        //            //스태미나 소모
        //            characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
        //        }
        //        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        //    }
        //}
    }

    void Loco_Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded == true)
            {
                animator.CrossFade("Player_JumpLoop", 0.25f);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                // 쿨타임 돌았을 때 스태미나 소모해서 비행

            }
        }
    }

    #region 스태미나 사용(작성 필요)
    void Loco_UseStamina()
    {

    }

    IEnumerator Co_UseStamina()
    {
        yield return null;
    }

    IEnumerator Co_ReFillStamina()
    {
        yield return new WaitForSeconds(waitForRefillStamina);
    }
    #endregion


    void Loco_UseWeapon()
    {
        weapon.WeaponKeyInput();
        animator.SetBool("FireFlag", weapon.isShooting);
    }

    #endregion

}
