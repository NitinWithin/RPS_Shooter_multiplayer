using Photon.Pun.UtilityScripts;
using Photon.Pun;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using System;
using TMPro;

public class GameMaster : MonoBehaviour, IPunObservable
{
    #region Variables
    [SerializeField] private GameObject _endRoundCanvas;
    private GameObject[] _playersInRoom;
    private int _deadPlayerCountTeamA;
    private int _deadPlayerCountTeamB;

    private bool _teamAWins = false, _teamBWins = false, _roundEnd = false;
    private int _teamBScore = 0, _teamAScore = 0;

    #endregion

    #region Default Unity Methods
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetPlayersInRoom();
            AllPlayersInTeamKilled();
        }

    }

    #endregion

    #region Private Methods
    private void GetPlayersInRoom()
    {
        _playersInRoom = GameObject.FindGameObjectsWithTag("Player");
    }

    private void AllPlayersInTeamKilled()
    {
        foreach (var player in _playersInRoom)
        {
            if (player.GetComponent<PhotonView>().Owner.GetPhotonTeam().Code == PhotonTeamsManager.Instance.GetAvailableTeams()[0].Code)
            {

                if(player.GetComponent<Damage>()._isDead)
                {
                    _deadPlayerCountTeamA++;
                }
            }
            else
            {
                if (player.GetComponent<Damage>()._isDead)
                {
                    _deadPlayerCountTeamB++;
                }
            }
        }

        if( _deadPlayerCountTeamA == 3 && !_roundEnd)
        {
            _roundEnd = true;
            _teamBWins = true;
            UpdatePlayFabWithRoundData();

        }
        else if( _deadPlayerCountTeamB == 3 && !_roundEnd)
        { 
            _roundEnd = true;
            _teamAWins = true;
            UpdatePlayFabWithRoundData();
        }
 
    }

    private void UpdatePlayFabWithRoundData()
    {
        GetScoresFromPlayFab();

        if (_teamBWins)
        {
            _teamBScore += 1;
        }
        if (_teamAWins)
        {
            _teamAScore += 1;
        }
        _endRoundCanvas.SetActive(true);
        if (_endRoundCanvas.activeSelf)
        {
            GetComponent<UIScoreUpdate>().Initialize(_teamAScore, _teamBScore);
        }
        UpdateScoreToPlayFab(_teamAScore, _teamBScore);
    }

    private void UpdateScoreToPlayFab(int teamAScore, int teamBScore)
    {
        var request = new UpdateUserDataRequest
        { 
            Data = new Dictionary<string, string> () { { "TEAMASCORE", teamAScore.ToString()},
                                                        { "TEAMBSCORE", teamBScore.ToString() }}
        
        };
        PlayFabClientAPI.UpdateUserData(request, UserDataUpdateSuccess, PlayFabCallBackFail);
    }

    private void GetScoresFromPlayFab()
    {
        var request = new GetUserDataRequest { PlayFabId = PlayerPrefs.GetString("PLAYFABID") };
        PlayFabClientAPI.GetUserData(request, GetScoreDataSuccess, PlayFabCallBackFail);
    }

    #endregion

    #region Public methods
    [PunRPC]
    public void RoundEndRPC(int teamAScore, int teamBScore)
    {
        GameObject.Find("TeamAScore").GetComponent<TMP_Text>().text = teamAScore.ToString();
        GameObject.Find("TeamBScore").GetComponent<TMP_Text>().text = teamBScore.ToString();

        if(_playersInRoom == null)
        {
            GetPlayersInRoom();
        }
        foreach (var player in _playersInRoom)
        {
            if (player.GetComponent<PhotonView>().Owner.IsLocal)
            {
                player.GetComponent<FPSController>().enabled = false;
                player.GetComponent<FireWeapon>().enabled = false;
            }
        }
    }
    #endregion

    #region Playfab callbacks

    private void GetScoreDataSuccess(GetUserDataResult result)
    {
        if (result.Data.ContainsKey("TEAMASCORE") && result.Data.ContainsKey("TEAMBSCORE"))
        {
            _teamAScore = int.Parse(result.Data["TEAMASCORE"].Value);
            _teamBScore = int.Parse(result.Data["TEAMBSCORE"].Value);
        }
        else
        {
            UpdateScoreToPlayFab(_teamAScore, _teamBScore);
        }
    }

    private void UserDataUpdateSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Score Updated : " + result.ToString());

        gameObject.GetComponent<PhotonView>().RPC("RoundEndRPC", RpcTarget.All, _teamAScore, _teamBScore);
        //RoundEndRPC();
    }

    private void PlayFabCallBackFail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_roundEnd);
        }
        else
        {
            _roundEnd = (bool)stream.ReceiveNext();
        }
    }

    #endregion
}
