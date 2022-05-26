using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SetValues : MonoBehaviour
{
    public int skinNumber;
    public int gunNumber;

    public static Player_SetValues Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
