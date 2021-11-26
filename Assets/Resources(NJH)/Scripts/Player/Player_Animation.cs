using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player_Animation : MonoBehaviour
{
    Player_Manager playerManager;

    [Header("애니메이션 리그")]
    [SerializeField] Rig rigLayer_WeaponFire;
    [SerializeField] Rig rigLayer_WeaponSprint;
    [SerializeField] Rig rigLayer_WeaponAim;
    [SerializeField] Rig rigLayer_WeaponReload;

    Animator animator;

    public float transitionSpeed = 10f;

    [Header("애니메이션 파라미터")]
    int horizontalID;
    int verticalID;
    int sprintID;
    int aimID;
    int fireID;
    int moveID;
    int groundedID;

    public void Initialize()
    {
        playerManager = GetComponent<Player_Manager>();
        animator = playerManager.animator;
        SetAnimationID();
    }
    public void UpdateFunction()
    {
        SetAnimatorParameter();
        RigUpdate();
    }

    void SetAnimationID()
    {
        horizontalID = Animator.StringToHash("Input_Horizontal");
        verticalID = Animator.StringToHash("Input_Vertical");
        sprintID = Animator.StringToHash("SprintFlag");
        aimID = Animator.StringToHash("AimFlag");
        fireID = Animator.StringToHash("FireFlag");
        moveID = Animator.StringToHash("MoveFlag");
        groundedID = Animator.StringToHash("isGrounded");
    }

    void SetAnimatorParameter()
    {
        animator.SetFloat(horizontalID, playerManager.horizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(verticalID, playerManager.vertical, 0.1f, Time.deltaTime);
        animator.SetBool(sprintID, playerManager.sprintFlag);
        animator.SetBool(aimID, playerManager.aimFlag);
        animator.SetBool(fireID, playerManager.weapon.isShooting);
        animator.SetBool(moveID, playerManager.moveFlag);
        animator.SetBool(groundedID, playerManager.isGrounded);
        animator.SetBool("FireFlag", playerManager.weapon.isShooting);
    }

    void RigUpdate()
    {
        // 달리기
        if (playerManager.sprintFlag == true)
        {
            rigLayer_WeaponSprint.weight += Time.deltaTime * transitionSpeed;
        }
        else
        {
            rigLayer_WeaponSprint.weight -= Time.deltaTime * transitionSpeed;
        }

        // 재장전
        if(playerManager.reloadFlag == true)
        {
            rigLayer_WeaponReload.weight += transitionSpeed * Time.deltaTime;
        }
        else
        {
            rigLayer_WeaponReload.weight -= transitionSpeed * Time.deltaTime;
        }

        // 사격
        if (playerManager.isShooting == true)
        {
            rigLayer_WeaponFire.weight += Time.deltaTime * transitionSpeed;
        }
        else
        {
            rigLayer_WeaponFire.weight -= Time.deltaTime * transitionSpeed;
        }

        // 조준
        if (playerManager.aimFlag == true)
        {
            rigLayer_WeaponAim.weight += Time.deltaTime * transitionSpeed;
        }
        else
        {
            rigLayer_WeaponAim.weight -= Time.deltaTime * transitionSpeed;
        }
    }
}
