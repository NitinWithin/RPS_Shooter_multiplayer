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
    [SerializeField] private GameObject _inviteButton;
    private bool _isOnline;
    private Color _onlineColor = Color.green;
    private Color _offlineColor = Color.grey;
    private string _friendName;

    public static Action<string> OnRemoveFriend = delegate { };
    public static Action<string> OnInviteFriend = delegate { };
    public static Action<string> OnGetCurrentStatus = delegate { };
    public static Action OnGetRoomStatus = delegate { };
    #endregion

    #region Default Unity Methods

    private void Awake()
    {
        PhotonChatController.OnStatusUpdated += HandleStatusUpdate;
        PhotonChatFriendController.OnStatusUpdated += HandleStatusUpdate;
        PhotonRoomController.OnRoomStatusChange += HandleRoomStatusChange;
    }

    private void OnDestroy()
    {
        PhotonChatController.OnStatusUpdated -= HandleStatusUpdate;
        PhotonChatFriendController.OnStatusUpdated -= HandleStatusUpdate;
        PhotonRoomController.OnRoomStatusChange -= HandleRoomStatusChange;
    }
    private void OnEnable()
    {
        if(!string.IsNullOrEmpty(_friendName))
        {
            OnGetCurrentStatus?.Invoke(_friendName);
            OnGetRoomStatus?.Invoke();
        }
    }

    #endregion

    #region private methods

    private void HandleRoomStatusChange(bool inRoom)
    {
        _inviteButton.SetActive(inRoom && _isOnline);
    }

    private void HandleStatusUpdate(PhotonStatus status)
    {
        Debug.Log("HANDLE STATUS UPDATE: " + status.PlayerName + " status: " + status.Status);
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
        _friendNameText.SetText(_friendName);
        _inviteButton.SetActive(false);
        OnGetCurrentStatus?.Invoke(_friendName);
    }

    public void PlayerStatus(int status)
    {
        Debug.Log("Player status : " + status);
        Debug.Log("Chat Online status: " + ChatUserStatus.Online);
        if(status == ChatUserStatus.Online)
        {
            _onlineImage.color = _onlineColor;
            _isOnline = true;
            OnGetRoomStatus?.Invoke();
        }
        else
        {
            _onlineImage.color = _offlineColor;
            _isOnline = false;
            _inviteButton?.SetActive(false);
        }
    }
    public void RemoveFriend()
    {
        OnRemoveFriend?.Invoke(_friend.UserId);
    }

    public void InviteFriend()
    {
        Debug.Log("CLICK TO INVITE : " + _friendName);
        OnInviteFriend?.Invoke(_friendName);
    }
    #endregion
}
