using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Locomotion : MonoBehaviour
{
    Player_Manager playerManager;
    Animator animator;

    CharacterController characterController;
    [SerializeField] Transform groundChecker;
    [SerializeField] Transform aimingTarget;

    [SerializeField] LayerMask groundLayer;

    [Header("플레이어 값")]
    public float moveSpeed;
    public float sprintSpeed;
    public float aimMoveSpeed;
    public float jumpHeight;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;
    public float stamina;
    public float waitForChargeStamina;
    public float smoothingSpeed = 15f;

    public bool chargeStaminaFlag;
    public bool useStaminaFlag;

    Vector3 velocity;

    public void Initialize()
    {
        playerManager = GetComponent<Player_Manager>();
        animator = playerManager.animator;
        characterController = GetComponent<CharacterController>();
    }

    public void FixedUpdateFunction()
    {
        GroundCheck();
        Loco_Move();
    }

    public void UpdateFunction()
    {
        Loco_Rotate();
        Loco_Jump();
        Loco_UseWeapon();
    }

    void GroundCheck()
    {
        playerManager.isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, groundLayer);
        if (playerManager.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f; // 땅으로 누름
            characterController.stepOffset = 0.35f;
        }
        else
        {
            characterController.stepOffset = 0;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void Loco_Rotate()
    {
        float mouseX = playerManager.xAxis.Value;
        float mouseY = playerManager.yAxis.Value;

        aimingTarget.eulerAngles = new Vector3(mouseY, mouseX, 0f);

        Quaternion mouseRotate = Quaternion.Euler(0f, mouseX, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, mouseRotate, smoothingSpeed * Time.fixedDeltaTime);
    }

    void Loco_Move()
    {
        playerManager.SetDirection();

        float speed = moveSpeed;
        playerManager.moveFlag = false;

        playerManager.horizontal = Input.GetAxisRaw("Horizontal");
        playerManager.vertical = Input.GetAxisRaw("Vertical");

        if (playerManager.aimFlag == true)
        {
            speed = aimMoveSpeed;
            playerManager.horizontal *= 0.5f;
            playerManager.vertical *= 0.5f;
        }
        else if (playerManager.sprintFlag == true && playerManager.vertical > 0 && stamina > 0)
        {
            speed = sprintSpeed;
            playerManager.horizontal *= 2f;
            playerManager.vertical *= 2f;
        }

        useStaminaFlag = playerManager.sprintFlag 
                         && playerManager.moveDirection.magnitude > 0f
                         && playerManager.vertical > 0
                         ? true : false;

        // 이동
        if (playerManager.moveDirection.magnitude > 0f)
        {
            if(playerManager.sprintFlag == true && playerManager.vertical > 0)
            {
                UseStamina();
            }

            playerManager.moveFlag = true;
            characterController.Move(playerManager.moveDirection * speed * Time.deltaTime);
        }
        ChargeStamina();
    }

    void Loco_Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (playerManager.isGrounded == true)
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

    #region 스태미나 사용
    void UseStamina()
    {
        if(stamina > 0 && useStaminaFlag == true)
        {
            stamina -= 15f * Time.deltaTime;
            if(stamina < 0)
            {
                stamina = 0;
            }
        }

        if (stamina <= 0 && chargeStaminaFlag == true)
        {
            StopCoroutine(Co_ChargeStaminaDelay());
            StartCoroutine(Co_ChargeStaminaDelay());
        }
    }

    void ChargeStamina()
    {
        if (stamina < 100 && useStaminaFlag == false)
        {
            stamina += 10f * Time.deltaTime;
            if(stamina > 100)
            {
                stamina = 100;
            }
        }
    }

    IEnumerator Co_ChargeStaminaDelay()
    {
        Debug.Log("Delay Start");
        chargeStaminaFlag = false;
        float delay = waitForChargeStamina;
        while(delay > 0)
        {
            delay -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        chargeStaminaFlag = true;
    }
    #endregion

    void Loco_UseWeapon()
    {
        playerManager.weapon.WeaponKeyInput();
    }
}
