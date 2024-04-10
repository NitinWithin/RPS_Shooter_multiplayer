using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInvite : MonoBehaviour
{
    #region Variables

    private string _friendName;
    private string _roomName;
    [SerializeField] private TMP_Text _friendNameText;

    public static Action<UIInvite> OnInviteAccept = delegate { };
    public static Action<UIInvite> OnInviteReject = delegate { };
    public static Action<string> OnRoomInviteAccept = delegate { };
    #endregion

    #region Public methods
    public void Initialize( string friendName, string roomName)
    {
        _friendName = friendName;
        _roomName = roomName;

        _friendNameText.SetText(friendName);
    }

    public void AcceptInvite()
    {
        OnInviteAccept?.Invoke(this);
        OnRoomInviteAccept?.Invoke(_roomName);
    }

    public void RejectInvite()
    {
        OnInviteReject?.Invoke(this);
    }

    
    #endregion
}
