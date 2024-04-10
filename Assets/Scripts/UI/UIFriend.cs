using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIFriend : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _friendNameText;
    [SerializeField] private FriendInfo _friend;
    [SerializeField] private Image _onlineImage;
    private Color _onlineColor = Color.green;
    private Color _offlineColor = Color.grey; 

    public static Action<string> OnRemoveFriend = delegate { };
    public static Action<string> OnInviteFriend = delegate { };
    #endregion

    #region public methods
    public void Initialize(FriendInfo friend)
    {
        this._friend = friend;
        _friendNameText.SetText(this._friend.UserId);

        PlayerStatus();
    }

    public void PlayerStatus()
    {
        if(_friend.IsOnline)
        {
            _onlineImage.color = _onlineColor;
        }
        else
        {
            _onlineImage.color = _offlineColor; 
        }
    }
    public void RemoveFriend()
    {
        OnRemoveFriend?.Invoke(_friend.UserId);
    }

    public void InviteFriend()
    {
        Debug.Log("CLICK TO INVITE : " + _friend.UserId);
        OnInviteFriend?.Invoke(_friend.UserId);
    }
    #endregion
}
