using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundCheck))]
public class Player : Entity
{
    private PlayerMoveState moveMoveState = PlayerMoveState.Cancelable;
    public PlayerMoveState MoveMoveState => moveMoveState;
    
    private PlayerAction actualPriority = null;
    public PlayerAction ActualPriority => actualPriority;
    
    public GroundCheck groundCheckScript;
    public Rigidbody2D rb;
    public int lastVelocityDirection = 1;
    
    

    private void Awake()
    {
        if (groundCheckScript == null)
            groundCheckScript = GetComponent<GroundCheck>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.linearVelocityX > 0.1f) lastVelocityDirection = 1;
        else if (rb.linearVelocityX < -0.1f) lastVelocityDirection = -1;
    }

    public void ChangePlayerMoveState(PlayerMoveState newState, PlayerAction actionScript = null)
    {
        moveMoveState = newState;
        actualPriority =  actionScript;
    }

}

public enum PlayerMoveState
{
    Cancelable,
    Prioritary,
    UnCancelable,
}