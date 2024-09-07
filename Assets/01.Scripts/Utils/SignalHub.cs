using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameStart();
public delegate void CountStart();
public delegate void PlayerDie(Transform playerVisual, Vector3 spawnPoint);


public static class SignalHub
{
    public static GameStart OnGameStartEvent;
    public static CountStart OnCountStartEvent;
    public static PlayerDie OnPlayerDieEvent;

}
