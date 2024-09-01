using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _carPrefab;

    private NetworkVariable<float> randomSpawnTime = new NetworkVariable<float>();

    private void StartSpawnCar()
    {
        StartCoroutine(SetSpawnTimeValue());
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        randomSpawnTime.OnValueChanged += SpawnCar;
        if (!LobbyManager.Instance.ClientInfo.IsServer) { return; }

        SignalHub.OnGameStartEvent += StartSpawnCar;
    }

    private IEnumerator SetSpawnTimeValue()
    {
        while (true)
        {
            randomSpawnTime.Value = Random.Range(2, 8f);

            yield return new WaitForSeconds(randomSpawnTime.Value);
        }
    }

    private void SpawnCar(float oldValue, float newValue)
    {
        GameObject spawnCar = Instantiate(_carPrefab, transform.position, transform.rotation);
    }

    private void OnDisable()
    {
        randomSpawnTime.OnValueChanged -= SpawnCar;
        SignalHub.OnGameStartEvent -= StartSpawnCar;
    }
}
