using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LobbyManager : MonoSingleTon<LobbyManager>
{
    [SerializeField]
    private int maxPlayerCount;

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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            LobbiesList();
        }
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayer = maxPlayerCount;

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

            NetworkManager.Singleton.StartHost();

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

            Debug.Log("asd");
            try
            {
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id); // 이렇게 joinedLobby를 업데이트
                _joinedLobby = lobby;
            }
            catch(Exception ex)
            {
                if (LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id) == null)
                {
                    Debug.Log("a");
                    ReSetLobby();
                }
            }
            UpdateLobbyUI();
        }
    }

    public async Task<bool> JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            // 로비 코드 정리
            string cleanedLobbyCode = CleanLobbyCode(lobbyCode);

            // 로비 참여 옵션 설정
            JoinLobbyByCodeOptions joinLobbyByCode = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };

            Debug.Log($"Joined Lobby: {cleanedLobbyCode}");

            // 로비에 참여
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(cleanedLobbyCode, joinLobbyByCode);

            _joinedLobby = lobby;

            NetworkManager.Singleton.StartClient();

            // 플레이어 정보 출력
            PrintPlayer(lobby);
            UpdateLobbyUI();

            return true;
        }
        catch (LobbyServiceException ex)
        {
            if (ex.Reason == LobbyExceptionReason.LobbyNotFound)
            {
                Debug.LogError("The lobby code you entered does not exist.");
                // 사용자에게 로비가 존재하지 않는다는 메시지 표시
            }
            else
            {
                Debug.LogError($"Failed to join lobby: {ex}");
            }

            return false;
        }
    }

    // 로비 코드에서 불필요한 문자를 제거하는 메서드
    private string CleanLobbyCode(string code)
    {
        // 모든 Zero Width Space 문자를 제거합니다
        return code.Replace("\u200B", string.Empty);
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

    public async Task<bool> LeaveLobby()
    {
        try
        {

            if (NetworkManager.Singleton.IsServer) // 서버(호스트)라면 클라이언트 연결 해제
            {
                //int playersCount = _joinedLobby.Players.Count;
                //Debug.Log($"playersCount: {playersCount}");
                //if (playersCount >= 2) // 내가 마지막으로 남은 것이 아니면
                //{
                //    MigrateLobbyHost(_joinedLobby.Players[1].Id); // 호스트 옮김
                //}

                // 여기다가 호스트가 나갔다고 UI로

                UIManager.Instance.HideUI("LobbyUI");

                HideLobbyUIClientRpc();
                NetworkManager.Singleton.Shutdown();
                await LobbyService.Instance.DeleteLobbyAsync(_hostLobby.Id);
            }
            else
            {
                HideLobbyUI();
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            }

            NetworkManager.Singleton.Shutdown();

            ReSetLobby();

            return true;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
            return false;
        }
    }

    //private async void MigrateLobbyHost(string hostId) // Change Host
    //{
    //    try
    //    {
    //        _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id,
    //        new UpdateLobbyOptions
    //        {
    //            HostId = hostId
    //        });

    //        Debug.Log($"hostId: {hostId}");
    //        Debug.Log($"hostId: {_hostLobby.HostId}");
    //        _joinedLobby = _hostLobby;
    //    }
    //    catch (LobbyServiceException ex)
    //    {
    //        Debug.LogError(ex);
    //    }
    //}

    private void UpdateLobbyUI()
    {
        UIManager.Instance.UpdateText("PlayUILobbyCode_Text", _joinedLobby.LobbyCode);
        string PlayerCountText = "Player : " + _joinedLobby.Players.Count.ToString() + '/' + maxPlayerCount;
        UIManager.Instance.UpdateText("PlayerCount_Text", PlayerCountText);
    }

    private void ReSetLobby()
    {
        _joinedLobby = null;
        _hostLobby = _joinedLobby;
    }

    [ClientRpc]
    private void HideLobbyUIClientRpc()
    {
        if (IsOwner) return;

        HideLobbyUI();
    }

    private void HideLobbyUI()
    {
        Debug.Log("HideLobbyUIClientRpc");
        UIManager.Instance.HideUI("LobbyUI");

        UIManager.Instance.ShowUI("Play_Button");
        UIManager.Instance.HideUI("ExitLobby_Button");

        //ReSetLobby();
    }
}
