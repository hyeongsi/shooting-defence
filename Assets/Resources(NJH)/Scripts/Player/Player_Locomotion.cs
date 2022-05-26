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
    [SerializeField] LayerMask groundLayer;

    [Header("플레이어 값")]
    public int lifePoint = 10;
    public float stamina = 100;
    public float moveSpeed;
    public float sprintSpeed;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;
    public float waitForChargeStamina;
    public float waitForChargeEmptyStamina;
    public float smoothingSpeed = 15f;

    bool useStaminaFlag;
    bool chargeStaminaDelayFlag;

    Vector3 velocity;

    public void Initialize()
    {
        playerManager = GetComponent<Player_Manager>();
        animator = playerManager.animator;
        characterController = GetComponent<CharacterController>();
    }

    public void FixedUpdateFunction()
    {
        CheckGround();
        Loco_Move();
    }

    public void UpdateFunction()
    {
        Loco_Rotate();
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

    //void CheckBehind()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        // 캐릭터를 기준으로 레이캐스트 맞은 위치를 로컬좌표로 변환
    //        Vector3 inverseTransform = behindChecker.InverseTransformPoint(hit.point);
    //        if (inverseTransform.z > 0) // 0보다 크면 앞에 있음
    //        {
    //            playerManager.isBehind = false;
    //            playerManager.disableAimPointImage.gameObject.SetActive(false);
    //            playerManager.aimPointImage.gameObject.SetActive(true);
    //        }
    //        else // 0보다 작으면 뒤에 있음
    //        {
    //            playerManager.isBehind = true;
    //            playerManager.disableAimPointImage.gameObject.SetActive(true);
    //            playerManager.aimPointImage.gameObject.SetActive(false);
    //        }
    //    }
    //}

    void Loco_Rotate()
    {
        Quaternion characterRotation;
        if(playerManager.GetMousePosition() != Vector3.zero)
        {
            characterRotation = Quaternion.LookRotation(playerManager.GetMousePosition());
            characterRotation.x = 0;
            characterRotation.z = 0;

            transform.rotation = characterRotation;
        }
        
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerManager.GetMousePosition()), 15f);
        //transform.forward = playerManager.GetMousePosition();
    }

    void Loco_Move()
    {
        float speed = moveSpeed;

        playerManager.moveFlag = playerManager.GetDirection().magnitude > 0 ? true : false;

        playerManager.horizontal = Input.GetAxisRaw("Horizontal");
        playerManager.vertical = Input.GetAxisRaw("Vertical");

        if (playerManager.sprintFlag == true && stamina > 0)
        {
            speed = sprintSpeed;
            playerManager.horizontal *= 2f;
            playerManager.vertical *= 2f;
        }

        useStaminaFlag = playerManager.sprintFlag 
                         && playerManager.moveFlag == true
                         ? true : false;

        // 이동
        if(playerManager.moveFlag == true)
        {
            characterController.Move(playerManager.GetDirection() * speed * Time.deltaTime);
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

    public void TakeDamage(float damage)
    {
        lifePoint -= 1;

        if (lifePoint <= 0)
        {

        }
    }
}
