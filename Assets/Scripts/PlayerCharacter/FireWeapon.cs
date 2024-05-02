using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] Transform _lasergun;
    [SerializeField] GameObject _particleSystem;
    [SerializeField] private float bulletSpeed = 10f;

    private GameObject _player;

    private bool isStunned = false;
    private float stunDuration = 2f;

    public static Action<int> OnDamageTaken = delegate {  };

    // Rock = 0; Paper = 1; Scissors = 2
    Dictionary<int, int> counters = new Dictionary<int, int>()
        {
            { 2, 1 },
            { 1, 0 },
            { 0, 2 }
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
                //RPC_Shoot();
                photonView.RPC("RPC_Shoot", RpcTarget.AllBuffered, gameObject.GetPhotonView().Owner);
            }
        }
    }
    #endregion

    #region private methods
    private GameObject GetPlayerGameObject(Player player)
    {
        PhotonView[] photonViews = GameObject.FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in photonViews)
        {
            if (view.Owner == player)
            {
                return view.gameObject;
            }
        }
        return null;
    }

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
        Vector3 mouseCursorPosition = Input.mousePosition;
        mouseCursorPosition.z = 15f;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mouseCursorPosition);

        _lasergun.LookAt(targetPosition);
    }

    public void ApplyRPSLogic(Player Shooter)
    {
        GameObject _shooter = GetPlayerGameObject(Shooter);
        
        Player enemyPlayer = gameObject.GetPhotonView().Owner;
        int enemyCharacter = (int)enemyPlayer.CustomProperties["CSN"];
        int playerCharacter = (int)_shooter.GetPhotonView().Owner.CustomProperties["CSN"];

        if (_shooter.GetPhotonView().Owner.GetPhotonTeam().Code == enemyPlayer.GetPhotonTeam().Code) // When in the same team
        {
            Debug.Log("Same team");
            gameObject.GetComponent<Damage>().DoDamage(5);
            OnDamageTaken?.Invoke(5);
        }
        else if(enemyCharacter == playerCharacter) // Same type
        {
            Debug.Log("Applying RPS logic: PushBack applied");
            PushPlayersBack(_player.transform, _shooter.transform);
        }
        else
        {
            if (counters.ContainsKey(playerCharacter) && counters[playerCharacter] == enemyCharacter) // Nemeis
            {
                Debug.Log("Applying RPS logic: damage taken");
                gameObject.GetComponent<Damage>().DoDamage(20);
            }
            else
            {
                Debug.Log("Applying RPS logic: Stun Applied");
                StunPlayer(_shooter);
            }
        }

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
    private void RPC_Shoot(Player _enemyPlayer)
    {
        GameObject Bullet = Instantiate(_particleSystem, _lasergun.position, _lasergun.rotation);
        Bullet.GetComponent<BulletMove>().Initialize(_enemyPlayer);

        Rigidbody bulletRb = Bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = _lasergun.forward * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab does not have a Rigidbody component.");
        }
        Destroy(Bullet, 3);
    }

    #endregion

}
