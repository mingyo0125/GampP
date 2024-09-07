using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f, rotationSpeed;

    private Rigidbody _rigidbody;
    private PlayerAnimator _animator;
    private SpawnPointReader _spawnPointReader;

    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float verticalVelocity;
    private Vector3 _inputVelocity;

    private bool isActive = true;


    private void Awake()
    {
        _rigidbody = transform.root.GetComponent<Rigidbody>();
        _animator = GetComponent<PlayerAnimator>();

        _rigidbody.useGravity = false;
        _spawnPointReader = transform.root.Find("SpawnPointReader").GetComponent<SpawnPointReader>();
    }

    public void SetMovementVelocity(Vector3 value)
    {
        if (value == Vector3.zero) { StopImmediately(); }
        _inputVelocity = value;
    }

    private void CalculatePlayerMovement()
    {
        _inputVelocity.Normalize();

        _movementVelocity = _inputVelocity;

        _animator?.SetSpeed(Mathf.CeilToInt(_movementVelocity.sqrMagnitude)); // 이동 속도 반영

        _movementVelocity *= speed;
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
        _animator?.SetSpeed(0); // 이동 속도 반영
        _rigidbody.velocity = Vector3.zero;
    }

    public void Move() // FixedUpdate에서 호출할 것
    {
        if (!isActive) { return; }

        CalculatePlayerMovement();
        SetRotation();

        Vector3 moveVec = _movementVelocity;
        moveVec.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveVec;

        //_agentAnimator?.SetAirbone(!_charController.isGrounded);
    }

    private void OnTriggerEnter(Collider other) // 시간 없어서 일단 걍 여기다가
    {
        if (other.gameObject.CompareTag("Car"))
        {
            isActive = false;
            transform.DOScaleY(5, 0.3f);
            StopImmediately();

            CoroutineUtil.CallWaitForSeconds(1f, () =>
            {
                SignalHub.OnPlayerDieEvent?.Invoke(transform, _spawnPointReader.RecentSpawnPoint);
                isActive = true;
            });
        }
    }
}
