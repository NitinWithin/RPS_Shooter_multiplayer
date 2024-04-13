using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonRoomController : MonoBehaviourPunCallbacks
{
    #region Variables
    
    private GameMode _selectedGameMode;
    private GameMode[] _availableGameModes;
    private const string GAME_MODE = "GAMEMODE";

    public static Action<GameMode> OnJoinRoom = delegate { };
    public static Action<bool> OnRoomStatusChange = delegate { };
    public static Action OnRoomLeft = delegate { };
    public static Action<Player> OnOtherPlayerLeftRoom = delegate { };

    #endregion

    #region Default Unity Methods

    private void Awake()
    {
        UIGameMode.OnGameModeSelected += HandleGameModeSelected;
        UIInvite.OnRoomInviteAccept += HandleRoomInviteAccept;
        PhotonConnector.OnLobbyJoined += HandleLobbyJoined;
        UIDisplayRoom.OnLeaveRoom += HandleLeaveRoom;
        UIFriend.OnGetRoomStatus += HandleGetRoomStatus;
    }

    private void OnDestroy()
    {
        UIGameMode.OnGameModeSelected -= HandleGameModeSelected;
        UIInvite.OnRoomInviteAccept -= HandleRoomInviteAccept;
        PhotonConnector.OnLobbyJoined -= HandleLobbyJoined;
        UIDisplayRoom.OnLeaveRoom -= HandleLeaveRoom;
        UIFriend.OnGetRoomStatus -= HandleGetRoomStatus;
    }


    #endregion

    #region Private Methods

    private void HandleGameModeSelected(GameMode gameMode)
    {
        if(!PhotonNetwork.IsConnectedAndReady) 
        { 
            return; 
        }
        if(PhotonNetwork.InRoom)
        {
            return;
        }

        _selectedGameMode = gameMode;
        Debug.Log("Joining gameMode: " + _selectedGameMode);
        JoinPhotonRoom();
    }

    private void JoinPhotonRoom()
    {
        Hashtable expectedCustomRoomProperties = new Hashtable()
        { { GAME_MODE, _selectedGameMode.Name } };

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    private void HandleLobbyJoined()
    {
        string roomName = PlayerPrefs.GetString("PHOTONROOM");
        if(roomName.Count() <= 0)
        {
            PhotonNetwork.JoinRoom(roomName);
            PlayerPrefs.SetString("PHOTONROOM", null);
        }
    }

    private void HandleLeaveRoom()
    {
        if(PhotonNetwork.InRoom)
        {
            OnRoomLeft?.Invoke();
            PhotonNetwork.LeaveRoom();
        }
    }

    private void HandleGetRoomStatus()
    {
        OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
    }

    private void HandleRoomInviteAccept(string obj)
    {
        
    }

    private void DebugPlayerList()
    {
        string players = "";
        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            players += player.Value.NickName + " ";
        }
        Debug.Log("photon Room: Current Players in room: " + players);
    }

    private GameMode GetRoomGameMode()
    {
        string gameModeName = (string)PhotonNetwork.CurrentRoom.CustomProperties[GAME_MODE];
        GameMode gameMode = null;
        for (int i = 0; i < _availableGameModes.Length; i++)
        {
            if (string.Compare(_availableGameModes[i].Name, gameModeName) == 0)
            {
                gameMode = _availableGameModes[i];
                break;
            }
        }
        return gameMode;
    }

    private void CreatePhotonRoom()
    {
        string roomName = Guid.NewGuid().ToString();
        RoomOptions roomOptions = GetRoomOptions();
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    private RoomOptions GetRoomOptions()
    {
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = _selectedGameMode.MaxPlayers;

        string[] roomProperties = { GAME_MODE };

        Hashtable customRoomProperites = new Hashtable()
        { {GAME_MODE, _selectedGameMode.Name } };

        options.CustomRoomPropertiesForLobby = roomProperties;
        options.CustomRoomProperties = customRoomProperites;

        return options;

    }

    #endregion

    #region Public methods

    #endregion

    #region PUN callbacks
    public override void OnCreatedRoom()
    {
        Debug.Log("Photon: Room Created. name: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Photon: Room joined. name: " + PhotonNetwork.CurrentRoom.Name);
        DebugPlayerList();

        _selectedGameMode = GetRoomGameMode();
        OnJoinRoom?.Invoke(_selectedGameMode);
        OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Yoiu have left the room");
        _selectedGameMode = null;
        OnRoomStatusChange?.Invoke(PhotonNetwork.InRoom);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("join Random Failed: " + message);
        CreatePhotonRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed: " +  message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player in Room: " + newPlayer.NickName);
        DebugPlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left room: " +  otherPlayer.NickName);
        OnOtherPlayerLeftRoom?.Invoke(otherPlayer);
    }
    #endregion

    #region Playfab callbacks

    #endregion
}
