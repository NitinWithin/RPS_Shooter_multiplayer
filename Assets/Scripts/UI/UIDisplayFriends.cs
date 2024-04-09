using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayFriends : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _friendContainer;
    [SerializeField] private UIFriend _friendPrefab;
    #endregion

    #region Default Unity Methods

    private void Awake()
    {
        PhotonFriendsController.OnDisplayFriends += HandleDisplayfriends;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        PhotonFriendsController.OnDisplayFriends -= HandleDisplayfriends;
    }


    #endregion

    #region Private Methods
    private void HandleDisplayfriends(List<FriendInfo> friends)
    {
        foreach(Transform child in _friendContainer)
        {
            Destroy(child.gameObject); 
        }

        foreach(FriendInfo friend in friends)
        {
            UIFriend uifriend = Instantiate(_friendPrefab, _friendContainer);
            uifriend.Initialize(friend);
        }
    }
    #endregion

    #region Public methods

    #endregion

    #region PUN callbacks

    #endregion

    #region Playfab callbacks

    #endregion
}
