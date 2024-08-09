using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExitLobbyButton : UIButton
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        UIManager.Instance.HideUI(name);
    }

    protected override void ButtonEvent()
    {
        UIManager.Instance.ShowUI("Play_Button");
        LobbyManager.Instance.LeaveLobby();
    }
}
