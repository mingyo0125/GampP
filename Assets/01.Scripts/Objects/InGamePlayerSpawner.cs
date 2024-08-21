using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Netcode.NetworkSceneManager;

public class InGamePlayerSpawner : PlayerSpawner
{
    private OnLoadCompleteDelegateHandler _inGameSceneLoadCompleteCallback;

    protected override void SubscribeCallbacks()
    {
        Debug.Log("OnEnable");

        _inGameSceneLoadCompleteCallback = null;
        _inGameSceneLoadCompleteCallback += OnInGameSceneLoadComplete;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += _inGameSceneLoadCompleteCallback;
    }

    protected override void Awake()
    {
        // ���⼭ �κ� ���־� �ϳ�? �÷��� ���϶� ���� ���� �����ϱ�. �ϴ� ���
    }

    private void OnInGameSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log("OnInGameSceneLoadComplete");
        if (!IsServer) { return; }

        foreach (ulong connectedClientsId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayer(connectedClientsId);
        }
    }

    protected override GameObject SpawnPlayer(ulong clientId)
    {
        GameObject player = base.SpawnPlayer(clientId);
        player.GetComponent<NetworkObject>().Spawn();
        return player;
    }
}
