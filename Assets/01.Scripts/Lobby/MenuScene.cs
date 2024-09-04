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
        // �κ�������� �ϰ� �ٲټ�
        try
        {
            Lobby lobby = LobbyManager.Instance.JoinedLobby;
            UIManager.Instance.UpdateText("PlayUILobbyCode_Text", lobby.LobbyCode);
            string PlayerCountText = "Player : " + lobby.Players.Count.ToString() + '/' + LobbyManager.Instance.MaxPlayerCount;
        }
        catch { }
    }
}
