using UnityEngine;
using System;
using System.Collections;

public class PlayerJumpScript : PlayerAction
{
    public Player player;
    
    public static Action JumpPerformedEvent;
    public static Action PerformJumpEvent;
        
    [SerializeField] float _delayMinBetweenJump = 0.25f;
    private float _timer = 0f;
    private bool _delayIsFinished = true;
    Coroutine _currentRoutine;
    
    private Vector2 direction;
    private bool _wasNormalJump = true; 

    private void OnEnable()
    {
        PlayerControllerScript.OnJumpInput += GetJumpInput;
        PlayerControllerScript.OnReleaseJumpInput += CheckJumpState;
        PerformJumpEvent += MakePlayerJump;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnJumpInput -= GetJumpInput;
        PlayerControllerScript.OnReleaseJumpInput -= CheckJumpState;
        PerformJumpEvent -= MakePlayerJump;
    }

    void CheckJumpState()
    {
        if (!player || player.MoveState != PlayerMoveState.Cancelable || !_wasNormalJump) 
            return;
        
        if (player.rb.linearVelocityY > 1f) // est en train de monter
            player.rb.linearVelocityY /= 2;
    }

    void MakePlayerJump()
    {
        _wasNormalJump = false;
        PerformJump();
    }
    
    void GetJumpInput()
    {
        TryToJump();
    }

    void TryToJump()
    {
        if (!player || player.MoveState != PlayerMoveState.Cancelable) 
            return;
        
        if (_delayIsFinished && (player.groundCheckScript.IsGrounded || player.groundCheckScript.CanUseCoyote))
        {
            _wasNormalJump = true;
            PerformJump();
            
            JumpPerformedEvent?.Invoke();
        }
    }
    void PerformJump()
    {
        _delayIsFinished = false;
        player.groundCheckScript.ConsumeCoyote();
        
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(DelayRoutine());
        
        //Debug.Log("PerformJump");
        player.rb.linearVelocityY = 0;
        player.rb.AddForce(new Vector2(0,1) * player.stats.JumpForce, ForceMode2D.Impulse);
    }
    
    IEnumerator DelayRoutine()
    {
        float timer = 0f;

        while (timer < _delayMinBetweenJump)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        _delayIsFinished = true;
        _currentRoutine = null;
    }
    
}
