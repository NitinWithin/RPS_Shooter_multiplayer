using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICheckWinCondition : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _teamAWins;
    [SerializeField] private TMP_Text _teamBWins;

    private int _teamAScore = 0, _teamBScore = 0;
    #endregion

    #region Default Unity Methods

    void Start()
    {
        _teamAWins.enabled = false;
        _teamBWins.enabled = false;

        if (PhotonNetwork.IsMasterClient)
        {
            FetchScoreFromPlayFab();
        }
    }

    #endregion

    #region Private Methods

    private void FetchScoreFromPlayFab()
    {
        var request = new GetUserDataRequest { PlayFabId = PlayerPrefs.GetString("PLAYFABID") };
        PlayFabClientAPI.GetUserData(request, GetGameScoreSuccess, PlayFabRequestFail);

    }

    private void CheckWinCondition()
    {
        if (_teamAScore + _teamBScore == 3)
        {
            if (_teamAScore > _teamBScore)
            {
                Debug.Log("TEAM A wins");
                _teamAWins.enabled = true;
            }
            else if (_teamBScore > _teamAScore)
            {
                Debug.Log("TEAM B Wins");
                _teamBWins.enabled = true;
            }
        }
    }

    #endregion

    #region Playfab callbacks
    private void GetGameScoreSuccess(GetUserDataResult result)
    {
        if (result.Data.ContainsKey("TEAMASCORE") && result.Data.ContainsKey("TEAMBSCORE"))
        {
            _teamAScore = int.Parse(result.Data["TEAMASCORE"].Value);
            _teamBScore = int.Parse(result.Data["TEAMBSCORE"].Value);
        }

        CheckWinCondition();
    }

    private void PlayFabRequestFail(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_teamAWins.enabled);
            stream.SendNext(_teamBWins.enabled);
        }
        else
        {
            _teamAWins.enabled = bool.Parse(stream.ReceiveNext().ToString());
            _teamBWins.enabled = bool.Parse(stream.ReceiveNext().ToString());
        }
    }


    #endregion
}
