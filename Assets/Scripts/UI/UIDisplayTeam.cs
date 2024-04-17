using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class UIDisplayTeam : MonoBehaviour
{
    #region Variables
    [SerializeField] private UITeam _uiTeamPrefab;
    [SerializeField] private List<UITeam> _uiTeams;
    [SerializeField] private Transform _teamContainer;

    public static Action<Player, PhotonTeam> OnAddPlayerToTeam = delegate {  };
    public static Action<Player> OnRemovePlayerFromTeam = delegate { };
    #endregion

    #region Default Unity Methods
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonTeamController.OnCreateTeams += HandleCreateTeams;
        PhotonTeamController.OnSwitchTeam += HandleSwitchTeam;
        PhotonTeamController.OnRemovePlayer += HandleRemovePlayer;
        PhotonTeamController.OnClearTeam += HandleClearTeam;
    }

    private void OnDestroy()
    {
        PhotonTeamController.OnCreateTeams -= HandleCreateTeams;
        PhotonTeamController.OnSwitchTeam += HandleSwitchTeam;
        PhotonTeamController.OnRemovePlayer += HandleRemovePlayer;
        PhotonTeamController.OnClearTeam -= HandleClearTeam;
    }

    #endregion

    #region Private Methods
    private void HandleClearTeam()
    {
        foreach(UITeam uiTeam in _uiTeams)
        {
            Destroy(uiTeam.gameObject);
        }
        _uiTeams.Clear();
    }

    private void HandleRemovePlayer(Player OtherPlayer)
    {
        OnRemovePlayerFromTeam(OtherPlayer);
    }

    private void HandleSwitchTeam(Player player, PhotonTeam team)
    {
        OnRemovePlayerFromTeam?.Invoke(player);
        OnAddPlayerToTeam(player, team);
    }

    private void HandleCreateTeams(List<PhotonTeam> teamList, GameMode mode)
    {
        foreach(PhotonTeam team in teamList)
        {
            UITeam uiTeam = Instantiate(_uiTeamPrefab, _teamContainer);
            uiTeam.Initialize(team, mode.TeamSize);
            _uiTeams.Add(uiTeam);
        }
    }
    #endregion

}
