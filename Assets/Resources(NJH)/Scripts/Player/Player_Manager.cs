using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Manager : MonoBehaviour
{
    Player_Locomotion playerLocomotion;
    Player_Animation playerAnimation;
    [SerializeField] Player_CameraFunction cameraFunction;
    [SerializeField] LayerMask checkThisLayer;

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
    public bool reloadFlag;
    public bool disableFlag;
    public bool isGrounded;
    public bool isBehind;
    public bool rotateLock;

    [Header("UI")]
    public Text bulletText;
    public Text reloadText;
    public Slider hpBar;
    public Slider staminaBar;
    public Sprite aimPointSprite;
    public Sprite disableAimPointSprite;

    public GameObject targetGuide;

    Vector3 moveDirection;

    private void Start()
    {
        Cursor.visible = false;

        targetGuide.SetActive(false);

        checkThisLayer = 1 << LayerMask.NameToLayer("Ground");

        playerLocomotion = GetComponent<Player_Locomotion>();
        playerAnimation = GetComponent<Player_Animation>();

        hpBar.maxValue = playerLocomotion.hp;
        staminaBar.maxValue = playerLocomotion.stamina;

        cameraFunction.Initialize();
        playerLocomotion.Initialize();
        playerAnimation.Initialize();
    }

    private void Update()
    {
        if(disableFlag == true || reloadFlag == true)
        {
            targetGuide.GetComponent<SpriteRenderer>().sprite = disableAimPointSprite;
        }
        else
        {
            targetGuide.GetComponent<SpriteRenderer>().sprite = aimPointSprite;
        }

        sprintFlag = Input.GetButton("Sprint") && aimFlag == false && weapon.isShooting == false;
        aimFlag = cameraFunction.aimCamFlag;

        hpBar.value = playerLocomotion.hp;
        staminaBar.value = playerLocomotion.stamina;

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
        moveDirection = new Vector3(horizontal, 0, vertical);
        return moveDirection.normalized;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        Vector3 lookDir = Vector3.zero;

        targetGuide.SetActive(false);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, checkThisLayer))
        {
            targetGuide.SetActive(true);
            targetGuide.transform.position = new Vector3(hit.point.x, hit.point.y + 0.8f, hit.point.z);
            lookDir = hit.point - transform.position;
            lookDir = new Vector3(lookDir.x, transform.position.y, lookDir.z);
        }

        return lookDir.normalized;
    }
}
