using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEditor.PackageManager;
using UnityEngine;

public abstract class PlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    protected Transform _playerSpawnedPoint;
    protected int playersCount = 0;

    [SerializeField]
    protected GameObject _playerPrefab;

    protected virtual void Start() { }
    protected virtual void Awake() { }

    protected virtual void OnEnable()
    {
        SubscribeCallbacks();
    }

    protected virtual GameObject SpawnPlayer(ulong clientId)
    {
        Vector3 playerPos = _playerSpawnedPoint.position;
        playerPos.x += 3 * playersCount;
        GameObject player = Instantiate(_playerPrefab, playerPos, Quaternion.identity);
        player.name = clientId.ToString();
        playersCount++;
        return player;
    }

    protected virtual void SubscribeCallbacks() { }

}