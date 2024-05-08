using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class UIFetchGameScore : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _teamAScore;
    [SerializeField] private TMP_Text _teamBScore;

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
        if (PhotonNetwork.IsMasterClient)
        {
            var request = new GetUserDataRequest { PlayFabId = PlayerPrefs.GetString("PLAYFABID") };
            PlayFabClientAPI.GetUserData(request, GetGameScoreSuccess, PlayFabRequestFail);
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
