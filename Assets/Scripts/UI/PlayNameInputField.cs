using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class PlayNameInputField : MonoBehaviour
{
    const string _playerNamePrefKey = "PlayerName";
    // Start is called before the first frame update
    void Start()
    {
        string _defaultName = string.Empty;
        InputField _inputField = GetComponent<InputField>();

        if (_inputField != null )
        {
            _defaultName = PlayerPrefs.GetString(_playerNamePrefKey);
            _inputField.text = _defaultName;
        }
        PhotonNetwork.NickName = _defaultName;
    }

    public void SetPlayerName(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(_playerNamePrefKey, value);
    }
}
