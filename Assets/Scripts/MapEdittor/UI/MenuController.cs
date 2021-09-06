using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private float MoveSize = 100.0f;

    public void OpenMenuBar()
    {
        transform.position = new Vector3(transform.position.x + MoveSize, transform.position.y, transform.position.z);
    }

    public void CloseMenuBar()
    {
        transform.position = new Vector3(transform.position.x - MoveSize, transform.position.y, transform.position.z);
    }
}