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

    protected override void Awake()
    {
        // ���⼭ �κ� ���־� �ϳ�? �÷��� ���϶� ���� ���� �����ϱ�. �ϴ� ���
    }

    private void OnInGameSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!LobbyManager.Instance.ClientInfo.IsServer) { return; }

        SpawnPlayer(clientId);
    }

    protected override GameObject SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(_playerPrefab, _playerSpawnedPoint.position, Quaternion.identity);
        player.transform.position -= new Vector3(2 * clientId, 0, 0);
        player.GetComponent<NetworkObject>().Spawn();
        player.transform.Find("Player").GetComponent<PlayerNetWork>().SetClientidClientRpc(clientId);
        return player;
    }
}
