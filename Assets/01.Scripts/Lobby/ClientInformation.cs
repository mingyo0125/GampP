using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ClientInformation
{
    private bool isServer;
    public bool IsServer => isServer;

    public ClientInformation(bool isServer)
    {
        this.isServer = isServer;
    }
}
