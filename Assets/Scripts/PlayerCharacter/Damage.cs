using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Damage : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Variables
    [SerializeField] TMP_Text _healthText;
    [SerializeField] private int _health = 100;
    private Renderer[] _visuals;
    #endregion

    #region Default Methods 
    // Start is called before the first frame update
    void Start()
    {
        _visuals = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_health <= 0)
        {
            StartCoroutine(Respawn());
        }
    }
    #endregion

    #region PUN methods
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_health);
        }
        else
        {
            _health = (int)stream.ReceiveNext();
        }
    }
    #endregion

    #region Private Methods
    
    IEnumerator Respawn()
    {
        VisualizeRenderer(false); 
        _health = 100;
        GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(0,5,0);
        yield return new WaitForSeconds(1f);
        GetComponent<CharacterController>().enabled = true;
        VisualizeRenderer(true);
    }

    private void VisualizeRenderer(bool state)
    {
        foreach(Renderer renderer in _visuals)
        {
            renderer.enabled = state;
        }
    }

    #endregion

    #region public methods
    public void DoDamage(int damage)
    {
        _health -= damage;
        _healthText.text = _health.ToString();
    }
    #endregion
}
