using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetWork : NetworkBehaviour
{
    #region PlayerComponents

    private PlayerMovement _playerMovement;
    private CinemachineVirtualCamera _virtualCamera;
    private AudioListener _audioListener;

    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _virtualCamera = transform.Find("PlayerFollowCam").GetComponent<CinemachineVirtualCamera>();
        _audioListener = transform.Find("Camera").GetComponent<AudioListener>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            _virtualCamera.Priority = 1;
            _audioListener.enabled = true;
        }
        else
        {
            _virtualCamera.Priority = 0;
            _audioListener.enabled = false;
        }
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
