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


    #endregion

    #region Default Unity Methods

    void Start()
    {
        FetchScoreFromPlayFab();
    }

    #endregion

    #region Private Methods

    private void FetchScoreFromPlayFab()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var request = new GetUserDataRequest { PlayFabId = PlayerPrefs.GetString("PLAYFABID") };
            PlayFabClientAPI.GetUserData(request, GetGameScoreSuccess, PlayFabRequestFail);
        }

    }

    #endregion

    #region Public methods
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_teamAScore.text);
            stream.SendNext(_teamBScore.text);
        }
        else
        {
            _teamAScore.text = (string)stream.ReceiveNext();
            _teamBScore.text = (string)stream.ReceiveNext();
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
    }

    private void PlayFabRequestFail(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }


    #endregion
}
