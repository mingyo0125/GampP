using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    Animator _animator;
    CharacterController _characterController;

    private float speed = 10;

    private void Awake()
    {
        _animator = transform.Find("Visual").GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _animator.SetInteger("Speed", (int)speed);
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = Vector3.right * Time.fixedDeltaTime * speed;

        _characterController.Move(moveVec);
    }
}
