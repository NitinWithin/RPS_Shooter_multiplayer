using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float delayInSeconds = 1f;

    private Vector3[] _spawnPositions = {new Vector3(0, 8, 0), new Vector3(5, 8, 0)};

    public static Action<GameObject> OnPlayerInstantiated = delegate { };
    #endregion

    #region Default methods
    void Start()
    {
        StartCoroutine(DelayedInstantiatePlayer());
    }

    #endregion

    #region Private methods
    private IEnumerator DelayedInstantiatePlayer()
    {
        yield return new WaitForSeconds(delayInSeconds);

        GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPositions[1], Quaternion.identity);
        OnPlayerInstantiated?.Invoke(instantiatedPlayer);
    }

    #endregion

}
