using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Netcode.NetworkSceneManager;

public class InGamePlayerSpawner : PlayerSpawner
{
    private OnLoadCompleteDelegateHandler _inGameSceneLoadCompleteCallback;

    protected override void SubscribeCallbacks()
    {
        _inGameSceneLoadCompleteCallback = null;
        _inGameSceneLoadCompleteCallback += OnInGameSceneLoadComplete;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += _inGameSceneLoadCompleteCallback;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SubscribeCallbacks();
    }

    protected override void Awake()
    {
        // ���⼭ �κ� ���־� �ϳ�? �÷��� ���϶� ���� ���� �����ϱ�. �ϴ� ���
    }

    private void OnInGameSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!IsServer) { return; }

        foreach (ulong connectedClientsId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayer(connectedClientsId);
        }
    }

    protected override GameObject SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(_playerPrefab, _playerSpawnedPoint.position, Quaternion.identity);
        player.GetComponent<NetworkObject>().Spawn();
        return player;
    }
}
