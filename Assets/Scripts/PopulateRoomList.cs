using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PopulateRoomList : MonoBehaviourPunCallbacks
{
    private RectTransform content; // Reference to the content of the scroll view
    public GameObject roomListItemPrefab; // Prefab for the room list item..
    [SerializeField] NetworkManager networkManager;
    private string RoomName;

    private float verticalSpacing = 15f;

    void Start()
    {
        content = transform.parent.GetComponent<RectTransform>();

    }

    public void RoomList(List<RoomInfo> rooms)
    {
        float currentYPosition = 0f;
        foreach (Transform child in content.transform)
        {
            // Check if the child object is a panel
            if (child.CompareTag("Panel"))
            {
                // Destroy the panel
                Destroy(child.gameObject);
            }
        }

        // Create room list items for each room
        foreach (RoomInfo room in rooms)
        {
            Debug.Log("RoomName: " + room.Name);
            Debug.Log("Content: " + content);
            
            // Instantiate room list item prefab
            GameObject listItem = Instantiate(roomListItemPrefab, content.transform);

            // Set position of the instantiated prefab
            RectTransform listItemRectTransform = listItem.GetComponent<RectTransform>();
            listItemRectTransform.anchoredPosition = new Vector2(0f, currentYPosition);

            // Update currentYPosition for the next prefab
            currentYPosition -= listItemRectTransform.sizeDelta.y + verticalSpacing;

            // Set room name
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = room.Name;
        }
    }

    public void JoinRoomFromLobby()
    {
        Debug.Log("Join room Button Clicked");
        // Get the display text from the selected panel


        string roomName = GetSelectedPanelDisplayText();
        // Check if the room name is not empty
        if (roomName != null)
        {
            Debug.Log("Button Clicked. joinging room: " + roomName);
            // Attempt to join the room with the specified name
            if (!PhotonNetwork.JoinRoom("Someroom"))
            {
                Debug.Log("Room not Joined");
            }
            else
            {
                Debug.Log("Room joined");
            }
        }
        else
        {
            Debug.LogWarning("Room name is empty.");
        }
    }

    // Method to fetch display text from the selected panel
    private string GetSelectedPanelDisplayText()
    {
        // Check if there is a selected GameObject
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null)
        {
            // Check if the selected GameObject has a Text component
            TextMeshProUGUI displayText = selectedObject.GetComponentInChildren<TextMeshProUGUI>();
            if (displayText != null)
            {
                Debug.LogWarning("Text component found ");
                // Return the display text
                return displayText.text;
            }
            else
            {
                Debug.LogWarning("Text component not found on selected panel.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject selected.");
        }

        return null;
    }
    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        // Debug.Log("RoomList update called from delete. has " + _roomList.Count);

        if (_roomList != null && _roomList.Count > 0)
        {
            RoomList(_roomList);
        }

    }
    public void closeRoom()
    {
        // Check if the local player is the master client and in a room
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
        {
            Debug.Log("Close Room, button clicked");
            Room currentRoom = PhotonNetwork.CurrentRoom;

            // Close the room
            currentRoom.IsOpen = false;
            currentRoom.IsVisible = false;

            // Kick all players from the room
            foreach (Player player in currentRoom.Players.Values)
            {
                PhotonNetwork.CloseConnection(player);
            }
            OnRoomListUpdate(null);
        }
    }

}