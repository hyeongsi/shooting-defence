using UnityEngine;
using UnityEngine.UI;

public class Player_Manager : MonoBehaviour
{
    Player_Locomotion playerLocomotion;
    Player_Animation playerAnimation;
    [SerializeField] Player_CameraFunction cameraFunction;
    [SerializeField] LayerMask checkThisLayer;

    public Animator animator;
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
    public bool isShooting;
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

        checkThisLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Block"));

        playerLocomotion = GetComponent<Player_Locomotion>();
        playerAnimation = GetComponent<Player_Animation>();
        weapon = GetComponentInChildren<Weapon_Gun>();

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

        sprintFlag = Input.GetButton("Sprint") && aimFlag == false && isShooting == false;
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, 0.3f);
    //}

    public GameObject GetTurretSpawnerCollision()
    {
        int mask = 0;
        mask = 1 << (LayerMask.NameToLayer("TurretSpawner"));

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.3f, mask);

        if (hitColliders.Length < 1)
            return null;
        else
            return hitColliders[0].gameObject;
    }

    public Vector3 GetDirection()
    {
        moveDirection = new Vector3(horizontal, 0, vertical);
        return moveDirection.normalized;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 lookDir = Vector3.zero;

        targetGuide.SetActive(false);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, checkThisLayer))
        {
            targetGuide.SetActive(true);
            targetGuide.transform.position = new Vector3(hit.point.x, hit.point.y + 0.8f, hit.point.z); // 조준점 위치
            lookDir = hit.point - transform.position;                                                   // 플레이어가 바라보는 방향(조준점 위치 - 자신의 위치)
            lookDir = new Vector3(lookDir.x, 0, lookDir.z);
        }

        return lookDir.normalized;
    }
}
