using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMapTrigger : MonoBehaviour
{
    bool isCreated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isCreated)
        {
            SignalHub.OnGenerateMapEvent?.Invoke(transform.position);
            isCreated = true;
        }
    }
}
