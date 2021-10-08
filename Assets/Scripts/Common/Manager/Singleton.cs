using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = (T)FindObjectOfType(typeof(T));  // T타입 오브젝트 있는지 찾기 

            if(instance == null)
            {
                GameObject singleton = new GameObject();
                instance = singleton.AddComponent<T>();
                singleton.name = "(singleton)" + typeof(T).ToString();

                DontDestroyOnLoad(singleton);
            }

            return instance;
        }
    }
}
