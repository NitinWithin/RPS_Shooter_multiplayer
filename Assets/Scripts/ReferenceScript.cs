using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceScript : MonoBehaviour
{

    [SerializeField] GameObject player;
    PlayerTeamAndCharacterChoice buttonScript;
    // Start is called before the first frame update
    void Start()
    {
        buttonScript = player.GetComponent<PlayerTeamAndCharacterChoice>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
