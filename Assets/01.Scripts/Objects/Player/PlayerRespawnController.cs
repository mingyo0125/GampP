using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnController : MonoBehaviour
{
    private void OnEnable()
    {
        SignalHub.OnPlayerDieEvent += RespawnPlayer;
    }

    private void RespawnPlayer(Transform playerVisual, Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        playerVisual.localScale = new Vector3(100, 100, 100);
    }

    private void OnDisable()
    {
        SignalHub.OnPlayerDieEvent -= RespawnPlayer;
    }
}
