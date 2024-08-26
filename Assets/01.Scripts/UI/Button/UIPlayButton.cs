using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayButton : UIButton
{
    SpawnDummyPlayersController _spawnDummyPlayersController;

    protected override void Awake()
    {
        base.Awake();
        _spawnDummyPlayersController = FindAnyObjectByType<SpawnDummyPlayersController>();
    }

    private void Start()
    {
        UIManager.Instance.HideUI(name);
    }

    protected override void ButtonEvent()
    {
        _spawnDummyPlayersController.SetPlayersMove();

        UIManager.Instance.HideAll();
    }
}
