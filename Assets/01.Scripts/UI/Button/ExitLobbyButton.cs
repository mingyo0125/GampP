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

    protected override async void ButtonEvent()
    {
        bool leaveSucces = await LobbyManager.Instance.LeaveLobby();
        if(!leaveSucces) { return; }

        UIManager.Instance.ShowUI("Lobby_Button");
        UIManager.Instance.HideUI("ExitLobby_Button");
        UIManager.Instance.HideUI("LobbyUI");
        UIManager.Instance.HideUI("Play_Button");
    }
}
