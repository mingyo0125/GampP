using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private Lobby _hostLobby = null;

    private float heartbeatTimer;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed In {AuthenticationService.Instance.PlayerId}");
        };

        StartCoroutine(HandleLobbyHeartbeatCorou());
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.J))
        {
            CreateLobby();
        }

        if(Input.GetKeyUp(KeyCode.K))
        {
            LobbiesList();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            JoinLobby();
        }
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayer = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer);

            _hostLobby = lobby;

            Debug.Log($"Create {lobbyName} Lobby. maxyPlayer: {maxPlayer}");
        }
        catch(LobbyServiceException ex)
        {
            Debug.LogError($"Lobby Error: {ex}");
        }
        
    }

    private async void LobbiesList()
    {
        try
        {
            QueryLobbiesOptions queryOptions = CreateQueryLobbiesOptions();

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);

            Debug.Log($"Find Lobbies: {queryResponse.Results.Count}");
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log($"{lobby.Name}: {lobby.MaxPlayers}");
            }
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
        }
    }

    private QueryLobbiesOptions CreateQueryLobbiesOptions()
    {
        return new QueryLobbiesOptions
        {
            Count = 25,

            Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,
                                "0",
                                QueryFilter.OpOptions.GT)
            },

            Order = new List<QueryOrder>
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            }
        };
    }

    private IEnumerator HandleLobbyHeartbeatCorou()
    {
        while(true)
        {
            if (_hostLobby != null)
            {
                HandleLobbyHeartbeat();
            }

            yield return null;
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        heartbeatTimer -= Time.deltaTime;

        if (heartbeatTimer < 0f)
        {
            float heartbeatTimerMax = 15;
            heartbeatTimer = heartbeatTimerMax;

            await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
        }
    }

    private async void JoinLobby()
    {
        QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

        Debug.Log($"Joined Lobby: {queryResponse.Results[0].Id}");

        await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
    }
}
