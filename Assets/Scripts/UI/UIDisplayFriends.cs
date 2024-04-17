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
        PhotonChatFriendController.OnDisplayFriends += HandleDisplayfriends;

    }

    private void OnDestroy()
    {
        PhotonChatFriendController.OnDisplayFriends -= HandleDisplayfriends;
    }


    #endregion

    #region Private Methods
    private void HandleDisplayfriends(List<string> friends)
    {
        foreach(Transform child in _friendContainer)
        {
            Destroy(child.gameObject); 
        }

        foreach(string friend in friends)
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
