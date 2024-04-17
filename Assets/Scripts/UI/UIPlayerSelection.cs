using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSelection : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private TMP_Text _userNameText;
    [SerializeField] private Player _owner;
    [SerializeField] private Image _characterImage;
    [SerializeField] private GameObject _previousButton;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _kickButton;
    [SerializeField] private Sprite[] _characterSprites;
    [SerializeField] private int _currentSelection;

    private const string CHARACTER_SELECTION_NUMBER = "CSN";
    private const string KICKED_PLAYER = "KICKED";

    public static Action<Player> OnKickPlayer = delegate { };

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
        _currentSelection = GetCharacterSelection();
        SetUpPlayerSelection();
        UpdateCharacterModel(_currentSelection);
    }
    #endregion

    #region Private Methods

    private void SetUpPlayerSelection()
    {
        _userNameText.SetText(_owner.NickName);
        _kickButton.SetActive(false);

        if(PhotonNetwork.LocalPlayer.Equals(Owner))
        {
            _previousButton.SetActive(true);
            _nextButton.SetActive(true);
        }
        else
        {
            _previousButton.SetActive(false) ;
            _nextButton.SetActive(false) ;
        }

        if(PhotonNetwork.IsMasterClient)
        {
            ShowMasterClientUI();
        }
    }

    private void ShowMasterClientUI()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if(PhotonNetwork.LocalPlayer.Equals(_owner))
        {
            _kickButton.SetActive(false );
        }
        else
        {
            _kickButton.SetActive(true) ;
        }
    }

    private int GetCharacterSelection()
    {
        int selection = 0;
        object playerSelectionObject;
        if (Owner.CustomProperties.TryGetValue(CHARACTER_SELECTION_NUMBER, out playerSelectionObject))
        {
            selection = (int)playerSelectionObject;               
        }
        return selection;
    }

    private void UpdateCharacterSelection(int selection)
    {
        Hashtable playerSelectionProperty = new Hashtable()
        {
            {CHARACTER_SELECTION_NUMBER, selection }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperty);
    }

    private void UpdateCharacterModel(int selection)
    {
        _characterImage.sprite = _characterSprites[selection]; 
    }
    #endregion

    #region Public methods
    public void PreviousSelection()
    {
        _currentSelection--;
        if( _currentSelection < 0 )
        {
            _currentSelection = _characterSprites.Length - 1;
        }
        UpdateCharacterSelection( _currentSelection );
    }

    public void NextSelection() 
    {
        _currentSelection++;
        if( _currentSelection > _characterSprites.Length - 1)
        {
            _currentSelection = 0;
            
        }
        UpdateCharacterSelection( _currentSelection );
    }

    public void KickPlayer()
    {
        Hashtable kickedProperty = new Hashtable()
        {
            { KICKED_PLAYER, true }
        };
        Owner.SetCustomProperties(kickedProperty);
    }
    #endregion

    #region PUN Callbacks
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (Owner.Equals(newMasterClient))
        {
            ShowMasterClientUI();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!Owner.Equals(targetPlayer)) return;

        object characterSelectedNumberObject;
        if (changedProps.TryGetValue(CHARACTER_SELECTION_NUMBER, out characterSelectedNumberObject))
        {
            _currentSelection = (int)characterSelectedNumberObject;
            UpdateCharacterModel(_currentSelection);
        }

        object kickedPlayerObject;
        if (changedProps.TryGetValue(KICKED_PLAYER, out kickedPlayerObject))
        {
            bool kickedPlayer = (bool)kickedPlayerObject;
            if (kickedPlayer)
            {
                Hashtable kickedProperty = new Hashtable()
                    {
                        {KICKED_PLAYER, false}
                    };
                Owner.SetCustomProperties(kickedProperty);

                OnKickPlayer?.Invoke(Owner);
            }
        }
    }
    #endregion
}
