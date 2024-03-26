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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("team");
        PlayerPrefs.DeleteKey("character");
        PlayerPrefs.Save();
    }
    #endregion

    #region Private methods
    private IEnumerator DelayedInstantiatePlayer()
    {
        //Set Team and character Properties
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", PlayerPrefs.GetString("team") },
            { "Character", PlayerPrefs.GetString("character") }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPositions[1], Quaternion.identity);

        

        Debug.Log("PLayerPref: " + PlayerPrefs.GetString("team") );
        Debug.Log("Playerpref Character " + " and " + PlayerPrefs.GetString("character"));
    }

    #endregion

    #region public methods


    #endregion

}
