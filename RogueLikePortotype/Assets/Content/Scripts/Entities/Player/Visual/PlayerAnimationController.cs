using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _moveInputVector;
    private bool _isGrounded = false;
    private bool _hasJumped = false;

    private Vector2 _inputDirection;
    private bool _isDashing = false;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerJumpScript.JumpPerformedEvent += UpdateHasJumped;
        PlayerControllerScript.OnMoveInput += UpdateInputDirection;
        GroundCheck.IsNowGroundedEvent += UpdateIsGrounded;
        PlayerDashScript.IsDashingEvent += UpdateIsDashing;

    }

    private void OnDisable()
    {
        PlayerJumpScript.JumpPerformedEvent -= UpdateHasJumped;
        PlayerControllerScript.OnMoveInput -= UpdateInputDirection;
        GroundCheck.IsNowGroundedEvent -= UpdateIsGrounded;
        PlayerDashScript.IsDashingEvent -= UpdateIsDashing;
    }

    void UpdateInputDirection(Vector2 newInputDirection)
    {
        _inputDirection = newInputDirection;
        // if _inputDirection.y > 0 alors le joueur reguarde en haut 
    }

    void UpdateIsDashing(bool newIsDashing)
    {
        _isDashing = newIsDashing;
        _animator.SetBool("IsDashing", _isDashing);
    }

    private void Update()
    {
        if (_rb.linearVelocity.normalized.x != _moveInputVector.x || _rb.linearVelocity.normalized.y != _moveInputVector.y)
        {
            UpdateMoveInput();
        }
    }

    void UpdateMoveInput()
    {
        _moveInputVector  = _rb.linearVelocity.normalized;
        _animator.SetFloat("X", _moveInputVector.x);
        _animator.SetFloat("Y", _moveInputVector.y);
        
        _animator.SetBool("IsMoving", Mathf.Abs(_rb.linearVelocityX) > 0.2f);
    }
    
    void UpdateIsGrounded(bool value)
    {
        _isGrounded  = value;
        
        if (_isGrounded) _hasJumped = false;
        
        _animator.SetBool("HasJumped",_hasJumped);
        _animator.SetBool("IsGrounded",_isGrounded);
    }
    
    void UpdateHasJumped()
    {
        _hasJumped = true;
        _animator.SetBool("HasJumped",_hasJumped);
    }
    
    
}

 