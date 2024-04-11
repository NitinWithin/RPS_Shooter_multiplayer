using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Chat;

public class UIFriend : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _friendNameText;
    [SerializeField] private FriendInfo _friend;
    [SerializeField] private Image _onlineImage;
    private Color _onlineColor = Color.green;
    private Color _offlineColor = Color.grey;
    private string _friendName;

    public static Action<string> OnRemoveFriend = delegate { };
    public static Action<string> OnInviteFriend = delegate { };
    public static Action<string> OnGetCurrentStatus = delegate { };
    #endregion

    #region Default Unity Methods

    private void Awake()
    {
        PhotonChatController.OnStatusUpdated += HandleStatusUpdate;
        PhotonChatFriendController.OnStatusUpdated += HandleStatusUpdate;
    }

    private void OnDestroy()
    {
        PhotonChatController.OnStatusUpdated -= HandleStatusUpdate;
        PhotonChatFriendController.OnStatusUpdated -= HandleStatusUpdate;
    }
    private void OnEnable()
    {
        if(!string.IsNullOrEmpty(_friendName))
        {
            OnGetCurrentStatus?.Invoke(_friendName);
        }
    }

    #endregion

    #region private methods
    private void HandleStatusUpdate(PhotonStatus status)
    {
        if(string.Compare(_friendName,status.PlayerName) == 0)
        {
            PlayerStatus(status.Status);
        }
    }

    #endregion

    #region public methods
    public void Initialize(string _friendName)
    {
        this._friendName = _friendName;
        _friendNameText.SetText(this._friend.UserId);
        OnGetCurrentStatus?.Invoke(_friendName);
    }
    public void Initialize(FriendInfo friend)
    {
        _friendNameText.SetText(this._friend.UserId);

    }

    public void PlayerStatus(int status)
    {
        if(status == ChatUserStatus.Online)
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
