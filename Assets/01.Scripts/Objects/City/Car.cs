using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Car : MonoBehaviour
{
    Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 moveVec = transform.forward * Time.fixedDeltaTime * 20;

        _rigidbody.Move(moveVec, Quaternion.identity);
    }
}
