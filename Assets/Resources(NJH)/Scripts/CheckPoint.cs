using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static Transform[] checkPoint;

    public void SetCheckPoint()
    {
        checkPoint = new Transform[transform.childCount];

        for (int i = 0; i < checkPoint.Length; i++)
        {
            checkPoint[i] = transform.GetChild(i);
        }
    }
}
