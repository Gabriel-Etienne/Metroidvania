using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class PlayerControllerScript : MonoBehaviour
{
    public static Action<bool> OnDebugAction;
    private bool debugBool = true;
    
    public static Action<Vector2> OnMoveInput;
    
    public static Action OnJumpInput;
    public static Action OnReleaseJumpInput;
    
    public static Action<bool> OnRunInput;
    public static Action OnDashInput;
    
    public static Action<Vector2> OnLookInput;
    
    public static Action OnInteractInput;
    public static Action OnAttackInput;

    public void OnDebug(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            debugBool = !debugBool;
            OnDebugAction?.Invoke(debugBool);
            Debug.Log($"Debug Action: {debugBool}");
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed && context.ReadValue<Vector2>().magnitude > 0.5f)
        {
            OnLookInput?.Invoke(context.ReadValue<Vector2>());
        }
        else 
        {
            OnLookInput?.Invoke(Vector2.zero);
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed && context.ReadValue<Vector2>().magnitude > 0.2f)
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Attack");
            OnAttackInput?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Interact");
            OnInteractInput?.Invoke();
        }
    }
}
