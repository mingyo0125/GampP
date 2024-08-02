using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject uiManager = new GameObject();
                    instance = uiManager.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
