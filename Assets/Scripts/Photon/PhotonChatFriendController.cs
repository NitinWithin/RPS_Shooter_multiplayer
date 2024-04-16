using Photon.Chat;
using PlayFab.ClientModels;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PhotonChatFriendController : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool _initialized;
    private List<string> _friendList;
    private ChatClient _chatClient;

    public static Dictionary<string, PhotonStatus> _friendStatus;

    public static Action<List<string>> OnDisplayFriends = delegate { };
    public static Action<PhotonStatus> OnStatusUpdated = delegate { };


    #endregion

    #region Default Unity Methods
    private void Awake()
    {
        _friendList = new List<string>();
        _friendStatus = new Dictionary<string, PhotonStatus>();

        PlayFabFriendController.OnFriendListUpdated += HandleFriendListUpdate;
        PhotonChatController.OnChatConnected += HandleChatConnected;
        PhotonChatController.OnStatusUpdated += HandleStatusUpdated;
        UIFriend.OnGetCurrentStatus += HandleGetCurrentStatus;
    }

    private void OnDestroy()
    {
        PlayFabFriendController.OnFriendListUpdated -= HandleFriendListUpdate;
        PhotonChatController.OnChatConnected -= HandleChatConnected;
        PhotonChatController.OnStatusUpdated -= HandleStatusUpdated;
        UIFriend.OnGetCurrentStatus -= HandleGetCurrentStatus;
    }

    #endregion

    #region Private Methods

    private void HandleFriendListUpdate(List<FriendInfo> friends)
    {
        _friendList = friends.Select(f => f.TitleDisplayName).ToList();
        Debug.Log("HandleFriendListupdate: " + _friendList.ToString());
        RemovePhotonFriends();
        FindPhotonFriends();
    }

    private void HandleChatConnected(ChatClient client)
    {
        _chatClient = client;
        RemovePhotonFriends();
        FindPhotonFriends();
    }

    private void HandleStatusUpdated(PhotonStatus status)
    {
        if (_friendStatus.ContainsKey(status.PlayerName))
        {
            _friendStatus[status.PlayerName] = status;
        }
        else
        {
            _friendStatus.Add(status.PlayerName, status);
        }
    }

    private void HandleGetCurrentStatus(string friendName) 
    {
        PhotonStatus status;
        if(_friendStatus.ContainsKey(friendName))
        {
            status = _friendStatus[friendName];
        }
        else
        {
            status = new PhotonStatus(friendName, 0, "");
        }
        OnStatusUpdated?.Invoke(status);
    }

    private void FindPhotonFriends()
    {
        if (_chatClient == null)
        {
            Debug.Log("ChatClient is null");
            return;
        }

        if(_friendList.Count() != 0)
        {
            Debug.Log("_friendList count " + _friendList.Count());
            _initialized = true;
            string[] friendsNameDisplay = _friendList.ToArray();
            _chatClient.AddFriends(friendsNameDisplay);
            OnDisplayFriends?.Invoke(_friendList);
        }
    }

    private void RemovePhotonFriends()
    {
        if( _friendList.Count > 0 && _initialized)
        {
            string[] friendNamedispaly = _friendList.ToArray();
            _chatClient.RemoveFriends(friendNamedispaly);
            OnDisplayFriends?.Invoke(_friendList);
        }
    }

    #endregion
}
