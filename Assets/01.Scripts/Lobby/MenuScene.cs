using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    void Update()
    {
        UpdateLobbyUI();
    }

    private void UpdateLobbyUI()
    {
        // 로비씬에서만 하게 바꾸셈
        try
        {
            Lobby lobby = LobbyManager.Instance.JoinedLobby;
            UIManager.Instance.UpdateText("PlayUILobbyCode_Text", lobby.LobbyCode);
            string PlayerCountText = "Player : " + lobby.Players.Count.ToString() + '/' + LobbyManager.Instance.MaxPlayerCount;
        }
        catch { }
    }
}
