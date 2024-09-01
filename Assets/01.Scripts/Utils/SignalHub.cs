using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameStart();


public static class SignalHub
{
    public static GameStart OnGameStartEvent;

}
