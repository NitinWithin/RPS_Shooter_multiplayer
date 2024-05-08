using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScoreUpdate : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _startRoundButton;
    [SerializeField] private TMP_Text _teamAScoreText, _teamBScoreText;
    private int _teamAScore, _teamBScore;

#endregion

#region Default Unity Methods

    public void Initialize(int teamAScore, int teamBScore)
    {
        _teamAScore = teamAScore;
        _teamBScore = teamBScore;
    }

    private void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            _startRoundButton.SetActive(false);
        }
    }

    void Update()
    {
        _teamAScoreText.text = _teamAScore.ToString();
        _teamBScoreText.text = _teamBScore.ToString();
    }

    #endregion

    #region Public methods

    public void StartNextRound()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            gameObject.GetComponent<PhotonView>().RPC("RestartLevel", RpcTarget.All);
        }

    }

    #endregion

    #region Private methods

    [PunRPC]
    private void RestartLevel()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
    }
    #endregion

}
