using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Game Mode", fileName = "gameMode")]
public class GameMode : MonoBehaviour
{
    [SerializeField] private String _name;
    [SerializeField] private byte _maxPlayers;
    [SerializeField] private bool _hasTeams;
    [SerializeField] private int _teamSize;

    public string Name
    {
        get { return _name; }
        private set { _name = value; }
    }

    public byte MaxPlayers
    {
        get { return _maxPlayers; }
        private set { _maxPlayers = value; }
    }

    public bool HasTeams
    {
        get { return _hasTeams; }
        private set { _hasTeams = value; }
    }

    public int TeamSize
    { get { return _teamSize; }
      private set { _teamSize = value; }
    }
}
