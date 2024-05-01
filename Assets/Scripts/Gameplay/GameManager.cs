using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private GameObject[] _TeamAplayerPrefab;
    [SerializeField] private GameObject[] _TeamBplayerPrefab;
    [SerializeField] private float delayInSeconds = 1f;

    private int _characterSelection;
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

        _characterSelection = (int)PhotonNetwork.LocalPlayer.CustomProperties["CSN"];

        switch (_characterSelection)
        {
            case 0:
                if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Code == 1)
                {
                    GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_TeamAplayerPrefab[0].name, _spawnPositions[1], Quaternion.identity);
                    OnPlayerInstantiated?.Invoke(instantiatedPlayer);
                }
                else
                {
                    GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_TeamBplayerPrefab[0].name, _spawnPositions[1], Quaternion.identity);
                    OnPlayerInstantiated?.Invoke(instantiatedPlayer);
                }
                break;
            case 1:
                if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Code == 1)
                {
                    GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_TeamAplayerPrefab[1].name, _spawnPositions[1], Quaternion.identity);
                    OnPlayerInstantiated?.Invoke(instantiatedPlayer);
                }
                else
                {
                    GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_TeamBplayerPrefab[1].name, _spawnPositions[1], Quaternion.identity);
                    OnPlayerInstantiated?.Invoke(instantiatedPlayer);
                }
                break;
            case 2:
                if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Code == 1)
                {
                    GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_TeamAplayerPrefab[2].name, _spawnPositions[1], Quaternion.identity);
                    OnPlayerInstantiated?.Invoke(instantiatedPlayer);
                }
                else
                {
                    GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_TeamBplayerPrefab[2].name, _spawnPositions[1], Quaternion.identity);
                    OnPlayerInstantiated?.Invoke(instantiatedPlayer);
                }

                break;
        }
        //     GameObject instantiatedPlayer = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPositions[1], Quaternion.identity);
        //     OnPlayerInstantiated?.Invoke(instantiatedPlayer);
    }

    #endregion

}
