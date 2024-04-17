using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIExitButton : MonoBehaviour
{
    #region public methods
    public void ExitGame()
    {
        DeleteUserSessionData();
        Application.Quit();
    }

    public void Logout()
    {
        DeleteUserSessionData();
        SceneManager.LoadScene("LoginOrRegister");
    }
    #endregion

    #region private methods

    private void DeleteUserSessionData()
    {
        var request = new UpdateUserDataRequest
        {
            KeysToRemove = new List<string> { "CURRENTSESSIONID" }
        };
        PlayFabClientAPI.UpdateUserData(request, SessionIDRemoveSuccess, SessionIDRemovefail);
    }
    #endregion

    #region Playfab Callbacks
    private void SessionIDRemoveSuccess(UpdateUserDataResult result)
    {
        Debug.Log("UserSessionID Deleted");
    }

    private void SessionIDRemovefail(PlayFabError error)
    {
        Debug.Log("UserSessionID Delete failed");
    }
    #endregion
}