using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIFriend : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _friendNameText;
    [SerializeField] private FriendInfo _friend;

    public static Action<string> OnRemoveFriend = delegate { };
    #endregion

    #region public methods
    public void Initialize(FriendInfo friend)
    {
        this._friend = friend;
        _friendNameText.SetText(this._friend.UserId);
    }

    public void RemoveFriend()
    {
        OnRemoveFriend?.Invoke(_friend.UserId);
    }
    #endregion
}
