using System;
using UnityEngine;

public class PlayerHardFall : MonoBehaviour
{
    private float _originalGravity = 1f;
    [SerializeField] private float _fastGravity = 2f;
    [SerializeField] private Rigidbody2D _rb;
    
    // si le joueur est en train de tombé il faut boost la gravité

    private void Awake()
    {
        _originalGravity = _rb.gravityScale;
    }

    private void OnEnable()
    {
        PlayerControllerScript.OnMoveInput += GetMoveInput;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnMoveInput -= GetMoveInput;
    }

    private void GetMoveInput(Vector2 input)
    {
        
    }

    private void FixedUpdate()
    {
        if (_rb.linearVelocityY < -0.2f)
        {
            _rb.gravityScale = _fastGravity;
        }
        else
        {
            _rb.gravityScale =  _originalGravity;
        }
    }

    // peut etre ajouter un fast fall en cas d'input vers le bas
}
