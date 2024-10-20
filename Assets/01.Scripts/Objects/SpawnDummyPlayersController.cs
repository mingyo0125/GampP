using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnDummyPlayersController : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    [ClientRpc]
    private void SetPlayersMoveClientRpc()
    {
        DummyPlayer[] dummyPlayers = FindObjectsByType<DummyPlayer>(FindObjectsSortMode.None);

        foreach (DummyPlayer dummyPlayer in dummyPlayers)
        {
            dummyPlayer.SetMove();
        }
    }

    public void SetPlayersMove()
    {
        if (!IsServer) { return; }

        SetPlayersMoveClientRpc();
    }
}
