using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerSelection : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _userNameText;
    [SerializeField] private Player _owner;

    public Player Owner 
    {
        get { return _owner; }
        set { _owner = value; }
    }

    #endregion

    #region Default Unity Methods
    public void Initialize(Player player)
    {
        _owner = player;
        SetUpPlayerSelection();
    }
    #endregion

    #region Private Methods

    private void SetUpPlayerSelection()
    {
        _userNameText.SetText(_owner.NickName);
    }
    #endregion

    
}
