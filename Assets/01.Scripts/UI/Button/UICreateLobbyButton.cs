using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UICreateLobbyButton : UIButton
{
    protected override void ButtonEvent()
    {
        LobbyManager.Instance.CreateLobby();
        UIManager.Instance.HideUI("PlayUI");
        UIManager.Instance.HideUI("Play_Button");
        UIManager.Instance.ShowUI("ExitLobby_Button");
        UIManager.Instance.ShowUI("LobbyUI");
    }
}
