using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLogin : MonoBehaviour
{
    #region Variables

     private string _useremail;
     private string _password;

    [SerializeField] private TextMeshProUGUI _errorLabel;
    #endregion

    #region Default Unity Methods
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _errorLabel.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region PUN Callbacks

    #endregion

    #region Playfab Callbacks
    private void OnGetPlayerProfileSuccess(GetPlayerProfileResult result)
    {
        string _username = result.PlayerProfile.DisplayName;
        PlayerPrefs.SetString("USERNAME", _username);
    }

    private void OnFailure(PlayFabError error)
    {
        _errorLabel.enabled = true;
        Debug.LogError("Error: " + error.ToString());
       
    }

    private void OnLoginWithEmailIDSuccess(LoginResult result)
    {
        _errorLabel.enabled = false;
        Debug.Log("Login Result : " + result);
    }

    #endregion

    #region private methods
    private bool IsValideUserEmail()
    {
        bool _isValid = false;
        if(_useremail.Length > 0) 
        {
            _isValid = true;
        }
        return _isValid;
    }

    private void LoginWithEmailID()
    {
        Debug.Log("Login in with " + _useremail + " and " +  _password);
        var request = new LoginWithEmailAddressRequest { Email = _useremail, Password = _password };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginWithEmailIDSuccess, OnFailure);
    }



    #endregion

    #region public methods
    public void SetUserEmail(string email)
    {
        _useremail = email;
        PlayerPrefs.SetString("USEREMAIL", _useremail);
        Debug.Log("EMAIL: " + PlayerPrefs.GetString("USEREMAIL"));
    }

    public void SetPassword(string password)
    {
        _password = password;
        PlayerPrefs.SetString("USERPASS", _password);
        Debug.Log("password: " + PlayerPrefs.GetString("USERPASS"));
    }

    public void Login()
    {
        if(!IsValideUserEmail()) 
        {
            
            Debug.Log("NOTVAILD" + _useremail);
            return;
        }
        Debug.Log("email Valid");

        LoginWithEmailID();
    }


    #endregion
}
