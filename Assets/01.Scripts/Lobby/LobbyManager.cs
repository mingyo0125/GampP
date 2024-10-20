using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class LobbyManager : MonoSingleTon<LobbyManager>
{
    [SerializeField]
    private int maxPlayerCount;
    public int MaxPlayerCount => maxPlayerCount;

    private Lobby _hostLobby = null;
    private Lobby _joinedLobby = null;
    public Lobby JoinedLobby => _joinedLobby;

    private float heartbeatTimer;
    private float lobbyUpdateTimer;

    private string playerName;

    private const string playerNameKey = "PlayerName";

    private ClientInformation _clientInfo;
    public ClientInformation ClientInfo => _clientInfo;

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
        //if(Input.GetKeyDown(KeyCode.K))
        //{
        //    LobbiesList();
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    PrintPlayer(_joinedLobby);
        //}
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
            _clientInfo = new ClientInformation(isServer: true);

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

            try
            {
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id); // 이렇게 joinedLobby를 업데이트
                _joinedLobby = lobby;
            }
            catch(Exception ex)
            {
                if (_joinedLobby == null||
                    LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id) == null)
                {
                    ReSetLobby();
                }
            }
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

            _clientInfo = new ClientInformation(isServer: false);

            // 플레이어 정보 출력
            PrintPlayer(lobby);

            return true;
        }
        catch (LobbyServiceException ex)
        {
            bool isloobyFound = ex.Reason == LobbyExceptionReason.LobbyNotFound ||
                                ex.Reason == LobbyExceptionReason.InvalidJoinCode;

            if (isloobyFound)
            {
                UIManager.Instance.ShowWarningText("로비를 찾을 수 없습니다.");
            }
            else
            {
                UIManager.Instance.ShowWarningText("로비 참가에 실패하였습니다.");
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
        if (!IsServer) { return; }
        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log($"{client.ClientId}");
        }
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

    private void ReSetLobby()
    {
        _joinedLobby = null;
        _hostLobby = _joinedLobby;

        //_clientInfo = null;
    }

    [ClientRpc]
    private void HideLobbyUIClientRpc()
    {
        if (IsOwner) return;
        
        UIManager.Instance.ShowWarningText("로비가 더 이상 존재하지 않습니다.");

        HideLobbyUI();
    }

    private void HideLobbyUI()
    {
        UIManager.Instance.HideUI("LobbyUI");
        UIManager.Instance.ShowUI("Lobby_Button");
        UIManager.Instance.HideUI("ExitLobby_Button");

        //ReSetLobby();
    }
}
