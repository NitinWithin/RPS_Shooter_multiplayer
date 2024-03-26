
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte _maxPlayers = 4;

    [SerializeField] private GameObject _controlPanel;
    [SerializeField] private GameObject _progressLabel;
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _progressLabel.SetActive(false);
        _controlPanel.SetActive(true);

        
    }


    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN, joining Random room.");
        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _progressLabel.SetActive(false);
        _controlPanel.SetActive(true);
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public void Connect()
    {
        _progressLabel.SetActive(true);
        _controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Isconnected To photon");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Is NOT connected To photon, connecting now...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Join room. Creating Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayers });
        Debug.Log("Room Created");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined:" + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("PlayGround");
        }
    }


}
