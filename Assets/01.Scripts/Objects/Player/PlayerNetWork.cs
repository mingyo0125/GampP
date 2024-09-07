using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetWork : NetworkBehaviour
{
    #region PlayerComponents

    private PlayerMovement _playerMovement;
    private Camera _camera;
    private AudioListener _audioListener;
    private Image _playerPointCursor;

    private bool isOwner;
    private ulong clientId;

    #endregion

    private void Awake()
    {
        Transform camera = transform.Find("Camera");
        _camera = camera.GetComponent<Camera>();
        _audioListener = camera.GetComponent<AudioListener>();

        _playerPointCursor = transform.parent.Find("Canvas/PlayerPointCursor").GetComponent<Image>();
    }

    public override void OnNetworkSpawn()
    {
        _camera.enabled = false;
        _audioListener.enabled = false;
        _playerPointCursor.enabled = false;

        SignalHub.OnCountStartEvent += SetEnableCamera;
        SignalHub.OnGameStartEvent += SetEnableMovement;
    }

    private void FixedUpdate()
    {
        if (!isOwner) { return; }
        _playerMovement?.Move();
    }

    private void SetEnableCamera()
    {
        isOwner = clientId == NetworkManager.Singleton.LocalClientId;

        Debug.Log(isOwner);

        _playerPointCursor.enabled = isOwner;
        _camera.enabled = isOwner;
        _audioListener.enabled = isOwner;
    }

    private void SetEnableMovement()
    {
        _playerMovement = transform.Find("Visual").GetComponent<PlayerMovement>();
    }

    private void OnDisable()
    {
        SignalHub.OnCountStartEvent -= SetEnableCamera;
        SignalHub.OnGameStartEvent -= SetEnableMovement;
    }

    [ClientRpc]
    public void SetClientidClientRpc(ulong clientid)
    {
        clientId = clientid;
    }

    
}
