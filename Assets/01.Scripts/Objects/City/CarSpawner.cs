using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] _carPrefabs;

    private NetworkVariable<float> randomSpawnTime = new NetworkVariable<float>();
    private NetworkVariable<int> randomSpawnIdx = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (LobbyManager.Instance.ClientInfo.IsServer)
        {
            StartSpawnCar();
        }

        randomSpawnTime.OnValueChanged += SpawnCar;
    }

    private void StartSpawnCar()
    {
        StartCoroutine(SetSpawnTimeValue());
    }

    private IEnumerator SetSpawnTimeValue()
    {
        while (true)
        {
            randomSpawnIdx.Value = Random.Range(0, _carPrefabs.Length);
            yield return new WaitForSeconds(0.1f);
            randomSpawnTime.Value = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomSpawnTime.Value - 0.1f);
        }
    }

    private void SpawnCar(float oldValue, float newValue)
    {
        Instantiate(_carPrefabs[randomSpawnIdx.Value], transform.position, transform.rotation);
    }

    private void OnDisable()
    {
        randomSpawnTime.OnValueChanged -= SpawnCar;
    }
}
