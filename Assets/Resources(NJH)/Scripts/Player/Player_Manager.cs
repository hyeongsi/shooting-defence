using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Manager : MonoBehaviour
{
    Player_Locomotion playerLocomotion;
    Player_Animation playerAnimation;
    [SerializeField] Player_CameraFunction cameraFunction;

    public Animator animator;
    public Camera camera;
    public Weapon_Gun weapon;

    [Header("방향 입력")]
    public float horizontal;
    public float vertical;

    [Header("마우스 입력")]
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    [Header("플레이어 상태")]
    public float hp;
    public bool sprintFlag;
    public bool aimFlag;
    public bool moveFlag;
    public bool isGrounded;
    public bool isBehind;
    public bool rotateLock;

    [Header("UI")]
    public Text bulletText;
    public Text reloadText;
    public Text staminaText;
    public Image aimPointImage;
    public Image disableAimPointImage;


    public Vector3 moveDirection;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerLocomotion = GetComponent<Player_Locomotion>();
        playerAnimation = GetComponent<Player_Animation>();

        cameraFunction.Initialize();
        playerLocomotion.Initialize();
        playerAnimation.Initialize();
    }

    private void Update()
    {
        staminaText.text = playerLocomotion.stamina.ToString();

        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        sprintFlag = Input.GetButton("Sprint") && aimFlag == false && weapon.isShooting == false;
        aimFlag = cameraFunction.aimCamFlag;

        playerLocomotion.UpdateFunction();
        playerAnimation.UpdateFunction();
        cameraFunction.CameraUpdtate();
    }

    private void FixedUpdate()
    {
        playerLocomotion.FixedUpdateFunction();
    }
    public void SetDirection()
    {
        moveDirection = camera.transform.forward * vertical;
        moveDirection += camera.transform.right * horizontal;
        moveDirection.y = 0f;   // 먼저 0으로 만들고 정규화 함
        moveDirection.Normalize();
    }

    public void takeDamage()
    {
        
    }
}
