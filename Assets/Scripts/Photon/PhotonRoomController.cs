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
    
    [SerializeField] private GameMode _selectedGameMode;
    [SerializeField] private GameMode[] _availableGameModes;
    [SerializeField] private bool _startGame;
    [SerializeField] private float _currentCountDown;
    [SerializeField] private int _gameSceneIndex;

    private const string GAME_MODE = "GAMEMODE";
    private const string START_GAME = "STARTGAME";
    private const float GAME_COUNT_DOWN = 10f;

    public static Action<GameMode> OnJoinRoom = delegate { };
    public static Action<bool> OnRoomStatusChange = delegate { };
    public static Action OnRoomLeft = delegate { };
    public static Action<Player> OnOtherPlayerLeftRoom = delegate { };
    public static Action<Player> OnMasterOfRoom = delegate { };
    public static Action<float> OnCountingDown = delegate { };

    #endregion

    #region Default Unity Methods

    private void Awake()
    {
        UIGameMode.OnGameModeSelected += HandleGameModeSelected;
        UIInvite.OnRoomInviteAccept += HandleRoomInviteAccept;
        PhotonConnector.OnLobbyJoined += HandleLobbyJoined;
        UIDisplayRoom.OnLeaveRoom += HandleLeaveRoom;
        UIDisplayRoom.OnStartGame += HandleGameStart;
        UIFriend.OnGetRoomStatus += HandleGetRoomStatus;
        UIPlayerSelection.OnKickPlayer += HandleKickPlayer;
    }

    private void Update()
    {
        if (!_startGame) return;

        if (_currentCountDown > 0)
        {
            OnCountingDown?.Invoke(_currentCountDown);
            _currentCountDown -= Time.deltaTime;
        }
        else
        {
            _startGame = false;

            Debug.Log("Loading level!");
            PhotonNetwork.LoadLevel(_gameSceneIndex);
        }
    }

    private void OnDestroy()
    {
        UIGameMode.OnGameModeSelected -= HandleGameModeSelected;
        UIInvite.OnRoomInviteAccept -= HandleRoomInviteAccept;
        PhotonConnector.OnLobbyJoined -= HandleLobbyJoined;
        UIDisplayRoom.OnLeaveRoom -= HandleLeaveRoom;
        UIDisplayRoom.OnStartGame -= HandleGameStart;
        UIFriend.OnGetRoomStatus -= HandleGetRoomStatus;
        UIPlayerSelection.OnKickPlayer -= HandleKickPlayer;
    }

    #endregion

    #region Private Methods

    private void HandleKickPlayer(Player kickedplayer)
    {
        if(PhotonNetwork.LocalPlayer.Equals(kickedplayer))
        { 
            HandleLeaveRoom(); 
        }
    }

    private void HandleGameStart()
    {
        Hashtable startRoomProperty = new Hashtable()
            { {START_GAME, true} };
        PhotonNetwork.CurrentRoom.SetCustomProperties(startRoomProperty);
    }

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
        Debug.Log($"Lobby joined. RoomName: {roomName}");
        if(roomName.Count() > 0)
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

    private void HandleRoomInviteAccept(string roomName)
    {
        PlayerPrefs.SetString("PHOTONROOM", roomName);
        if(PhotonNetwork.InRoom)
        {
            OnRoomLeft?.Invoke();
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if(PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinRoom(roomName);
                PlayerPrefs.SetString("PHOTONROOM", null);
            }
        }
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

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"New Master Client is {newMasterClient.NickName}");
        OnMasterOfRoom?.Invoke(newMasterClient);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object startGameObject;
        if (propertiesThatChanged.TryGetValue(START_GAME, out startGameObject))
        {
            _startGame = (bool)startGameObject;
            if (_startGame)
            {
                _currentCountDown = GAME_COUNT_DOWN;
            }
            if (_startGame && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    #endregion

}
