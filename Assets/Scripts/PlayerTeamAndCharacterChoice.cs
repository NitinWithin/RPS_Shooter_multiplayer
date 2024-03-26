using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.UI;

public class PlayerTeamAndCharacterChoice : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _teamPanel, _CharacterPanel, _gameUI;
    [SerializeField] Button _redButton, _blueButton, _rockButton, _paperButton, _scissorsButton;
    private string _playerlayer, _playerTag;

    private void Start()
    {

        if (_gameUI != null &&
           _teamPanel != null &&
           _CharacterPanel != null &&
           _redButton != null &&
           _blueButton != null &&
           _rockButton != null &&
           _paperButton != null &&
           _scissorsButton != null)
        {
            Debug.LogError("ALL fields loaded000");
        }
        else
        {
            Debug.LogError("Something is null");
        }
        

        if (_teamPanel != null)
        {
            _redButton = _teamPanel.transform.Find("RedButton").GetComponent<Button>();
            _blueButton = _teamPanel.transform.Find("BlueButton").GetComponent<Button>();
        }

        Time.timeScale = 0f;
        _teamPanel.SetActive(true);
        _CharacterPanel.SetActive(false);
       
    }

    private void Update()
    {
        if (CheckOtherPlayersTeam("red") == 3)
        {
            _redButton.enabled = false;
        }
        else if (CheckOtherPlayersTeam("blue") == 3)
        {
            _blueButton.enabled = false;
        }

        DisableCharacterButtons();
        UpdateTagAndLayer(_playerTag, _playerlayer);
    }

    public void setRedTeam()
    {
        _playerlayer = "Player_Red";
        _teamPanel.SetActive(false);
        _CharacterPanel.SetActive(true);
    }
    
    public void setBlueTeam()
    {

        _playerlayer = "Player_Blue";
        Destroy(_teamPanel);
        _CharacterPanel.SetActive(true);
    }

    public void setRockTeam()
    {
        _playerTag = "Rock";
        Destroy(_CharacterPanel);
    }
    
    public void setPaperTeam()
    {
        _playerTag = "Paper";
        _CharacterPanel.SetActive(false);
    }
    
    public void setScissorsTeam()
    {
        _playerTag = "Scissors";
        _CharacterPanel.SetActive(false);
    }


    private int CheckOtherPlayersTeam(string team)
    {
        int _redcount = 0, _blueCount = 0;
        // Get all p1layers in the room
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Player player in players)
        {
            // Get the GameObject associated with the player
            GameObject _playerObject = GameObject.Find(player.NickName); // Assuming player's nickname is unique

            if (_playerObject != null)
            {
                // Check if the player's layer is Player_Red
                if (_playerObject.layer == LayerMask.NameToLayer("Player_Red"))
                {
                    _redcount++;

                }
                else if (_playerObject.layer == LayerMask.NameToLayer("Player_Blue"))
                {
                    _blueCount++;
                }
            }
        }

        if(team == "red")
        {
            return _redcount;
        }
        else if(team == "blue")
        { 
            return _blueCount;
        }
        return 0;
    }
    
    private void DisableCharacterButtons()
    {
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Player player in players)
        {
            // Get the GameObject associated with the player
            GameObject playerObject = GameObject.Find(player.NickName); // Assuming player's nickname is unique

            if (playerObject != null)
            {
                // Check if the player's layer is Player_Red
                if (playerObject.layer == LayerMask.NameToLayer("Player_Red"))
                {
                    if (playerObject.tag == "Rock")
                    {
                        _rockButton.enabled = false;
                    }
                    else if (playerObject.tag == "Paper")
                    {
                        _paperButton.enabled = false;
                    }
                    else if (playerObject.tag == "Scissors")
                    {
                        _scissorsButton.enabled = false;
                    }

                }
                else if (playerObject.layer == LayerMask.NameToLayer("Player_Blue"))
                {
                    if (playerObject.tag == "Rock")
                    {
                        _rockButton.enabled = false;
                    }
                    else if (playerObject.tag == "Paper")
                    {
                        _paperButton.enabled = false;
                    }
                    else if(playerObject.tag == "Scissors")
                    {
                        _scissorsButton.enabled = false;
                    }
                }
            }
        }
    }


    [PunRPC]
    void SetTagAndLayer(string tag, string layer)
    {
        if (photonView.IsMine) // Ensure the RPC is only executed on the local player's GameObject
        {
            photonView.gameObject.transform.parent.gameObject.tag = tag;
            photonView.gameObject.transform.parent.gameObject.layer = LayerMask.NameToLayer(layer);
            Debug.Log("RPC CALL SUCCESS");
        }
    }

    // Call this method to update the tag and layer for a specific player
    public void UpdateTagAndLayer(string tag, string layer)
    {
        if (PhotonNetwork.LocalPlayer ==
        {

            photonView.RPC("SetTagAndLayer", photonView.Owner, tag, layer);
        }
    }
}
