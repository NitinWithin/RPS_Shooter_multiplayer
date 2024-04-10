using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Linq;

public class PlayFabFriendController : MonoBehaviour
{
    #region Variables
    public static Action<List<FriendInfo>> OnFriendListUpdated = delegate { };
    private List<FriendInfo> _friends;
    #endregion

    #region default Unity methods

    private void Awake()
    {
        _friends = new List<FriendInfo>();
        PhotonConnector.GetPhotonFriends += HandleGetPhotonFriends;
        UIAddFriend.OnAddFriend += HandleAddPlayFabFriend;
        UIFriend.OnRemoveFriend += HandleRemoveFriend;
    }

    private void OnDestroy()
    {
        PhotonConnector.GetPhotonFriends -= HandleGetPhotonFriends;
        UIAddFriend.OnAddFriend -= HandleAddPlayFabFriend;
        UIFriend.OnRemoveFriend -= HandleRemoveFriend;
    }


    #endregion

    #region public methods

    #endregion

    #region private methods
    private void GetPlayfabFriends()
    {
        var request = new GetFriendsListRequest { XboxToken = null};
        PlayFabClientAPI.GetFriendsList(request, OnGetFriendListSuccess, OnGetFriendListFailure);
    }

    private void HandleGetPhotonFriends()
    {
        GetPlayfabFriends();
    }

    private void HandleAddPlayFabFriend(string name)
    {
        var request = new AddFriendRequest { FriendTitleDisplayName = name};
        PlayFabClientAPI.AddFriend(request, OnFriendAddedSuccess, OnFriendAddFailure);
    }

    private void HandleRemoveFriend(string name)
    {
        string id = _friends.FirstOrDefault(f => f.TitleDisplayName == name).FriendPlayFabId;
        var request = new RemoveFriendRequest { FriendPlayFabId = id };
        PlayFabClientAPI.RemoveFriend(request, OnRemoveFriendSuccess, OnRemoveFriendFailure);
    }

    #endregion

    #region PlayFab Callbacks
    private void OnFriendAddFailure(PlayFabError error)
    {
        Debug.Log("Friend NOt Added: " + error.ToString());
    }

    private void OnFriendAddedSuccess(AddFriendResult result)
    {
        //Debug.Log("Friend Added: " + result.ToString());
        GetPlayfabFriends();
    }

    private void OnGetFriendListFailure(PlayFabError error)
    {
        Debug.LogError("Could not get friend list: "+ error.ToString());
    }

    private void OnGetFriendListSuccess(GetFriendsListResult result)
    {
        _friends = result.Friends;
        //Debug.Log("Got friend list: " + result.ToString());
        OnFriendListUpdated?.Invoke(result.Friends);
    }
    private void OnRemoveFriendSuccess(RemoveFriendResult result)
    {
        //Debug.Log("Friend Removed: " + result.ToString());

        GetPlayfabFriends();
    }

    private void OnRemoveFriendFailure(PlayFabError error)
    {
        Debug.LogError(error.ToString());
    }

    #endregion
}
