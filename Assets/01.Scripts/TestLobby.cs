using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestLobby : MonoBehaviour
{
    private Lobby _hostLobby = null;
    private Lobby _joinedLobby = null;

    private float heartbeatTimer;
    private float lobbyUpdateTimer;

    private string playerName;

    private const string playerNameKey = "PlayerName";

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed In {AuthenticationService.Instance.PlayerId}");
        };

        StartCoroutine(LobbyUpdate());
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "player" + Random.Range(10, 99);
        Debug.Log(playerName);
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
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayer = 4;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,

                Player = GetPlayer(),

                Data = new Dictionary<string, DataObject>
                {
                    {
                        "LobbyName",

                        new DataObject(DataObject.VisibilityOptions.Public,
                                       "CaptureTheFlag") /*,
                                       DataObject.IndexOptions.S1 ) */ // 
                    }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);

            _hostLobby = lobby;
            _joinedLobby = _hostLobby;

            Debug.Log($"Create {lobbyName} Lobby. maxyPlayer: {maxPlayer}, LobbyID: {lobby.Id}, LobbyCode: {lobby.LobbyCode}");
            PrintPlayer(_hostLobby);
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
            QueryLobbiesOptions queryOptions = GetQueryLobbiesOptions();

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);

            Debug.Log($"Find Lobbies: {queryResponse.Results.Count}");
            //foreach (Lobby lobby in queryResponse.Results)
            //{
            //    Debug.Log($"{lobby.Name}: {lobby.MaxPlayers}");
            //}
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
        }
    }

    private IEnumerator LobbyUpdate()
    {
        while(true)
        {
            if (_hostLobby != null)
            {
                UpdateLobbyHeartbeat();
            }

            if(_joinedLobby != null)
            {
                UpdateJoinLobby();
            }

            yield return null;
        }
    }

    private async void UpdateLobbyHeartbeat()
    {
        heartbeatTimer -= Time.deltaTime;

        if (heartbeatTimer < 0f)
        {
            float heartbeatTimerMax = 15;
            heartbeatTimer = heartbeatTimerMax;

            await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
        }
    }

    private async void UpdateJoinLobby()
    {
        lobbyUpdateTimer -= Time.deltaTime;

        if (lobbyUpdateTimer < 0f)
        {
            float lobbyUpdateTimerMax = 1.1f;
            lobbyUpdateTimer = lobbyUpdateTimerMax;

            Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id); // 이렇게 joinedLobby를 업데이트
            _joinedLobby = lobby;
        }
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCode = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };

            Debug.Log($"Joined Lobby: {lobbyCode}");

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCode);
            _joinedLobby = lobby;

            PrintPlayer(lobby);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
        }
    }

    private void PrintPlayer(Lobby lobby)
    {
        Debug.Log($"Players in Lobby: {lobby.Name}");

        foreach (Player player in lobby.Players)
        {
            Debug.Log($"{player.Id}: {player.Data[playerNameKey].Value}");
        }
    }

    private QueryLobbiesOptions GetQueryLobbiesOptions()
    {
        return new QueryLobbiesOptions
        {
            Count = 25,

            Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,
                                "0",
                                QueryFilter.OpOptions.GT),

                //new QueryFilter(QueryFilter.FieldOptions.S1,
                //                "CaptureTheFlag",
                //                QueryFilter.OpOptions.EQ)
            },

            Order = new List<QueryOrder>
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            }
        };
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>()
                    {
                        {
                            "PlayerName",
                            new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,
                                                 playerName)
                        }
                    }
        };
    }

    private async void UpdateLobbyData(string key, DataObject.VisibilityOptions visibilityOption, string updateStringValue)
    {
        try
        {
            _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id,
            new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {
                        key,
                        new DataObject(visibilityOption, updateStringValue)
                    }
                }
            });

            _joinedLobby = _hostLobby;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
        }
        
    }
}
