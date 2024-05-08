using Photon.Pun;
using System.Collections;
using UnityEngine;

public class UIRoundStartButton : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _startRoundButton;

    #endregion

    #region Default UNity methods

    private void Start()
    {
        StartCoroutine(RestartLevel());
    }
    #endregion

    #region Public methods

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(10f);
        PhotonNetwork.LoadLevel(3);
    }
    #endregion

}
