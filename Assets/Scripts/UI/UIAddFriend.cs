using System;
using UnityEngine;

public class UIAddFriend : MonoBehaviour
{
    #region Variables
    private string _displayName;

    public static Action<string> OnAddFriend = delegate { };



    #endregion

    #region Default Unity methods

    #endregion

    #region private methods

    #endregion

    #region public methods

    public void SetAddFriendName(string displayName)
    {
        _displayName = displayName; 
    }

    public void AddFriend()
    {
        if(!string.IsNullOrEmpty(_displayName))
        {
            OnAddFriend?.Invoke(_displayName);
        }
        
    }

    #endregion
}
