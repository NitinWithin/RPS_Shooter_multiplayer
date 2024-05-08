using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Damage : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Variables
    [SerializeField] private int _health = 100;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _stunText;
    [SerializeField] private GameObject _DeathText;

    public bool _isDead = false;

    private Renderer[] _visuals;
    private bool isStunned = false;
    #endregion

    #region Default Methods 
    private void Awake()
    {
        _DeathText.SetActive(false);
        _stunText.enabled = false;
    }
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
            _isDead = true;
            _DeathText.SetActive( true);
            //StartCoroutine(Respawn());

            VisualizeRenderer(false);
            GetComponent<CharacterController>().enabled = false;
            GetComponent<FireWeapon>().enabled = false; 
            
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
        _healthText.text = _health.ToString();
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

    private IEnumerator StunCoroutine(GameObject _enemyPlayer, float stunDuration)
    {
        isStunned = true;
        _stunText.enabled = true;
        _enemyPlayer.GetComponent<FPSController>().enabled = false;
        _enemyPlayer.GetComponent<FireWeapon>().enabled = false;

        Debug.Log("Player is stunned");

        yield return new WaitForSeconds(stunDuration);

        _enemyPlayer.GetComponent<FPSController>().enabled = true;
        _enemyPlayer.GetComponent<FireWeapon>().enabled = true;

        Debug.Log("Player is no longer stunned");
        _stunText.enabled = false;
        isStunned = false;
    }

    #endregion

    #region public methods

    [PunRPC]
    public void StunPlayer(PhotonView _enemyPlayer, float stunDuration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(_enemyPlayer.gameObject, stunDuration));
        }
    }

    [PunRPC]
    public void DoDamage(int damage)
    {
        _health -= damage;
        //_healthText.GetComponent<UIHealthUpdate>().HandleHealthUpdate(_health.ToString());
        _healthText.text = _health.ToString();
    }

    [PunRPC]
    public void PushPlayersBack(GameObject _player1, GameObject _player2)
    {
        if (_player1.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("This RB is not null: " + _player1.name);
        }

        if (_player2.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("this RB is not null: " + _player2.name);
        }
        Rigidbody _rb1 = _player1.GetComponent<Rigidbody>();
        Rigidbody _rb2 = _player2.GetComponent<Rigidbody>();

        if (_rb1 != null && _rb2 != null)
        {
            // Calculate direction from player2 to player1
            Vector3 _pushDirection = (_player1.transform.position - _player2.transform.position).normalized;

            // Apply force to push players back
            _rb1.AddForce(_pushDirection * 10f, ForceMode.Impulse);
            _rb2.AddForce(-_pushDirection * 10f, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on one or both players.");
        }
    }

    #endregion
}
