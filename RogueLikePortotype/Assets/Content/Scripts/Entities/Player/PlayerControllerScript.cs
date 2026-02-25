using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class PlayerControllerScript : MonoBehaviour
{
    public static Action<Vector2> OnMoveInput;
    
    public static Action OnJumpInput;
    public static Action OnReleaseJumpInput;
    
    public static Action<bool> OnRunInput;
    public static Action OnDashInput;
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
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpInput?.Invoke();
        }
        else if (context.canceled)
        {
            OnReleaseJumpInput?.Invoke();
        }
    }
    
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnDashInput?.Invoke();
        }
    }
    
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnRunInput?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnRunInput?.Invoke(false);
        }
    }
}
