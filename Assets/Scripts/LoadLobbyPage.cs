using Photon.Pun.Demo.Cockpit.Forms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLobbyPage : MonoBehaviour
{
    public void LoadLobbypageScene()
    {
        SceneManager.LoadScene("RoomList");
    }

}
