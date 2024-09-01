using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _carPrefab;

    private void SpawnCar()
    {
        GameObject spawnCar = Instantiate(_carPrefab, transform.position, transform.rotation);
    }

    [ClientRpc]
    private void SpawnCarClientRpc()
    {
        SpawnCar();
    }

    private void Update()
    {
        if (!LobbyManager.Instance.ClientInfo.IsServer) { return; }
        if(Input.GetKeyDown(KeyCode.S))
        {
            SpawnCarClientRpc();
        }
    }
}
