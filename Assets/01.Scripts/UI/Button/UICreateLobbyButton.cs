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
    }
}
