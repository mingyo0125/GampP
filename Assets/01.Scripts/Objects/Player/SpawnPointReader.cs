using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointReader : MonoBehaviour
{
    private Vector3 _recentSpawnPoint;
    public Vector3 RecentSpawnPoint => _recentSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            _recentSpawnPoint = other.transform.position;
        }
    }
}
