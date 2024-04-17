using Photon.Pun;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Variables
    [SerializeField] private float _yVelocity = 0f;

    [Range(-5f, -25f)]
    [SerializeField] private float _gravity = -15f;
    [Range(5f, 15f)]
    [SerializeField] private float _movementSpeed = 10f;
    [Range(5f, 15f)]
    [SerializeField] private float _jumpSpeed = 10f;

    [SerializeField] private Transform _cameraTransform;
    private float _pitch = 0f;
    [Range(1f, 60f)]
    [SerializeField] private float _maxPitch = 45f;
    [Range(-1f, -60f)]
    [SerializeField] private float _minPitch = -45f;
    [Range(0.5f, 5f)]
    [SerializeField] private float _mouseSensitivity = 2f;

    [SerializeField] private CharacterController _characterController;

    #endregion

    #region Default methods
    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.IsMine) 
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            PlayerView();
            PlayerMovement();
        }
    }

    #endregion

    #region Private methods
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }


    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }
    private void PlayerMovement()
    {
        Vector3 _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _input = Vector3.ClampMagnitude(_input, 1f);

        Vector3 move = transform.TransformDirection(_input) * _movementSpeed;

        if (_characterController.isGrounded)
        {
            _yVelocity = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                _yVelocity = _jumpSpeed;
            }
        }

        _yVelocity += _gravity * Time.deltaTime; // Apply gravity continuously

        move.y = _yVelocity;
        _characterController.Move(move * Time.deltaTime);
    }

    private void PlayerView()
    {
        float _xInput = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float _yInput = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        transform.Rotate(0, _xInput, 0);

        _pitch -= _yInput;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        Quaternion rotation = Quaternion.Euler(_pitch, 0, 0);
        _cameraTransform.localRotation = rotation;
    }

    #endregion

    #region PUN methods
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_pitch);
        }
        else
        {
            _pitch = (float)stream.ReceiveNext();
            Quaternion rotation = Quaternion.Euler(_pitch, 0 , 0);
            _cameraTransform.localRotation = rotation;
        }
    }
    #endregion
}
