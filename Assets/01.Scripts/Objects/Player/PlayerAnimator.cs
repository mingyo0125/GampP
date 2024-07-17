using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private readonly int _isAirboneHash = Animator.StringToHash("is_airbone");
    
    private Animator _animator;
    public Animator Animator => _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetSpeed(int value)
    {
        _animator.SetInteger(_speedHash, value);
    }

    public void SetAirbone(bool value)
    {
        _animator.SetBool(_isAirboneHash, value);
    }

    public void StopAnimator(bool value)
    {
        _animator.speed = value ? 0 : 1;
    }
}
