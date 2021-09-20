using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager
{
    static TurretManager instance = null;

    public static TurretManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new TurretManager();
            }

            return instance;
        }
    }
}
