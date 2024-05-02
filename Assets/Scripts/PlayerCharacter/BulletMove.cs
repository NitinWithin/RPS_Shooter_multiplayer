
using Photon.Realtime;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private Player _shooter;
    public void Initialize(Player shooter)
    {
        _shooter = shooter;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<FireWeapon>().ApplyRPSLogic(_shooter);
        }
        Destroy(gameObject);

    }
}
