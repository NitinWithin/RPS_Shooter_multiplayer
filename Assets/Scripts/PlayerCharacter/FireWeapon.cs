using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] Transform _lasergun;
    [SerializeField] ParticleSystem _particleSystem;

    private GameObject _player;

    private bool isStunned = false;
    private float stunDuration = 2f;

    Dictionary<string, string> counters = new Dictionary<string, string>()
        {
            { "scissors", "paper" },
            { "paper", "rock" },
            { "rock", "scissors" }
        };
    #endregion

    #region Default methods
    // Start is called before the first frame update
    void Start()
    {
        _player = photonView.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            WeaponAim();
            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("RPC_Shoot", RpcTarget.AllBuffered);
            }
        }
    }
    #endregion

    #region private methods
    private void StunPlayer(GameObject _enemyPlayer)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(_enemyPlayer));
        }
    }
    private IEnumerator StunCoroutine(GameObject _enemyPlayer)
    {
        isStunned = true;
        _enemyPlayer.GetComponent<FPSController>().enabled = false;
        _enemyPlayer.GetComponent<FireWeapon>().enabled = false;

        Debug.Log("Player is stunned");

        yield return new WaitForSeconds(stunDuration);

        _enemyPlayer.GetComponent<FPSController>().enabled = true;
        _enemyPlayer.GetComponent<FireWeapon>().enabled = true;

        Debug.Log("Player is no longer stunned");
        isStunned = false;
    }


    private void WeaponAim()
    {
        // Get the position of the mouse cursor in screen space
        Vector3 mouseCursorPosition = Input.mousePosition;
        mouseCursorPosition.z = 15f;

        // Convert the mouse cursor position from screen space to world space
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mouseCursorPosition);

        // Make the gun's forward direction point towards the cursor position
        _lasergun.LookAt(targetPosition);
    }

    public void ApplyRPSLogic(GameObject _enemyPlayer)
    {
        _enemyPlayer.GetComponent<Damage>().DoDamage(20);
        // TODO: Change this to use PHOTON TEAMS

       /* if (_enemyPlayer.layer == photonView.gameObject.layer) // When in the same team
        {
            _enemyPlayer.GetComponent<Damage>().DoDamage(5);
        }
        else if(_enemyTag == _playerTag) // Same type
        {
            PushPlayersBack(_player.transform, _enemyPlayer.transform);
        }
        else
        {
            if (counters.ContainsKey(_playerTag) && counters[_playerTag] == _enemyTag) // Nemeis
            {
                _enemyPlayer.GetComponent<Damage>().DoDamage(20);
            }
            else
            {
                StunPlayer(_enemyPlayer);
            }
        }*/

    }

    public void PushPlayersBack(Transform _player1, Transform _player2)
    {
        Rigidbody _rb1 = _player1.GetComponent<Rigidbody>();
        Rigidbody _rb2 = _player2.GetComponent<Rigidbody>();

        if (_rb1 != null && _rb2 != null)
        {
            // Calculate direction from player2 to player1
            Vector3 _pushDirection = (_player1.position - _player2.position).normalized;

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

    #region Pun Methods
    [PunRPC]
    private void RPC_Shoot()
    {
       _particleSystem.Play();

        Ray ray = new Ray(_lasergun.position, _lasergun.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var _enemyPlayerHealth = hit.collider.gameObject;

            if(_enemyPlayerHealth)
            {
                //_enemyPlayerHealth.DoDamage(20);
                ApplyRPSLogic(_enemyPlayerHealth);
            }
        }
    }
    
    #endregion

   
}
