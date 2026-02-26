using System;
using UnityEngine;

public class PlayerLookAtScript : MonoBehaviour
{
    private Vector2 _center =  Vector2.zero;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private Transform _transformToMove;

    private void OnEnable()
    {
        PlayerControllerScript.OnLookInput += ChangeLookInput;
    }

    private void OnDisable()
    {
        PlayerControllerScript.OnLookInput -= ChangeLookInput;
    }

    private void ChangeLookInput(Vector2 input)
    {
        //Debug.Log($"Looking at {input}");

        Vector2 newPos = input * maxDistance;
        
        if (newPos.magnitude > maxDistance) newPos = input.normalized * maxDistance;
        
        _transformToMove.localPosition = _center + newPos;
        
        if (input == Vector2.zero)
            CameraManager.ChangeDeadZoneEvent?.Invoke(true);
        else
        {
            CameraManager.ChangeDeadZoneEvent?.Invoke(false);
        }
    }
}
