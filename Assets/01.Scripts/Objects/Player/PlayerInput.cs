using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _inputReader.OnMovementEvent += Movement;

        _playerMovement = transform.Find("Visual").GetComponent<PlayerMovement>();
    }

    private void OnDestroy()
    {
        _inputReader.OnMovementEvent -= Movement;
   }

    private void Movement(Vector2 inputPos)
    {
        Vector3 targetVec = new Vector3(inputPos.x, 0, inputPos.y);
        _playerMovement.SetMovementVelocity(targetVec);
    }

}
