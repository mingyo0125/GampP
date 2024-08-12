using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DummyPlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private Transform _playerSpawnedPoint;

    private int playersCount = 0;

    private Action<ulong> OnClientConnectedCallback = null;

    private void Start()
    {
        OnClientConnectedCallback = null;
        OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        SpawnDummyPlayer();
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
        SpawnDummyPlayer();
    }

    private void SpawnDummyPlayer()
    {
        Vector3 playerPos = _playerSpawnedPoint.position;
        playerPos.x += 3 * playersCount;
        GameObject player = Instantiate(_playerPrefab, playerPos, Quaternion.identity);
        playersCount++;
    }
}
