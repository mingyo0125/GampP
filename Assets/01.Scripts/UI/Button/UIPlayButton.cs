using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayButton : UIButton
{
    TTTTest _tTTest;

    protected override void Awake()
    {
        base.Awake();
        _tTTest = FindAnyObjectByType<TTTTest>();
    }

    private void Start()
    {
        UIManager.Instance.HideUI(name);
    }

    protected override void ButtonEvent()
    {
        _tTTest.SetPlayersMove();

        UIManager.Instance.HideAll();
    }
}
