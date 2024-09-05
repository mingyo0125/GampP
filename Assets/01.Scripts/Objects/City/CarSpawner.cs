using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] _carPrefabs;

    private NetworkVariable<float> randomSpawnTime = new NetworkVariable<float>();

    private void StartSpawnCar()
    {
        StartCoroutine(SetSpawnTimeValue());
    }

    private void Awake()
    {
        randomSpawnTime.OnValueChanged += SpawnCar;
        if (!LobbyManager.Instance.ClientInfo.IsServer) { return; }
        SignalHub.OnGameStartEvent += StartSpawnCar;
    }

    private IEnumerator SetSpawnTimeValue()
    {
        Debug.Log("SetSpawnTimeValue");
        while (true)
        {
            randomSpawnTime.Value = Random.Range(2, 5f);

            yield return new WaitForSeconds(randomSpawnTime.Value);
        }
    }

    private void SpawnCar(float oldValue, float newValue)
    {
        int randomcarIdx = Random.Range(0, _carPrefabs.Length - 1);
        GameObject spawnCar = Instantiate(_carPrefabs[randomcarIdx], transform.position, transform.rotation);
    }

    private void OnDisable()
    {
        randomSpawnTime.OnValueChanged -= SpawnCar;
        SignalHub.OnGameStartEvent -= StartSpawnCar;
    }
}
