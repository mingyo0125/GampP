using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    Animator _animator;
    CharacterController _characterController;

    private void Awake()
    {
        _animator = transform.Find("Visual").GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _animator.SetInteger("Speed", 10);
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = Vector3.right * Time.fixedDeltaTime * 10;

        _characterController.Move(moveVec);
    }
}
