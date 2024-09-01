using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Car : MonoBehaviour
{
    CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = transform.forward * Time.fixedDeltaTime * 20;

        _characterController.Move(moveVec);
    }
}
