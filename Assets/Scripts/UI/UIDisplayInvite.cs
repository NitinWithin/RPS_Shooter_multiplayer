using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayInvite : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _partySystem;
    [SerializeField] private Transform _inviteContainer;
    [SerializeField] private UIInvite _uiInvitePrefab;

    private List<UIInvite> _inviteList;
    #endregion

    #region Default Unity Methods

    private void Awake()
    {
        _inviteList = new List<UIInvite>();
        PhotonChatController.OnRoomInvite += HandleRoomInvite;
        UIInvite.OnInviteAccept += HandleInviteAccept;
        UIInvite.OnInviteReject += HandleInviteReject;
    }

    private void OnDestroy()
    {
        PhotonChatController.OnRoomInvite -= HandleRoomInvite;
        UIInvite.OnInviteAccept -= HandleInviteAccept;
        UIInvite.OnInviteReject -= HandleInviteReject;
    }

    #endregion

    #region Private Methods

    private void HandleRoomInvite(string friend, string room)
    {
        _partySystem.GetComponent<UIToggle>().ToggleObjects();
        Debug.Log("HandleRoomInvite: From: " + friend + " RoomName: " + room);
        UIInvite uiprefab = Instantiate(_uiInvitePrefab, _inviteContainer);
        uiprefab.Initialize(friend, room);
        _inviteList.Add(uiprefab);
    }

    private void HandleInviteReject(UIInvite invite)
    {
        if (_inviteList.Contains(invite))
        {
            _inviteList.Remove(invite);
            Destroy(invite.gameObject);
        }

    }

    private void HandleInviteAccept(UIInvite invite)
    {
        if(_inviteList.Contains(invite))
        {
            _inviteList.Remove(invite);
            Destroy(invite.gameObject);
        }
        
    }
    #endregion

}
