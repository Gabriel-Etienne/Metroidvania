using UnityEngine;

public class Player : Entity
{
    public PlayerMoveState moveMoveState = PlayerMoveState.Cancelable ;
    
}

public enum PlayerMoveState
{
    Cancelable,
    UnCancelable,
}