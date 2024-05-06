using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScoreUpdate : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _teamAScoreText, _teamBScoreText;
    private int _teamAScore, _teamBScore;
#endregion

#region Default Unity Methods

    public void Initialize(int teamAScore, int teamBScore)
    {
        _teamAScore = teamAScore;
        _teamBScore = teamBScore;
    }

    void Start()
    {
        _teamAScoreText.text = _teamAScore.ToString();
        _teamBScoreText.text = _teamBScore.ToString();
    }

    #endregion

    #region Public methods
    public void StartNextRound()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
#endregion

}
