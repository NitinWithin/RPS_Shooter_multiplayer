using Photon.Pun;
using UnityEngine;

public class UIShowMenuforLocalPlayer : MonoBehaviour
{
    #region Variables
    [SerializeField] private Canvas[] _pauseMenuCanvas;

    #endregion

    #region Default Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        foreach (Canvas c in _pauseMenuCanvas)
        {
            //_canvasRenderer = _pauseMenuCanvas.GetComponent<Renderer>();
            if (PhotonView.Get(this).IsMine)
            {
                // This Canvas belongs to the local player, so activate it
                c.enabled = true;
            }
            else
            {
                // This Canvas does not belong to the local player, so deactivate it
                c.enabled = false;
            }
        }
        
    }

    #endregion

    #region Private Methods

    #endregion

    #region Public methods

    #endregion
}
