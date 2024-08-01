using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GenerateMap(Vector3 triggerPos);


public static class SignalHub
{
    public static GenerateMap OnGenerateMapEvent;

}
