using UnityEngine;
using System;

public class PlayerMoveScript : PlayerAction
{
    public Player player;
    
    private Vector2 _direction;
    private bool _isMoving = false;
    private bool _isRunning = false;
    private bool _isGrounded = false;

    private void OnEnable()
    {
        PlayerControllerScript.OnMoveInput += GetMoveInput;
        PlayerControllerScript.OnRunInput += UpdateIsRunning;
        GroundCheck.IsNowGroundedEvent += GetIsGrounded;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnMoveInput -= GetMoveInput;
        PlayerControllerScript.OnRunInput -= UpdateIsRunning;
        GroundCheck.IsNowGroundedEvent -= GetIsGrounded;
    }

    void UpdateIsRunning(bool value)
    {
        _isRunning =  value;
    }

    void GetIsGrounded(bool value)
    {
        _isGrounded = value;
    }

    void GetMoveInput(Vector2 inputValue)
    {
        if (Mathf.Abs(inputValue.x) > 0.2f)
        {
            _isMoving = true;
            _direction = new Vector2(inputValue.x, 0).normalized;
        }
        else
        {
            _isMoving = false;
        }
    }

    void FixedUpdate()
    {
        if (!player || player.MoveState != PlayerMoveState.Cancelable) 
            return;
        
        //Debug.Log("UPDATE");

        float speedToTarget = _isRunning && _isGrounded ? player.stats.RunSpeed : player.stats.WalkSpeed;
        float speedToAccel = _isRunning && _isGrounded ? player.stats.AccelRunSpeed : player.stats.AccelWalkSpeed;
        float speedToDecel = player.stats.DecelWalkSpeed;
        
        float targetSpeed = _isMoving 
            ? _direction.x * speedToTarget
            : 0f;

        float currentSpeed = player.rb.linearVelocity.x;

        float acceleration = _isMoving
            ? speedToAccel
            : speedToDecel;

        float newSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.fixedDeltaTime
        );

        player.rb.linearVelocityX = newSpeed;
    }
}
