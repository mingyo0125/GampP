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
        // 여기서 로비를 없애야 하나? 플레이 중일때 들어올 수도 있으니까. 일단 대기
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
