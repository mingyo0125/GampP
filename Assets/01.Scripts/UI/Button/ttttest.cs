using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ttttest : NetworkBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.StartHost();
    }

}
