using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SetSkin : MonoBehaviour
{
    [SerializeField] Mesh[] meshes;

    Player_Manager playerManager;
    SkinnedMeshRenderer skinnedMeshRenderer;

    void Start()
    {
        playerManager = GetComponentInParent<Player_Manager>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        SetSkin();
    }

    void SetSkin()
    {
        switch (playerManager.skinSelectNumber)
        {
            case 1:
                skinnedMeshRenderer.sharedMesh = meshes[0];
                break;
            case 2:
                skinnedMeshRenderer.sharedMesh = meshes[1];
                break;
            case 3:
                skinnedMeshRenderer.sharedMesh = meshes[2];
                break;
            case 4:
                skinnedMeshRenderer.sharedMesh = meshes[3];
                break;
            case 5:
                skinnedMeshRenderer.sharedMesh = meshes[4];
                break;
            case 6:
                skinnedMeshRenderer.sharedMesh = meshes[5];
                break;
            case 7:
                skinnedMeshRenderer.sharedMesh = meshes[6];
                break;
            case 8:
                skinnedMeshRenderer.sharedMesh = meshes[7];
                break;
            case 9:
                skinnedMeshRenderer.sharedMesh = meshes[8];
                break;
        }
    }
}
