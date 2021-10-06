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

    [Header("플레이어 상태")]
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

    Vector3 moveDirection;

    private void Start()
    {
        //Cursor.visible = false;

        playerLocomotion = GetComponent<Player_Locomotion>();
        playerAnimation = GetComponent<Player_Animation>();

        cameraFunction.Initialize();
        playerLocomotion.Initialize();
        playerAnimation.Initialize();
    }

    private void Update()
    {
        staminaText.text = playerLocomotion.stamina.ToString();

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
    public Vector3 GetDirection()
    {
        //moveDirection = camera.transform.forward * vertical;
        //moveDirection += camera.transform.right * horizontal;
        //moveDirection.y = 0f;   // 먼저 0으로 만들고 정규화 함
        //moveDirection.Normalize();

        moveDirection = new Vector3(horizontal, 0, vertical);

        return moveDirection.normalized;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        Vector3 lookDir = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            lookDir = hit.point - transform.position;
            lookDir = new Vector3(lookDir.x, transform.position.y, lookDir.z);
        }

        return lookDir.normalized;
    }
}
