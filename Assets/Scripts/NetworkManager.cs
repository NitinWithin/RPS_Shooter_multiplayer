
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private byte _maxPlayers = 6;

    TMP_Text displayText;

    private string _playerName;
    List<RoomInfo> _createdRoom = new List<RoomInfo>();
    private string _roomName = "\'s Room";
    private Vector2 _roomListScroll = Vector2.zero;
    private bool _joiningRoom = false;

    bool _render = false;

    PopulateRoomList _populateRoomList;
    #endregion
    #region Default Methods
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _populateRoomList = GameObject.Find("Canvas").GetComponentInChildren<PopulateRoomList>();

    }

    // Start is called before the first frame update
    void Start()
    {
        _playerName = PlayerPrefs.GetString("PlayerName");

        PhotonNetwork.EnableCloseConnection = true;

        Debug.Log("The population class is : " + _populateRoomList);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }


    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region PUN Callbacks
   
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Join room. Creating Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayers });
        Debug.Log("Room Created");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined:" + PhotonNetwork.CurrentRoom.Name + " player: " + PhotonNetwork.LocalPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("PlayGround");
        }
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        Debug.LogError("Falied to connect: " + _cause.ToString());
        _createdRoom.Clear();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {

        Debug.Log("RoomList update called. has " + _roomList.Count);
        _createdRoom = _roomList;
        if (_createdRoom.Count > 0)
        {
            _populateRoomList.RoomList(_roomList);
        }
        else
        {
            //CreateRoom();
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully: " + PhotonNetwork.CurrentRoom.Name);
        RoomInfo createdRoomInfo = PhotonNetwork.CurrentRoom;
        _createdRoom.Add(createdRoomInfo);
        Debug.Log("RoomList update called. has " + _createdRoom.Count);
        _populateRoomList.RoomList(_createdRoom);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Room creation failed: " + message);
    }
    #endregion

    #region private methods

    #endregion

    #region public methods



    public void Connect()
    {

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void CreateRoom()
    {
        // Check if not already in a room
        if (!PhotonNetwork.InRoom)
        {
            // Check if a room with the specified name already exists
            RoomInfo existingRoom = _createdRoom.Find(room => room.Name == _playerName + _roomName);

            if (existingRoom == null)
            {
                // Create a room with the specified name
                PhotonNetwork.CreateRoom(_playerName + _roomName);
            }
            else
            {
                Debug.LogWarning("A room with the name '" + _playerName + _roomName + "' already exists.");
                PhotonNetwork.CreateRoom(_playerName + _roomName + UnityEngine.Random.Range(1, 100));
            }
        }
        else
        {
            Debug.LogWarning("Cannot create room while already in a room.");
            PhotonNetwork.CreateRoom(_playerName + _roomName + UnityEngine.Random.Range(1, 100));
        }
    }

    public void JoinRoomFromLobby()
    {
        Debug.Log("Join room Button Clicked");
        // Get the display text from the selected panel


        string roomName = GetSelectedPanelDisplayText();
        // Check if the room name is not empty

        if(roomName == null)
        {
            Debug.LogError("Room name is empty.");
            return;
        }

        Debug.Log("Button Clicked. joinging room: " + roomName);
        // Attempt to join the room with the specified name
        if (!PhotonNetwork.JoinRoom(roomName))
        {
            Debug.Log("Room not Joined");
        }
        else
        {
            Debug.Log("Room joined");
        }
    }


    private string GetSelectedPanelDisplayText()
    {
        // Check if there is a selected GameObject
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        if(selectedObject == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return null;
        }
        TextMeshProUGUI displayText = selectedObject.GetComponentInChildren<TextMeshProUGUI>();

        if (displayText == null)
        {
            Debug.LogWarning("Text component not found on selected panel.");
            return null;
        }

        return displayText.text;
    }
    #endregion

}
