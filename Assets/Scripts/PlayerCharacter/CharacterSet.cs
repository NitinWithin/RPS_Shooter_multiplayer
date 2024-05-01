using Photon.Pun;
using UnityEngine;

public class CharacterSet : MonoBehaviour
{
    #region Variables
    [SerializeField] private Material[] _headSignSprites;
    [SerializeField] private GameObject[] _characterRock;
    [SerializeField] private GameObject[] _characterPaper;
    [SerializeField] private GameObject[] _characterScissors;
    MeshRenderer _headSignMeshRenderer;

    int _characterSelection;
    #endregion

    #region Default Unity Methods
    private void Awake()
    {
        _headSignMeshRenderer = transform.Find("headsign").GetComponent<MeshRenderer>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.LogWarning("player Nick name : " + player.NickName);
            if(player.IsLocal && player.CustomProperties.ContainsKey("CSN"))
            {
                Debug.LogWarning("player CSN: " + (int)player.CustomProperties["CSN"]);
                _characterSelection = (int)player.CustomProperties["CSN"];

                switch (_characterSelection)
                {
                    case 0:
                        _headSignMeshRenderer.material = _headSignSprites[0];
                        foreach (GameObject character in _characterRock)
                        {
                            character.SetActive(true);
                        }
                        break;
                    case 1:
                        _headSignMeshRenderer.material = _headSignSprites[1];
                        foreach (GameObject character in _characterPaper)
                        {
                            character.SetActive(true);
                        }
                        break;
                    case 2:
                        _headSignMeshRenderer.material = _headSignSprites[2];
                        foreach (GameObject character in _characterScissors)
                        {
                            character.SetActive(true);
                        }
                        break;
                }
            }
            /*if (item.CustomProperties.ContainsKey("CSN"))
            {
                if(item.IsLocal)
                {
                    _characterSelection = (int)item.CustomProperties["CSN"];

                    
                }
            }*/
        }

    }
    #endregion

}
