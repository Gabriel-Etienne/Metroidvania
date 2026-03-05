using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackScript : PlayerAction
{
    public Player player;
    
    private bool _isInputRecentlyPressed = false;
    
    [SerializeField] private float _inputMaxDelay = 0.5f;
    [SerializeField] private float _delayBetweenAttacks = 0.75f;
    
    private Coroutine _coroutine;

    private int _comboCount = 0;
    [SerializeField] private float _radius = 0.5f;
    
    private Vector2 _actualMoveInput = Vector2.right;
    private Vector2 _lastDirection = Vector2.right;
    private Vector3 _lastAttack = Vector3.zero;
    
    [SerializeField] private bool _drawGizmo = true;

    private bool _finishedAttack = true;
    private bool _drawLastAttack = false;
    
    [SerializeField] private LayerMask _layerMask;
    
    
    private void OnEnable()
    {
        PlayerControllerScript.OnAttackInput += GetAttackInput;
        PlayerControllerScript.OnMoveInput += GetMoveInput;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnAttackInput -= GetAttackInput;
        PlayerControllerScript.OnMoveInput -= GetMoveInput;
    }


    void GetAttackInput()
    {
        // commence la coroutine 
        if (_coroutine !=  null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CheckIfCanAttack());

    }

    void GetMoveInput(Vector2 input)
    {
        _actualMoveInput = input;
    }

    void Attack()
    {
        Debug.Log("Attack");
        
        _finishedAttack  = false;
        Vector2 dir = _actualMoveInput;

        if (player.groundCheckScript.IsGrounded)
        {
            // 3 possibilité : haut , gauche , droit
            
            if (dir.y > Mathf.Abs(dir.x))
            {
                // attaque up
                dir =  Vector2.up;
            }
            else
            {
                // attaque dans la direction x
                dir = new Vector2(player.lastVelocityDirection, 0);
            }
        }
        else
        {
            // 4 possibilité : bas , haut , gauche , droit
            
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                // attaque 
                float y = dir.y >= 0 ? 1 : -1;
                dir = new Vector2(0, y);
            }
            else
            {
                // attaque dans la direction x
                dir = new Vector2(player.lastVelocityDirection, 0);
            }
        }
        
        _lastDirection = dir;
        _lastAttack = player.transform.position + (Vector3)_lastDirection;

        _drawLastAttack = true;
        
        Collider2D[] _coll = Physics2D.OverlapCircleAll(_lastAttack, _radius, _layerMask);
        
        foreach (Collider2D coll in _coll)
        {
            Debug.Log(coll.name);
        }

        StartCoroutine(DelayBetweenAttacks());
        
        // au contact d'un ennemie alors que l'on frappe en bas
        if (_coll.Length > 0 && _lastDirection.y < 0)
        {
            PlayerJumpScript.PerformJumpEvent?.Invoke();
            PlayerDashScript.ResetDashEvent?.Invoke();
        }

    }

    IEnumerator DelayBetweenAttacks()
    {
        float timer = 0f;
        
        while (timer < _delayBetweenAttacks)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        _finishedAttack = true;
        _drawLastAttack =  false;
    }

    IEnumerator CheckIfCanAttack()
    {
        // check for the time duration and check if the actual action is cancelable
        float timer = 0f;
        
        while (timer < _inputMaxDelay)
        {
            timer += Time.deltaTime;

            if (player.MoveState != PlayerMoveState.Cancelable || !_finishedAttack)
            {
                yield return null;
                continue;
            }
            
            Attack();
            break;
        }
        
        _coroutine = null;
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmo || !_drawLastAttack) 
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_lastAttack, _radius);
    }

}
