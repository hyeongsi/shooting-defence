using UnityEngine;
using UnityEngine.UI;

public class Player_Manager : MonoBehaviour
{
    Player_SetValues PlayerSetValues;
    Player_Locomotion playerLocomotion;
    Player_Animation playerAnimation;
    [SerializeField] Player_CameraFunction cameraFunction;
    [SerializeField] LayerMask checkThisLayer;

    public Animator animator;
    public Weapon_Gun weapon;

    [Header("방향 입력")]
    public float horizontal;
    public float vertical;

    [Header("애니메이션 전용 방향")]
    public float animHorizontal;
    public float animVertical;

    [Header("플레이어 설정값")]
    public GameObject[] weapon_Guns;
    public int skinSelectNumber;
    public int gunSelectNumber = 0;

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
    public bool triggerLock;

    [Header("UI")]
    public Text bulletText;
    public Text reloadText;
    public Slider staminaBar;
    public Sprite aimPointSprite;
    public Sprite disableAimPointSprite;

    public GameObject targetGuide;

    Vector3 moveDirection;
    public Vector3 animDirection;
    public float mouseHorizontal;
    public float mouseVertical;

    private void Awake()
    {
        PlayerSetValues = FindObjectOfType<Player_SetValues>();
        
        // 선택한 스킨, 무기 사용
        if (PlayerSetValues != null)
        {
            skinSelectNumber = PlayerSetValues.skinNumber;
            gunSelectNumber = PlayerSetValues.gunNumber;
        }

        if (gunSelectNumber == 0)
        {
            weapon_Guns[0].gameObject.SetActive(true);
            weapon = weapon_Guns[0].GetComponent<Weapon_Gun>();
        }
        else
        {
            weapon_Guns[1].gameObject.SetActive(true);
            weapon = weapon_Guns[1].GetComponent<Weapon_Gun>();
        }
    }

    private void Start()
    {
        Cursor.visible = false;

        targetGuide.SetActive(false);

        checkThisLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Block"));

        playerLocomotion = GetComponent<Player_Locomotion>();
        playerAnimation = GetComponent<Player_Animation>();
        
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

        mouseHorizontal = lookDir.normalized.x;
        mouseVertical = lookDir.normalized.z;

        animHorizontal = (lookDir.normalized.x + lookDir.normalized.z) * horizontal;
        animVertical = (lookDir.normalized.x + lookDir.normalized.z) * vertical;

        return lookDir.normalized;
    }
}
