using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FireWeapon : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] Transform _lasergun;
    [SerializeField] GameObject _particleSystem;
    [SerializeField] private float bulletSpeed = 10f;

    private GameObject _player;
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
                RPC_Shoot(photonView.Owner);
                //photonView.RPC("RPC_Shoot", RpcTarget.AllBuffered, gameObject.GetPhotonView().Owner);
            }
        }
    }
    #endregion

    #region private methods

    private void WeaponAim()
    {
        Vector3 mouseCursorPosition = Input.mousePosition;
        mouseCursorPosition.z = 15f;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mouseCursorPosition);

        _lasergun.LookAt(targetPosition);
    }

    #endregion

    #region Pun Methods
    private void RPC_Shoot(Player _shooter)
    {
        GameObject Bullet = PhotonNetwork.Instantiate(_particleSystem.name, _lasergun.position, _lasergun.rotation);
        Bullet.GetComponent<BulletMove>().Initialize(_shooter);

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
