using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Locomotion : MonoBehaviour
{
    Player_Manager playerManager;
    Animator animator;

    CharacterController characterController;
    [SerializeField] Transform groundChecker;
    [SerializeField] Transform behindChecker;
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
    public float waitForChargeEmptyStamina;
    public float smoothingSpeed = 15f;

    public bool chargeStaminaFlag;
    public bool useStaminaFlag;
    public bool chargeStaminaDelayFlag;

    Vector3 velocity;

    public void Initialize()
    {
        playerManager = GetComponent<Player_Manager>();
        animator = playerManager.animator;
        characterController = GetComponent<CharacterController>();
    }

    public void FixedUpdateFunction()
    {
        CheckBehind();
        CheckGround();
        Loco_Move();
    }

    public void UpdateFunction()
    {
        Loco_Rotate();
        Loco_Jump();
        Loco_UseWeapon();
        Loco_Stamina();
    }

    
    void CheckGround()
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

    void CheckBehind()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 캐릭터를 기준으로 레이캐스트 맞은 위치를 로컬좌표로 변환
            Vector3 inverseTransform = behindChecker.InverseTransformPoint(hit.point);
            if (inverseTransform.z > 0) // 0보다 크면 앞에 있음
            {
                playerManager.isBehind = false;
                playerManager.disableAimPointImage.gameObject.SetActive(false);
                playerManager.aimPointImage.gameObject.SetActive(true);

            }
            else // 0보다 작으면 뒤에 있음
            {
                playerManager.isBehind = true;
                playerManager.disableAimPointImage.gameObject.SetActive(true);
                playerManager.aimPointImage.gameObject.SetActive(false);
            }
        }
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
            playerManager.moveFlag = true;
            characterController.Move(playerManager.moveDirection * speed * Time.deltaTime);
        }
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
        }
    }

    void Loco_Stamina()
    {
        UseStamina();
        ChargeStamina();
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
    }

    void ChargeStamina()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) && useStaminaFlag == true)    // 스태미나 쓰다가 버튼을 떼야 실행
        {
            if(stamina <= 0)
            {
                StartCoroutine(Co_ChargeStaminaDelay(waitForChargeEmptyStamina));
            }
            else
            {
                StartCoroutine(Co_ChargeStaminaDelay(waitForChargeStamina));
            }
        }

        if (stamina < 100 && useStaminaFlag == false && chargeStaminaDelayFlag == false)
        {
            stamina += 10f * Time.deltaTime;
            if(stamina > 100)
            {
                stamina = 100;
            }
        }
    }

    IEnumerator Co_ChargeStaminaDelay(float delayTime)
    {
        Debug.Log("Delay Start, " + delayTime);
        chargeStaminaDelayFlag = true;
        float delay = delayTime;
        while(delay > 0)
        {
            delay -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        chargeStaminaDelayFlag = false;
    }
    #endregion

    void Loco_UseWeapon()
    {
        if(playerManager.isBehind == true)
        {
            return;
        }
        playerManager.weapon.WeaponKeyInput();
    }
}
