using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class PlayerControllerScript : MonoBehaviour
{
    public static Action<Vector2> OnMoveInput;
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed && context.ReadValue<Vector2>().magnitude > 0.2)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            OnMoveInput?.Invoke(Vector2.zero);
        }
    }
}
