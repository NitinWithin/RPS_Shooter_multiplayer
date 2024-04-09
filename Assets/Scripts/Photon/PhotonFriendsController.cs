using Photon.Pun;
using Photon.Realtime;
using PlayfabFriendInfo = PlayFab.ClientModels.FriendInfo;
using PhotonFriendInfo = Photon.Realtime.FriendInfo;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class PhotonFriendsController : MonoBehaviourPunCallbacks
{
    #region variables
    public static Action<List<PhotonFriendInfo>> OnDisplayFriends = delegate { };
    #endregion

    #region Default Unity methods
    private void Awake()
    {
        PlayFabFriendController.OnFriendListUpdated += HandleFriendsUpdated;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        PlayFabFriendController.OnFriendListUpdated -= HandleFriendsUpdated;
    }


    #endregion

    #region public methods

    #endregion

    #region private methods
    private void HandleFriendsUpdated(List<PlayfabFriendInfo> friends)
    {
        if(friends.Count != 0)
        {
            string[] _friendsDisplayName = friends.Select(f => f.TitleDisplayName).ToArray();
            PhotonNetwork.FindFriends(_friendsDisplayName);
        }
        else
        {
            List<PhotonFriendInfo> _friendList = new List<PhotonFriendInfo>();
            OnDisplayFriends?.Invoke(_friendList);
        }
    }
    #endregion

    #region Pun callbacks
    public override void OnFriendListUpdate(List<PhotonFriendInfo> friendList)
    {
        OnDisplayFriends?.Invoke(friendList);
    }
    #endregion

    #region Playfab callbacks
    #endregion
}
