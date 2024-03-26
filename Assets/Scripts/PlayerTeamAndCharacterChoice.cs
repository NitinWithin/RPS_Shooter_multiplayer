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
        _CharacterPanel.SetActive(false);
        Time.timeScale = 0f;      
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
        //UpdateTagAndLayer(_playerTag, _playerlayer);
        Time.timeScale = 1f;
    }

    public void setRedTeam()
    {
        _playerlayer = "Player_Red";
        _teamPanel.SetActive(false);
        _CharacterPanel.SetActive(true);
        //Destroy(_teamPanel);
        //Destroy(_CharacterPanel);
    }
    
    public void setBlueTeam()
    {

        _playerlayer = "Player_Blue";
        _teamPanel.SetActive(false);
        _CharacterPanel.SetActive(true);
    }

    public void setRockTeam()
    {
        _playerTag = "Rock";
        _CharacterPanel.SetActive(false);
        UpdateTagAndLayer(_playerTag, _playerlayer);
    }
    
    public void setPaperTeam()
    {
        _playerTag = "Paper";
        _CharacterPanel.SetActive(false);
        UpdateTagAndLayer(_playerTag, _playerlayer);
    }
    
    public void setScissorsTeam()
    {
        _playerTag = "Scissors";
        _CharacterPanel.SetActive(false);
        UpdateTagAndLayer(_playerTag, _playerlayer);
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

    // Call this method to update the tag and layer for a specific player
    public void UpdateTagAndLayer(string character, string team)
    {
       foreach(Player _player in PhotonNetwork.PlayerList)
        {
            if(_player.IsLocal)
            {
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable
                {
                    { "Team", PlayerPrefs.GetString("team") },
                    { "Character", PlayerPrefs.GetString("character") }
                };
                
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

                if (_player.CustomProperties["Team"] != null)
                {
                    // Use the custom property value
                    Debug.Log("TEAM: " + (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] + " for " + _player.NickName);
                }
                else
                {
                    Debug.LogWarning("Custom property 'team' not found or has invalid type for Player " + _player.NickName);
                }
                if (_player.CustomProperties["Character"] != null)
                {
                    // Use the custom property value
                    Debug.Log("Character: " + (string)_player.CustomProperties["Team"] + " for " + _player.NickName);
                }
                else
                {
                    Debug.LogWarning("Custom property 'character' not found or has invalid type for Player " + _player.NickName);
                }
            }
            

        }
    }
}
