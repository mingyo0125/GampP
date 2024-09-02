using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameStart();
public delegate void CountStart();


public static class SignalHub
{
    public static GameStart OnGameStartEvent;
    public static CountStart OnCountStartEvent;

}
