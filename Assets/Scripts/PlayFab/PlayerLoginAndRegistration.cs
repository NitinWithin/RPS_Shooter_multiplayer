using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Collections.Generic;


public class PlayerLoginAndRegistration : MonoBehaviour
{
    #region Variables
    private Color _originalColor;
    private String _originalText;

    private string _loginuseremail;
    private string _loginpassword;

    private string _regUserName;
    private string _regPassword;
    private string _regEmail;

    [SerializeField] private TextMeshProUGUI _errorLabel;
    [SerializeField] private TextMeshProUGUI _regerrorLabel;
    [SerializeField] private Canvas _registrationCanvas;
    [SerializeField] private Canvas _loginCanvas;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private TMP_InputField _confirmPasswordField;
    #endregion

    #region Default Unity Methods
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _originalColor = _errorLabel.color;
        _originalText = _errorLabel.text;
        _errorLabel.enabled = false;
        _regerrorLabel.enabled = false;
        _registrationCanvas.enabled = false;
    }

    #endregion

    #region Playfab Callbacks

    private void OnFailure(PlayFabError error)
    {
        
        Debug.LogError("Error: email login fail. trying username login" + error.ToString());
    }

    private void OnLoginWithEmailIDSuccess(LoginResult result)
    {
        _errorLabel.enabled = false;
        Debug.Log("Login Success : " + result);
        
        GetPlayerDetails(_loginuseremail);
    }

    private void OnRegistrationFailure(PlayFabError error)
    {
        Debug.Log("Registration Failed: " + error);
    }

    private void OnRegisterationSuccess(RegisterPlayFabUserResult result)
    {
        GoBackToLogin();

        ShowMessage("Registration Successful", Color.green, 5f);
        
    }
    private void GetAccountInfoSuccess(GetAccountInfoResult result)
    {
        PlayerPrefs.SetString("USERNAME", result.AccountInfo.Username);
        PlayerPrefs.SetString("PLAYFABID", result.AccountInfo.PlayFabId);
        CheckUserSessionID(result.AccountInfo.PlayFabId);
        //SceneManager.LoadScene("MainMenu");
    }

    private void GetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError("Could not find PlayerInfo: " + error.ToString());
    }

    private void OnUserNameLoginFailure(PlayFabError error)
    {
        _errorLabel.enabled = true;
        Debug.Log("UserName Login failed: " + error.ToString());
    }

    private void OnLoginwithUserNameSuccess(LoginResult result)
    {
        Debug.Log("UserName Login success: " + result.PlayFabId);
        GetPlayerDetails(_loginuseremail);
    }

    private void GetUserDataFail(PlayFabError error)
    {
        Debug.Log("Get User data error " + error.ToString());
    }

    private void GetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data.ContainsKey("CURRENTSESSIONID"))
        {
            var LastSessionID = result.Data["CURRENTSESSIONID"].Value;


            if (LastSessionID == SystemInfo.deviceUniqueIdentifier)
            {
                Debug.Log("SessionIdMatch");
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                ShowMessage("User Logined elsewhere. pls login out in the other device first", Color.red, 5f);
            }
        }
        else
        {
            UpdateUserSessionID();
        }
    }

    private void UpdateUserDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log("UserData updated");
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateUserDataFail(PlayFabError error)
    {
        Debug.Log("UserData update failed:  " + error.ToString());
    }

    #endregion

    #region private methods

    private void UpdateUserSessionID()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>() { { "CURRENTSESSIONID", SystemInfo.deviceUniqueIdentifier } }
        };
        PlayFabClientAPI.UpdateUserData(request, UpdateUserDataSuccess, UpdateUserDataFail);
    }


    private void CheckUserSessionID(string PlayFabID)
    {
        var request = new GetUserDataRequest { PlayFabId = PlayFabID };
        PlayFabClientAPI.GetUserData(request, GetUserDataSuccess, GetUserDataFail);
    }

    private void GetPlayerDetails(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            if (IsValideUserEmail())
            {
                var request = new GetAccountInfoRequest { Email = email };
                PlayFabClientAPI.GetAccountInfo(request, GetAccountInfoSuccess, GetAccountInfoFailure);
            }
            else
            {
                var request = new GetAccountInfoRequest { Username = email };
                PlayFabClientAPI.GetAccountInfo(request, GetAccountInfoSuccess, GetAccountInfoFailure);
            }
        }
    }

    private void ShowMessage(string message, Color color, float duration)
    {
        _errorLabel.text = message;
        _errorLabel.color = color;

        _errorLabel.enabled = true;
        StartCoroutine(RevertChangesAfterDelay(duration));
    }

    private IEnumerator RevertChangesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _errorLabel.color = _originalColor;
        _errorLabel.text = _originalText;

        _errorLabel.enabled = false;
    }
    private bool IsValideUserEmail()
    {
        // Regular expression pattern for validating email addresses
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Use Regex.IsMatch to check if the email matches the pattern
        return Regex.IsMatch(_loginuseremail, pattern);
    }

    private void LoginWithEmailID()
    {
        var request = new LoginWithEmailAddressRequest { Email = _loginuseremail, Password = _loginpassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginWithEmailIDSuccess, OnFailure);
    }
    private void LoginWithUserName()
    {
        var request = new LoginWithPlayFabRequest { Username = _loginuseremail, Password = _loginpassword };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginwithUserNameSuccess, OnUserNameLoginFailure);
    }

    private void RegisterUser()
    {
        Debug.Log("Inside register USer");
        if (_regUserName != null &&  _regPassword != null && _regEmail != null)
        {
            var request = new RegisterPlayFabUserRequest {Email = _regEmail,
                                                          Password = _regPassword,
                                                          DisplayName = _regUserName,
                                                          Username = _regUserName };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterationSuccess, OnRegistrationFailure);
        }
    }

    #endregion

    #region public methods
    public void SetUserEmail(string email)
    { 
        _loginuseremail = email;
        PlayerPrefs.SetString("USEREMAIL", _loginuseremail.Trim());
    }

    public void SetPassword(string password)
    {
        _loginpassword = password;
        PlayerPrefs.SetString("USERPASS", _loginpassword.Trim());
    }

    public void EnableRegistration()
    {
        _loginCanvas.enabled = false;
        _registrationCanvas.enabled = true;
    }

    public void GoBackToLogin()
    {
        _registrationCanvas.enabled = false;
        _loginCanvas.enabled = true;
    }

    public void SetRegUserName(String userName)
    {
        _regUserName = userName;
    }

    public void SetRegEmail(string email)
    {
        _regEmail = email;
    }

    public void SetRegPassword(string password)
    {
        if (_passwordField.text.Length < 6)
        {
            ShowMessage("Password has to be more than 6 characters long", Color.red, 5f);
        }
        else if (_passwordField != null &&
                    _confirmPasswordField != null &&
                    _passwordField.text == _confirmPasswordField.text)
        {
            _regPassword = password;
            Debug.Log("Password set" + _regPassword);
        }
        else
        {
            _regerrorLabel.enabled = true;
        }

    }

    public void Login()
    {
        if(IsValideUserEmail()) 
        {
            LoginWithEmailID(); 
        }
        else
        {
            LoginWithUserName();
        }
        
    }

    public void Registration()
    {
        Debug.Log("Registration click");
        RegisterUser();
    }    

    #endregion
}
