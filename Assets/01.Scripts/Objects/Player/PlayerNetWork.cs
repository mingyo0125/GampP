using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetWork : NetworkBehaviour
{
    #region PlayerComponents

    private PlayerMovement _playerMovement;

    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
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
