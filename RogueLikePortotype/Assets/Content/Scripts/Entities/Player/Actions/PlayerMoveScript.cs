using UnityEngine;
using System;

public class PlayerMoveScript : MonoBehaviour
{
    public Player player;
    public Rigidbody2D rb;
    
    private Vector2 direction;
    private bool isMoving = false;

    private void OnEnable()
    {
        PlayerControllerScript.OnMoveInput += GetMoveInput;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnMoveInput -= GetMoveInput;
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
        if (!player || player.moveMoveState != PlayerMoveState.Cancelable) 
            return;

        float targetSpeed = isMoving 
            ? direction.x * player.stats.WalkSpeed 
            : 0f;

        float currentSpeed = rb.linearVelocity.x;

        float acceleration = isMoving 
            ? player.stats.AccelWalkSpeed 
            : player.stats.DecelWalkSpeed;

        float newSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocityX = newSpeed;
    }
}
