using UnityEngine;
using System;
using System.Collections;

public class PlayerJumpScript : PlayerAction
{
    public Player player;
    
    public static Action JumpPerformedEvent;
        
    [SerializeField] float _delayMinBetweenJump = 0.25f;
    private float _timer = 0f;
    private bool _delayIsFinished = true;
    Coroutine _currentRoutine;
    
    private Vector2 direction;

    private void OnEnable()
    {
        PlayerControllerScript.OnJumpInput += GetJumpInput;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnJumpInput -= GetJumpInput;
    }

    void GetJumpInput()
    {
        TryToJump();
    }

    void TryToJump()
    {
        if (!player || player.MoveMoveState != PlayerMoveState.Cancelable) 
            return;
        
        if (_delayIsFinished && (player.groundCheckScript.IsGrounded || player.groundCheckScript.CanUseCoyote))
        {
            PerformJump();
            player.groundCheckScript.ConsumeCoyote();
            
            JumpPerformedEvent?.Invoke();
        }
    }
    void PerformJump()
    {
        _delayIsFinished = false;
        
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
