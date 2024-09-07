using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Netcode.NetworkSceneManager;

public class InGamePlayerSpawner : PlayerSpawner
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
    }

    protected override void Start()
    {
        base.Start();
        if (!LobbyManager.Instance.ClientInfo.IsServer) { return; }
        Debug.Log(NetworkManager.LocalClientId);

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayer(clientId);
        }
    }

    protected override GameObject SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(_playerPrefab, _playerSpawnedPoint.position, Quaternion.identity);
        player.transform.position -= new Vector3(2 * clientId, 0, 0);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        player.transform.Find("Player").GetComponent<PlayerNetWork>().SetClientidClientRpc(clientId);
        return player;
    }
}
