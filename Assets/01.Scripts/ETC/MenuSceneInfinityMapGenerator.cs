using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneInfinityMapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _map;

    private float mapSpawnDistance = 92.2f;

    private void OnEnable()
    {
        SignalHub.OnGenerateMapEvent += GenerateMap;
    }

    private void GenerateMap(Vector3 triggerPos)
    {
        Vector3 spawnPos = new Vector3(triggerPos.x + mapSpawnDistance,
                                       0,
                                       0);

        GameObject map = Instantiate(_map, spawnPos, Quaternion.identity);
        map.transform.position = spawnPos;
    }

    private void OnDisable()
    {
        SignalHub.OnGenerateMapEvent -= GenerateMap;
    }
}
