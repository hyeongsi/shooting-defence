using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{ 
    void Update()
    {
        AimTargetPosition();
    }

    void AimTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePos = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mousePos = hit.point;
        }
        mousePos.y = 1f;

        transform.position = mousePos;
    }
}
