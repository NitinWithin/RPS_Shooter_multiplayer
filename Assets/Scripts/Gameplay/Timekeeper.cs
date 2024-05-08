using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

public class Timekeeper : MonoBehaviour
{
    [SerializeField] private PhotonView _timeKeeperPhotonView;
    private TMP_Text _timeText;
    [SerializeField] private float _gameTimeInSeconds = 180f; // 5 minutes

    private float elapsedTime = 0f;
    private bool isGameRunning = true;

    public static Action OnRoundTimeEnded = delegate { };
    private void Start()
    {
        _timeText = GameObject.FindGameObjectWithTag("Timer").GetComponent<TMP_Text>();
    }

    void Update()
    {
        if(_timeText == null)
        {
            _timeText = GameObject.FindGameObjectWithTag("Timer").GetComponent<TMP_Text>();
        }
        TimeTracker();
    }

    [PunRPC]
    private void TimerTextUpdate(int minutes, int seconds)
    {
        if (_timeText == null)
        {
            Debug.Log("TimeText is null");
            return;
        }
        // Update UI text
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimeTracker() 
    {
        if (isGameRunning && PhotonNetwork.IsMasterClient)
        {
            elapsedTime += Time.deltaTime;

            // Calculate remaining time
            float remainingTime = Mathf.Max(0f, _gameTimeInSeconds - elapsedTime);

            // Format remaining time as minutes and seconds
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            _timeKeeperPhotonView.RPC("TimerTextUpdate", RpcTarget.All, minutes, seconds);

            // Check if time is up
            if (remainingTime <= 0f)
            {
                isGameRunning = false;
                // Perform game over actions here
                Debug.Log("Round Over");
                OnRoundTimeEnded?.Invoke();
                
            }
        }
    }
}
