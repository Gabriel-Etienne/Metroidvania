using UnityEngine;
using System;
using System.Collections;

public class PlayerDashScript : PlayerAction
{
    public static Action<bool> IsDashingEvent;
    public static Action ResetDashEvent;
    
    public Player player;
    [SerializeField] float _delayOfDash = 0.15f;
    private bool _delayIsFinished = true;
    Coroutine _currentRoutine;
    
    [SerializeField] float _delayBetweenDash = 0.1f;
    private bool _delayBetweenIsFinished = true;
    Coroutine _currentBetweenRoutine;


    private float _initialGravityScale = 1f;

    private bool _hasDash = true;

    private void Start()
    {
        _initialGravityScale = player.rb.gravityScale;
    }

    private void OnEnable()
    {
        PlayerControllerScript.OnDashInput += GetDashInput;
        GroundCheck.IsNowGroundedEvent += UpdateHasDash;
        ResetDashEvent += ResetDash;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnDashInput -= GetDashInput;
        GroundCheck.IsNowGroundedEvent -= UpdateHasDash;
        ResetDashEvent -= ResetDash;
    }

    void GetDashInput()
    {
        TryToDash();
    }

    void UpdateHasDash(bool hasDash = true)
    {
        if (hasDash) ResetDash();
    }

    void ResetDash()
    {
        _hasDash = true;
    }

    void TryToDash()
    {
        if (!player || player.MoveMoveState == PlayerMoveState.UnCancelable) 
            return;
        
        if (_hasDash && _delayBetweenIsFinished)
        {
            PerformDash();
            player.groundCheckScript.ConsumeCoyote();
            IsDashingEvent?.Invoke(true);
        }
    }
    void PerformDash()
    {
        player.ChangePlayerMoveState(PlayerMoveState.Prioritary, this);
        
        _delayIsFinished = false;
        
        if (!player.groundCheckScript.IsGrounded)
            _hasDash =  false;
        
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);
        if (_currentBetweenRoutine != null)
            StopCoroutine(_currentBetweenRoutine);

        _currentRoutine = StartCoroutine(DashRoutine()); // dash duration
        _currentBetweenRoutine = StartCoroutine(DelayRoutine()); // delay between 2 dash
        
    }

    void EndDashing()
    {
        player.ChangePlayerMoveState(PlayerMoveState.Cancelable);
        player.rb.gravityScale = _initialGravityScale;

        _delayIsFinished = true;
        IsDashingEvent?.Invoke(false);
    }

    IEnumerator DelayRoutine()
    {
        float timer = 0f;

        while (timer < _delayBetweenDash)
        {
            timer += Time.deltaTime;
            
            yield return null;
        }

        _delayBetweenIsFinished = true;
        _currentBetweenRoutine = null;
    }
    
    IEnumerator DashRoutine()
    {
        float timer = 0f;
        
        player.rb.gravityScale = 0;

        float currentXToApply = player.stats.DashSpeed * player.lastVelocityDirection;

        while (timer < _delayOfDash)
        {
            timer += Time.deltaTime;
            
            // do logic
            player.rb.linearVelocity = new Vector2(currentXToApply, 0);

            yield return null;
        }
        
        EndDashing();
        player.rb.linearVelocityX = currentXToApply/3;
        
        _currentRoutine = null;
    }
}
