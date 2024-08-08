using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DummyPlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _player;

    private Transform _playerTransform;

    private int playersCount = 1;

    private Action<ulong> OnClientConnectedCallback = null;

    private void Start()
    {
        OnClientConnectedCallback = null;
        OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
    }

    private void Awake()
    {
        _playerTransform = FindAnyObjectByType<DummyPlayer>().transform;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsHost && NetworkManager.Singleton.ConnectedClients.Count >= 2)
        {
            SpawnDummyPlayerServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnDummyPlayerServerRpc()
    {
        Vector3 playerPos = _playerTransform.position;
        playerPos.x += playersCount * 5;
        GameObject player = Instantiate(_player, playerPos, Quaternion.Euler(0, 90, 0));
        player.GetComponent<NetworkObject>().Spawn(true);
        playersCount++;
    }

    //public override void OnNetworkSpawn()
    //{

    //}
}
