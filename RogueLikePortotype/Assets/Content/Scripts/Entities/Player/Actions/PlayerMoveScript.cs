using UnityEngine;
using System;

public class PlayerMoveScript : PlayerAction
{
    public Player player;
    
    private Vector2 direction;
    private bool isMoving = false;
    private bool isRunning = false;

    private void OnEnable()
    {
        PlayerControllerScript.OnMoveInput += GetMoveInput;
        PlayerControllerScript.OnRunInput += UpdateIsRunning;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnMoveInput -= GetMoveInput;
        PlayerControllerScript.OnRunInput -= UpdateIsRunning;
    }

    void UpdateIsRunning(bool value)
    {
        isRunning =  value;
    }

    void GetMoveInput(Vector2 inputValue)
    {
        if (inputValue.x != 0)
        {
            isMoving = true;
            direction = new Vector2(inputValue.x, 0).normalized;
        }
        else
        {
            isMoving = false;
        }
    }

    void FixedUpdate()
    {
        if (!player || player.MoveMoveState != PlayerMoveState.Cancelable) 
            return;

        float speedToTarget = isRunning ? player.stats.RunSpeed : player.stats.WalkSpeed;
        float speedToAccel = isRunning ? player.stats.AccelRunSpeed : player.stats.AccelWalkSpeed;
        float speedToDecel = player.stats.DecelWalkSpeed;
        
        float targetSpeed = isMoving 
            ? direction.x * speedToTarget
            : 0f;

        float currentSpeed = player.rb.linearVelocity.x;

        float acceleration = isMoving
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
