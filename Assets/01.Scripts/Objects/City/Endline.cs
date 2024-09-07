using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Endline : NetworkBehaviour
{
    private NetworkVariable<int> rankingNum = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        rankingNum.Value = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        rankingNum.Value++;
        SignalHub.OnEnterEndlineEvent?.Invoke(rankingNum.Value);
    }
}
