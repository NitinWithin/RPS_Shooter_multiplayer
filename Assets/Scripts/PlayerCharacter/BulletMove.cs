using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviourPunCallbacks
{
    private Player _shooter;

    // Rock = 0; Paper = 1; Scissors = 2
    Dictionary<int, int> counters = new Dictionary<int, int>()
        {
            { 2, 1 },
            { 1, 0 },
            { 0, 2 }
        };

    public void Initialize(Player shooter)
    {
        _shooter = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ApplyRPSLogic(_shooter, other.gameObject);
        }
        Destroy(gameObject);

    }

    public void ApplyRPSLogic(Player shooter, GameObject enemyPlayer)
    {
        GameObject _shooter = GetPlayerGameObject(shooter);
        
        int enemyCharacter = (int)enemyPlayer.GetPhotonView().Owner.CustomProperties["CSN"];
        int playerCharacter = (int)_shooter.GetPhotonView().Owner.CustomProperties["CSN"];

        if (_shooter.GetPhotonView().Owner.GetPhotonTeam().Code == enemyPlayer.GetPhotonView().Owner.GetPhotonTeam().Code) // When in the same team
        {
            Debug.Log("Same team");
            enemyPlayer.GetPhotonView().RPC("DoDamage", RpcTarget.All, 5);
        }
        else if (enemyCharacter == playerCharacter) // Same type
        {
            Debug.Log("Applying RPS logic: PushBack applied");
            enemyPlayer.GetPhotonView().RPC("PushPlayersBack", RpcTarget.All, _shooter, enemyPlayer);
            
        }
        else
        {
            if (counters.ContainsKey(playerCharacter) && counters[playerCharacter] == enemyCharacter) // Nemeis
            {
                Debug.Log("Applying RPS logic: damage taken");
                enemyPlayer.GetPhotonView().RPC("DoDamage", RpcTarget.All, 20);
            }
            else
            {
                Debug.Log("Applying RPS logic: Stun Applied");
                enemyPlayer.GetPhotonView().RPC("StunPlayer", RpcTarget.All, enemyPlayer);
            }
        }

    }

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

}
