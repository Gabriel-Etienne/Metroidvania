using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundCheck))]
public class Player : Entity
{
    public PlayerMoveState moveMoveState = PlayerMoveState.Cancelable ;
    public GroundCheck groundCheckScript;

    private void Awake()
    {
        if (groundCheckScript == null)
            groundCheckScript = GetComponent<GroundCheck>();
    }

}

public enum PlayerMoveState
{
    Cancelable,
    UnCancelable,
}