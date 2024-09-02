using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetWork : NetworkBehaviour
{
    #region PlayerComponents

    private PlayerMovement _playerMovement;
    private Camera _camera;
    private AudioListener _audioListener;

    #endregion

    private void Awake()
    {
        _playerMovement = transform.Find("Visual").GetComponent<PlayerMovement>();

        Transform camera = transform.Find("Camera");
        _camera = camera.GetComponent<Camera>();
        _audioListener = camera.GetComponent<AudioListener>();
    }

    public override void OnNetworkSpawn()
    {
        _camera.enabled = false;
        _audioListener.enabled = false;
    }

    private void Update()
    {
        if (!IsOwner) { return; }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) { return; }
        _playerMovement.Move();
    }
}
