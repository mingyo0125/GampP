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
        DummyPlayer[] dummyPlayers = FindObjectsOfType<DummyPlayer>();

        foreach (DummyPlayer dummyPlayer in dummyPlayers)
        {
            Debug.Log(dummyPlayer);
            Destroy(dummyPlayer);
        }

        UIManager.Instance.HideAll();
    }
}
