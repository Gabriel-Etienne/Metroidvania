using System;
using UnityEngine;

public class PlayerLookAtScript : MonoBehaviour
{
    private Vector2 _center =  Vector2.zero;
    private Vector2 _defaultCenter = Vector2.zero;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private Transform _transformToMove;
    [SerializeField] private Rigidbody2D _rb;
    
    private bool _isLooking = false;
    private void OnEnable()
    {
        PlayerControllerScript.OnLookInput += ChangeLookInput;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnLookInput -= ChangeLookInput;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_rb.linearVelocityX) > 0.75f)
        {
            int dir = _rb.linearVelocityX > 0 ? 1 : -1;
            Vector2 newCenter = _defaultCenter * dir;

            if (newCenter != _center)
            {
                _center = _defaultCenter * dir;
                if (!_isLooking)
                    _transformToMove.localPosition =  _center;
            }
                
        }
    }

    private void Awake()
    {
        _center =  _transformToMove.localPosition;
        _defaultCenter = _center;
    }

    private void ChangeLookInput(Vector2 input)
    {
        //Debug.Log($"Looking at {input}");

        Vector2 newPos = input * maxDistance;
        
        if (newPos.magnitude > maxDistance) newPos = input.normalized * maxDistance;
        
        _transformToMove.localPosition = _center + newPos;

        if (input == Vector2.zero)
        {
            CameraManager.ChangeDeadZoneEvent?.Invoke(true);
            _isLooking = false;
        }
        else
        {
            CameraManager.ChangeDeadZoneEvent?.Invoke(false);
            _isLooking = true;
        }
    }
}
