using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CharacterSet : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] _characterRock;
    [SerializeField] private GameObject[] _characterPaper;
    [SerializeField] private GameObject[] _characterScissors;

    int _characterSelection;
    #endregion

    #region Default Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.CustomProperties.ContainsKey("CSN"))
            {
                if(item.IsLocal)
                {
                    _characterSelection = (int)item.CustomProperties["CSN"];
                    Debug.Log(_characterSelection);
                }
            }
        }

        switch (_characterSelection)
        {
            case 0:
                foreach (GameObject character in _characterRock)
                {
                    character.SetActive(true); 
                }
                break;
            case 1:
                foreach (GameObject character in _characterPaper)
                {
                    character.SetActive(true);
                }
                break;
            case 2:
                foreach (GameObject character in _characterScissors)
                {
                    character.SetActive(true);
                }
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Private Methods

    #endregion

    #region Public methods

    #endregion

    
}
