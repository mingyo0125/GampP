using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DummyPlayerSpawner : PlayerSpawner
{
    private Action<ulong> OnClientConnectedCallback = null;
    private Action<ulong> OnClientDisconnectCallback = null;

    protected override void SubscribeCallbacks()
    {
        OnClientConnectedCallback = null;
        OnClientDisconnectCallback = null;

        OnClientConnectedCallback += OnClientConnected;
        OnClientDisconnectCallback += OnClientDisconnected;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    protected override void Start()
    {
        SpawnPlayer(NetworkManager.Singleton.LocalClientId);
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer && NetworkManager.Singleton.ConnectedClients.Count >= 2)
        {
            SpawnDummyPlayerClientRpc(clientId);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        DestroyDummyPlayer(clientId);
    }

    private void DestroyDummyPlayer(ulong clientId)
    {
        GameObject disconnectedPlayer = GameObject.Find(clientId.ToString());
        Destroy(disconnectedPlayer);
        playersCount--;
    }

    [ClientRpc]
    private void SpawnDummyPlayerClientRpc(ulong clientId)
    {
        SpawnPlayer(clientId);
    }
}
