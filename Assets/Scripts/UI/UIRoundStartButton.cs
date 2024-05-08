using Photon.Pun;
using TMPro;
using UnityEngine;

public class UIRoundStartButton : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _startRoundButton;

    #endregion

    #region Public methods

    public void StartNextRound()
    {
        RestartLevel();

    }

    public void RestartLevel()
    {
        PhotonNetwork.LoadLevel(3);
    }
    #endregion

}
