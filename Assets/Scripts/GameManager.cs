using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float delayInSeconds = 1f;

    private Vector3[] _spawnPositions = {new Vector3(0, 8, 0), new Vector3(5, 8, 0)};

    #endregion

    #region Default methods
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedInstantiatePlayer());
    }

    #endregion

    #region Private methods
    private IEnumerator DelayedInstantiatePlayer()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPositions[1], Quaternion.identity);
    }

    #endregion

}
