using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTeamController : MonoBehaviourPunCallbacks
{
    #region Variables
    private List<PhotonTeam> _roomTeams;
    private int _teamSize;
    private PhotonTeam _priorTeam;

    public static Action<List<PhotonTeam>, GameMode> OnCreateTeams = delegate { };
    public static Action<Player, PhotonTeam> OnSwitchTeam = delegate { };
    public static Action<Player> OnRemovePlayer = delegate { };
    public static Action OnClearTeam = delegate { };
    #endregion

    #region Default Unity Methods
    private void Awake()
    {
        UITeam.OnSwitchToTeam += HandleSwitchTeam;
        PhotonRoomController.OnJoinRoom += HandleCreateTeams;
        PhotonRoomController.OnRoomLeft += HandleRoomLeft;
        PhotonRoomController.OnOtherPlayerLeftRoom += HandleOtherPlayerLeftRoom;

        _roomTeams = new List<PhotonTeam>();
    }

    private void OnDestroy()
    {
        UITeam.OnSwitchToTeam -= HandleSwitchTeam;
        PhotonRoomController.OnJoinRoom -= HandleCreateTeams;
        PhotonRoomController.OnRoomLeft -= HandleRoomLeft;
        PhotonRoomController.OnOtherPlayerLeftRoom -= HandleOtherPlayerLeftRoom;
    }


    #endregion

    #region Private Methods

    private void HandleSwitchTeam(PhotonTeam newteam)
    {
        if(PhotonNetwork.LocalPlayer.GetPhotonTeam() == null)
        {
            _priorTeam = PhotonNetwork.LocalPlayer.GetPhotonTeam();
            PhotonNetwork.LocalPlayer.JoinTeam(newteam);
        }
        else if (CanSwitchToTeam(newteam))
        {
            _priorTeam = PhotonNetwork.LocalPlayer.GetPhotonTeam();
            PhotonNetwork.LocalPlayer.SwitchTeam(newteam);
        }
    }

    private void HandleOtherPlayerLeftRoom(Player player)
    {
        OnRemovePlayer?.Invoke(player);
    }

    private void HandleRoomLeft()
    {
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
        _roomTeams.Clear();
        _teamSize = 0;
        OnClearTeam?.Invoke();
    }

    private void HandleCreateTeams(GameMode mode)
    {
        Debug.Log("Heandle create team called in Team Controller");
        CreateTeams(mode);

        OnCreateTeams?.Invoke(_roomTeams, mode);

        AutoAssignPlayerToTeam(PhotonNetwork.LocalPlayer, mode);
    }

    private void AutoAssignPlayerToTeam(Player player, GameMode gameMode)
    {
        foreach (PhotonTeam team in _roomTeams)
        {
            int teamPlayerCount = PhotonTeamsManager.Instance.GetTeamMembersCount(team.Code);

            if (teamPlayerCount < gameMode.TeamSize)
            {
                Debug.Log("auto assigning " + player.NickName + " to " + team.Code);
                Debug.Log("GET photon team: " + player.GetPhotonTeam());
                if (player.GetPhotonTeam() == null)
                {
                    player.JoinTeam(team.Code);
                }
                else if (player.GetPhotonTeam().Code != team.Code)
                {
                    player.SwitchTeam(team.Code);
                }
                break;
            }
        }
    }

    private void CreateTeams(GameMode mode)
    {
        _teamSize = mode.TeamSize;
        int numberOfTeams = mode.MaxPlayers;
        if(mode.HasTeams)
        {
            numberOfTeams = mode.MaxPlayers / mode.TeamSize;
        }

        for(int i = 1; i <= numberOfTeams; i++)
        {
            _roomTeams.Add(new PhotonTeam
            {
                Name = "Team " + i,
                Code = (byte)i

            });
        }
    }

    private bool CanSwitchToTeam(PhotonTeam newteam)
    {
        bool canSwitch = false;

        if(PhotonNetwork.LocalPlayer.GetPhotonTeam().Code != newteam.Code)
        {
            Player[] players = null;
            if(PhotonTeamsManager.Instance.TryGetTeamMembers(newteam.Code, out players))
            {
                if(players.Length < _teamSize)
                {
                    canSwitch = true;
                }
                else
                {
                    Debug.Log("Team is full");
                }    
            }
        }
        else
        {
            Debug.Log("you already onm teh team: " + newteam.Name);
        }
        return canSwitch;
    }

    #endregion

    #region PUN callbacks
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        object teamCodeObject;
        if(changedProps.TryGetValue(PhotonTeamsManager.TeamPlayerProp, out teamCodeObject))
        {
            Debug.Log("team code for player: " + teamCodeObject);
            if (teamCodeObject == null) 
            {
                return;
            }
            byte teamcode = (byte)teamCodeObject;

            PhotonTeam newTeam;
            if(PhotonTeamsManager.Instance.TryGetTeamByCode(teamcode, out newTeam))
            {
                Debug.Log("Switching " + targetPlayer + " to new team " + newTeam.Name);
                OnSwitchTeam?.Invoke(targetPlayer, newTeam);
            }
        }
    }

    #endregion

}
