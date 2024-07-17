using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float gravity = -9.8f, speed = 5f, rotationSpeed;

    private CharacterController _charController;
    private PlayerAnimator _animator;

    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float verticalVelocity;
    private Vector3 _inputVelocity;

    //private AgentController _controller;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _animator = transform.Find("Visual").GetComponent<PlayerAnimator>();
        //_controller = GetComponent<AgentController>();
    }

    public void SetMovementVelocity(Vector3 value)
    {
        if (value == Vector3.zero) { StopImmediately(); }
        _inputVelocity = value;
        //_movementVelocity = value;
    }

    private void CalculatePlayerMovement()
    {
        _inputVelocity.Normalize();

        _movementVelocity = _inputVelocity;

        _animator?.SetSpeed(Mathf.CeilToInt(_movementVelocity.sqrMagnitude)); //이동속도 반영

        _movementVelocity *= speed * Time.fixedDeltaTime;
    }

    public void SetRotation()
    {
        if (_inputVelocity.sqrMagnitude > 0.01f)
        {
            Vector3 dir = _inputVelocity;
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  targetRotation,
                                                  rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
        _animator?.SetSpeed(0); //이동속도 반영
    }

    public void Move() // FixedUpdate에서 해줄 것
    {
        CalculatePlayerMovement();
        SetRotation();

        if (_charController.isGrounded == false)
        {
            verticalVelocity = gravity * Time.fixedDeltaTime;
        }
        else
        {
            verticalVelocity = gravity * 0.3f * Time.fixedDeltaTime;
        }

        Vector3 move = _movementVelocity + verticalVelocity * Vector3.up;
        _charController.Move(move);
        
        //_agentAnimator?.SetAirbone(!_charController.isGrounded);
    }

}
