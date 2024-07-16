using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActionsActions
{
    public event Action<Vector2> OnMovementEvent;
    public event Action OnTestEvent;

    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.PlayerActions.SetCallbacks(this);
        }
        _controls.PlayerActions.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Vector2 inputVec = context.ReadValue<Vector2>();
            OnMovementEvent?.Invoke(inputVec);
        }
    }

    public void OnTestAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnTestEvent?.Invoke();
        }
    }

    private void OnDisable()
    {
        _controls.PlayerActions.Disable();
    }
}
