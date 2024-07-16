using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private void Awake()
    {
        _inputReader.OnMovementEvent += Movement;
    }

    private void OnDestroy()
    {
        _inputReader.OnMovementEvent -= Movement;
   }

    private void Movement(Vector2 inputPos)
    {
        Debug.Log(inputPos);
    }

}
