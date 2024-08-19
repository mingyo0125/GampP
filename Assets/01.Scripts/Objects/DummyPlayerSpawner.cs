using System;
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
    private Action<ulong> OnClientDisconnectCallback = null;

    private void Start()
    {
        OnClientConnectedCallback = null;
        OnClientDisconnectCallback = null;

        OnClientConnectedCallback += OnClientConnected;
        OnClientDisconnectCallback += OnClientDisconnected;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;



        SpawnDummyPlayer(NetworkManager.Singleton.LocalClientId);
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
        playersCount--; // ¾ÈµÊ
    }

    [ClientRpc]
    private void SpawnDummyPlayerClientRpc(ulong clientId)
    {
        SpawnDummyPlayer(clientId);
    }

    private void SpawnDummyPlayer(ulong clientId)
    {
        Vector3 playerPos = _playerSpawnedPoint.position;
        playerPos.x += 3 * playersCount;
        GameObject player = Instantiate(_playerPrefab, playerPos, Quaternion.identity);
        player.name = clientId.ToString();
        playersCount++;
    }
}