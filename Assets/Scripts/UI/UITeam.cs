using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UITeam : MonoBehaviour
{
    #region Variables
    [SerializeField] private int _teamSize;
    [SerializeField] private int _maxTeamSize;
    [SerializeField] private PhotonTeam _team;
    [SerializeField] private TMP_Text _teamNameText;
    [SerializeField] private Transform _playerSelectionContainer;
    [SerializeField] private UIPlayerSelection _playerSelectionPrefab;
    [SerializeField] private Dictionary<Player, UIPlayerSelection> _playerSelection;

    public static Action<PhotonTeam> OnSwitchToTeam = delegate { };
    #endregion

    #region Default Unity Methods

    public void Initialize(PhotonTeam team, int teamSize)
    {
        _team = team;
        _maxTeamSize = teamSize;
        _teamSize = teamSize; 

        _playerSelection = new Dictionary<Player, UIPlayerSelection>();
        UpdateTeamUI();

        Player[] teamMembers;
        if(PhotonTeamsManager.Instance.TryGetTeamMembers(_team.Code, out teamMembers))
        {
            foreach(Player member in teamMembers)
            {
                AddPlayerToTeam(member);
            }
        }
    }
    private void Awake()
    {
        UIDisplayTeam.OnAddPlayerToTeam += HandleAddPlayerToTeam;
        UIDisplayTeam.OnRemovePlayerFromTeam += HandleRemovePlayerFromTeam;
        PhotonRoomController.OnRoomLeft += HandleRoomLeft;
    }

    private void OnDestroy()
    {
        UIDisplayTeam.OnAddPlayerToTeam -= HandleAddPlayerToTeam;
        UIDisplayTeam.OnRemovePlayerFromTeam -= HandleRemovePlayerFromTeam;
        PhotonRoomController.OnRoomLeft -= HandleRoomLeft;
    }

    #endregion

    #region Private Methods
    private void HandleRoomLeft()
    {
        Destroy(gameObject);
    }

    private void HandleRemovePlayerFromTeam(Player player)
    {
        RemovePlayerFromTeam(player);
    }

    private void HandleAddPlayerToTeam(Player player, PhotonTeam team)
    {
        if(_team.Code == team.Code)
        {
            AddPlayerToTeam(player);
        }
    }

    private void UpdateTeamUI()
    {
        _teamNameText.SetText(_team.Name + "\n" + _playerSelection.Count() + " / " + _maxTeamSize);
    }

    private void AddPlayerToTeam(Player player)
    {
        UIPlayerSelection uiPlayerSelection = Instantiate(_playerSelectionPrefab, _playerSelectionContainer);
        uiPlayerSelection.Initialize(player);
        _playerSelection.Add(player,uiPlayerSelection);
        UpdateTeamUI();
    }

    private void RemovePlayerFromTeam(Player player)
    {
        if(_playerSelection.ContainsKey(player))
        {
            Destroy(_playerSelection[player].gameObject);
            _playerSelection.Remove(player);
            UpdateTeamUI() ;
        }
    }

    #endregion

    #region Public methods
    public void SwitchToTeam()
    {
        if (_teamSize >= _maxTeamSize)
        {
            return;
        }

        OnSwitchToTeam?.Invoke(_team);
    }
    #endregion


}
