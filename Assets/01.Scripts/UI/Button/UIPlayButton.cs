using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayButton : UIButton
{
    private void Start()
    {
        UIManager.Instance.HideUI(name);
    }

    protected override void ButtonEvent()
    {
        DummyPlayer[] dummyPlayers = FindObjectsByType<DummyPlayer>(FindObjectsSortMode.None);

        foreach (DummyPlayer dummyPlayer in dummyPlayers)
        {
            dummyPlayer.SetMove();
        }

        UIManager.Instance.HideAll();
    }
}
