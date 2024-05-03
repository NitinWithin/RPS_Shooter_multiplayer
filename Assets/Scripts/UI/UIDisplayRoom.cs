using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class UIDisplayRoom : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _roomGameModeText;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private GameObject _roomContainer;
    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private GameObject[] _hideObjects;
    [SerializeField] private GameObject[] _showObjects;

    public static Action OnStartGame = delegate { };
    public static Action OnLeaveRoom = delegate { };
    #endregion

    #region Default Unity Methods
    private void Awake()
    {
        PhotonRoomController.OnJoinRoom += HandleOnJoinRoom;
        PhotonRoomController.OnRoomLeft += HandleOnRoomLeft;
    }

    private void OnDestroy()
    {
        PhotonRoomController.OnJoinRoom -= HandleOnJoinRoom;
        PhotonRoomController.OnRoomLeft -= HandleOnRoomLeft;
    }

    #endregion

    #region Private Methods
    private void HandleOnJoinRoom(GameMode mode)
    {
        _roomGameModeText.SetText(PhotonNetwork.CurrentRoom.CustomProperties["GAMEMODE"].ToString());

        _exitButton.SetActive(true);
        _roomContainer.SetActive(true);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _startGameButton.SetActive(true);
        }
        else
        {
            _startGameButton.SetActive(false);
        }

        foreach (GameObject go in _hideObjects)
        {
            go.SetActive(false);
        }
    }

    private void HandleOnRoomLeft()
    {
        _roomGameModeText.SetText("JOINING ROOM");

        _exitButton.SetActive(false);
        _roomContainer.SetActive(false);

        foreach (GameObject go in _showObjects)
        {
            go?.SetActive(true);
        }
    }

    #endregion

    #region Public methods
    public void LeaveRoom()
    {
        OnLeaveRoom?.Invoke();
    }

    public void StartGame()
    {
        Debug.Log($"Starting game...");
        OnStartGame?.Invoke();
    }
    #endregion


}
