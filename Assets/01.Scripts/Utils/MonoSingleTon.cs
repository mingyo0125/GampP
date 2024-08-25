using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class MonoSingleTon<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();

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
