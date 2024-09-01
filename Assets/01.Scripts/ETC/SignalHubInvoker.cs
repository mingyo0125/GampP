using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalHubInvoker : MonoBehaviour
{
    public void GameStart()
    {
        SignalHub.OnGameStartEvent?.Invoke();
    }
}
