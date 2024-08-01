using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMapTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SignalHub.OnGenerateMapEvent?.Invoke(transform.position);
    }
}
