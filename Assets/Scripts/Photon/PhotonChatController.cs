using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using System;
using UnityEngine;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    #region Variables
    [SerializeField] private string _nickName;
    private ChatClient _chatClient;
    #endregion

    #region Default Unity Methods
    private void Awake()
    {
        _nickName = PlayerPrefs.GetString("USERNAME");
    }
    void Start()
    {
        _chatClient = new ChatClient(this);
        ConnectToPhotonChat();
    }

    // Update is called once per frame
    void Update()
    {
        _chatClient.Service();
    }
    #endregion

    #region Private Methods

    private void ConnectToPhotonChat()
    {
        Debug.Log("Connecting to photon chat");
        _chatClient.AuthValues = new Photon.Chat.AuthenticationValues(_nickName);
        Debug.Log("AuthValues: " + _chatClient.AuthValues);
        ChatAppSettings chatsettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        _chatClient.ConnectUsingSettings(chatsettings);
    }

    #endregion

    #region Public methods
    public void SendDirectMessage(string recipient, string message)
    {
        Debug.Log("sending message: " + recipient + " with message: " + message);
        _chatClient.SendPrivateMessage(recipient, message);
    }
    #endregion

    #region PUN callbacks
    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnDisconnected()
    {
        Debug.LogError("Connection FAILED to Photon Chat");
    }

    public void OnConnected()
    {
        Debug.Log("Connected to Photon Chat");
        SendDirectMessage("NitinWithin", "YO!!");
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage");
        if(!string.IsNullOrEmpty(message.ToString())) 
        {
            //Channel Name format [sender : recipient]
            string[] splitNames = channelName.Split(':');
            string senderName = splitNames[0];
            if(!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Sender : " + sender + " Message: " + message.ToString());
            }
        
        }
        else
        {
            Debug.LogError("OnprivateMessage: Message is empty");
        }
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
    #endregion

    #region Playfab callbacks

    #endregion
}
