using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActionsActions
{
    public event Action<Vector2> OnMovementEvent;

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
            inputVec.x *= -1;
            OnMovementEvent?.Invoke(inputVec);
        }

        if (context.canceled)
        {
            OnMovementEvent?.Invoke(Vector2.zero);
        }
    }

    private void OnDisable()
    {
        _controls.PlayerActions.Disable();
    }
}
