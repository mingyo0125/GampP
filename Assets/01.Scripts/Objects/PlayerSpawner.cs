using Unity.Netcode;
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

    protected virtual GameObject SpawnPlayer(ulong clientId)
    {
        Vector3 playerPos = _playerSpawnedPoint.position;
        playerPos.x += 3 * playersCount;
        GameObject player = Instantiate(_playerPrefab, playerPos, Quaternion.identity);
        player.name = clientId.ToString();
        playersCount++;
        return player;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SubscribeCallbacks();
    }

    protected virtual void SubscribeCallbacks() { }

}