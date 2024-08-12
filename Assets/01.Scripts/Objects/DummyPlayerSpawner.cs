using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DummyPlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _player, _otherPlayer;

    string playerName;

    private int playersCount = 1;

    private Action<ulong> OnClientConnectedCallback = null;

    private void Start()
    {
        OnClientConnectedCallback = null;
        OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        GameObject dummyPlayer = Instantiate(_player,
                                             new Vector3(-39.4f, -4.01f, 12.1f),
                                             Quaternion.Euler(0, 90, 0));

        playerName = GenerateRandomName();
        dummyPlayer.name = playerName;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer && NetworkManager.Singleton.ConnectedClients.Count >= 2)
        {
            SpawnDummyPlayerClientRpc();
        }
    }

    [ClientRpc]
    private void SpawnDummyPlayerClientRpc()
    {
        Vector3 playerPos = GameObject.Find(playerName).transform.position;
        playerPos.x += playersCount * 5;
        GameObject player = Instantiate(_otherPlayer, playerPos, Quaternion.Euler(0, 90, 0));
        playersCount++;
    }

    private string GenerateRandomName()
    {
        // Generate a unique name using GUID
        return $"Player_{Guid.NewGuid()}";
    }

    //public override void OnNetworkSpawn()
    //{

    //}
}
