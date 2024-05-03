using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class UIPauseMenu : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private GameObject _pauseMenuCanvas;

    #endregion

    #region Default Unity Methods
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Call a method to handle Escape key press
            PauseMenu();
        }
    }
    #endregion

    #region Private Methods

    private void PauseMenu()
    {
        _pauseMenuCanvas.SetActive(!_pauseMenuCanvas.activeSelf);
    }

    #endregion

    #region Public methods


    public void ResumeGame()
    {
        _pauseMenuCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region PUN callbacks
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Player otherPlayer has left the room
        Debug.Log("Player " + otherPlayer.NickName + " has left the room.");

        // You can perform any necessary actions here, such as cleaning up or updating UI
    }
    #endregion

}
