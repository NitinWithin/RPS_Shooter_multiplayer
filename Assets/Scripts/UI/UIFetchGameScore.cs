using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class UIFetchGameScore : MonoBehaviour, IPunObservable
{
    #region Variables
    [SerializeField] private TMP_Text _teamAScore;
    [SerializeField] private TMP_Text _teamBScore;
    [SerializeField] private bool _checkWinCondition = false;

    #endregion

    #region Default Unity Methods

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
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
        if (int.Parse(_teamAScore.text) + int.Parse(_teamBScore.text) == 3 )
        {
           if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(5);
            }
        }
    }

    #endregion

    #region Playfab callbacks
    private void GetGameScoreSuccess(GetUserDataResult result)
    {
        if (result.Data.ContainsKey("TEAMASCORE") && result.Data.ContainsKey("TEAMBSCORE"))
        {
            _teamAScore.text = result.Data["TEAMASCORE"].Value;
            _teamBScore.text = result.Data["TEAMBSCORE"].Value;
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
            stream.SendNext(_teamAScore.text);
            stream.SendNext(_teamBScore.text);
        }
        else
        {
            _teamAScore.text = stream.ReceiveNext().ToString();
            _teamBScore.text = stream.ReceiveNext().ToString();
        }
    }

    #endregion
}
