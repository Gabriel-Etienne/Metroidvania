using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public static Action<bool> ManageParticleEvent;
    [SerializeField] private List<ParticleSystem> particles;
    
    public static Action<bool> ManageFogEvent;
    
    public static Action<Vector2,Vector2> UpdateMinMaxCamPosEvent;
    
    [SerializeField] private List<SpriteRenderer> _spriteFogMaterials;
    private Coroutine _currentFogCoroutine;
    [SerializeField] private float _fogTransitionDuration = 3f;
    
    
    public static Action<Transform> ChangeTransformTargetEvent;
    public static Action<bool> ChangeDeadZoneEvent;
    
    
    private Transform _target = null;
    private Coroutine _currentMoveCoroutine;
    
    private bool _isDeadZoneActive = true;
    [SerializeField] private Vector2 _deadZoneSize = new Vector2(3f, 2f);
    [SerializeField] private bool _drawGizmo = true;
    
    
    [SerializeField] Vector2 _clampCameraPositionX = new Vector2(-50f, 50f);
    [SerializeField] Vector2 _clampCameraPositionY = new Vector2(-50f, 50f);
    
    [SerializeField] float _smoothTime = 0.2f;
    [SerializeField] float _transitionSpeed = 10f;
    [SerializeField] float _tolerance = 0.001f;
    private Vector3 _velocity;

    private void Awake()
    {
        for (int i = 0; i < _spriteFogMaterials.Count; i++)
        {
            _spriteFogMaterials[i].material = new Material(_spriteFogMaterials[i].material);
        }
    }

    private void Start()
    {
        UpdateMinMaxCamPosEvent?.Invoke(_clampCameraPositionX, _clampCameraPositionY);
    }
    

    private void OnEnable()
    {
        ChangeTransformTargetEvent += ChangeTransformTarget;
        ChangeDeadZoneEvent += ChangeDeadZone;
        ManageParticleEvent += ManageParticle;
        ManageFogEvent +=  ManageFog;
        
        UpdateMinMaxCamPosEvent += UpdateMinMaxCamPos;

        PlayerControllerScript.OnDebugAction += ManageFog;
        PlayerControllerScript.OnDebugAction += ManageParticle;
        
    }

    private void OnDisable()
    {
        ChangeTransformTargetEvent -= ChangeTransformTarget;
        ChangeDeadZoneEvent -= ChangeDeadZone;
        ManageParticleEvent -= ManageParticle;
        ManageFogEvent -=  ManageFog; 
        
        UpdateMinMaxCamPosEvent -= UpdateMinMaxCamPos;
        
        PlayerControllerScript.OnDebugAction -= ManageFog;
        PlayerControllerScript.OnDebugAction -= ManageParticle;
    }

    private void UpdateMinMaxCamPos(Vector2 xPos, Vector2 yPos)
    {
        if (_currentMoveCoroutine != null)
            StopCoroutine(_currentMoveCoroutine);
        
        _clampCameraPositionX = xPos;
        _clampCameraPositionY = yPos;
        
        _currentMoveCoroutine = StartCoroutine(TransitionCoroutine());
    }

    private void ChangeDeadZone(bool newIsDeadZoneActive = true)
    {
        _isDeadZoneActive = newIsDeadZoneActive;
    }

    private void ChangeTransformTarget(Transform newTarget)
    {
        Debug.Log($"Change Transform Target Event: {newTarget.name}");
        _target = newTarget;
        
        
        if (_target != null)
        {
            if (_currentMoveCoroutine != null)
                StopCoroutine(_currentMoveCoroutine);
            
            _currentMoveCoroutine = StartCoroutine(FollowTarget());
        }
    }

    private void ApplyMovementToCamera()
    {
        float camSpeed = _smoothTime;
        
        Vector3 camPos = transform.position;
        Vector3 targetPos = _target.position;

        Vector3 desiredPos = camPos;
        Vector3 delta = targetPos - camPos;

        Vector2 deadZoneSize = _isDeadZoneActive ? _deadZoneSize: new Vector2(1f, 1f);
            
        if (delta.x > deadZoneSize.x)
            desiredPos.x += delta.x - deadZoneSize.x;
        else if (delta.x < -deadZoneSize.x)
            desiredPos.x += delta.x + deadZoneSize.x;

        if (delta.y > deadZoneSize.y)
            desiredPos.y += delta.y - deadZoneSize.y;
        else if (delta.y < -deadZoneSize.y)
            desiredPos.y += delta.y + deadZoneSize.y;

        Vector3 newPos = Vector3.SmoothDamp(
            camPos,
            desiredPos,
            ref _velocity,
            camSpeed
        );
        
        // brutal mais bon endroit
        
        if (newPos.x < _clampCameraPositionX.x)
        {
            newPos.x = _clampCameraPositionX.x;
        }
        else if (newPos.x > _clampCameraPositionX.y)
        {
            newPos.x = _clampCameraPositionX.y;
        }
            
        if (newPos.y < _clampCameraPositionY.x)
        {
            newPos.y = _clampCameraPositionY.x;
        }
        else if (newPos.y > _clampCameraPositionY.y)
        {
            newPos.y = _clampCameraPositionY.y;
        }

        transform.position = newPos;
    }
    
    IEnumerator FollowTarget()
    {
        while (_target != null)
        {
            ApplyMovementToCamera();

            yield return null;
        }
    }
    
    IEnumerator TransitionCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = _target.position;
        
        float duration = _transitionSpeed; // temps total
        float elapsed = 0f;

        while (elapsed < duration)
        {
            startPos = transform.position;
            targetPos = _target.position;
            targetPos.z = startPos.z;

            // Clamp propre
            targetPos.x = Mathf.Clamp(targetPos.x, _clampCameraPositionX.x, _clampCameraPositionX.y);
            targetPos.y = Mathf.Clamp(targetPos.y, _clampCameraPositionY.x, _clampCameraPositionY.y);
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            // Easing (accélère puis ralentit)
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            
            // Add a break if the target has reached before the end of the time elapsed  
            if (Vector2.Distance(startPos, targetPos) <= 0.1f)
            {
                Debug.Log("Transition Complete");
                break;
            }

            yield return null;
        }

        transform.position = targetPos;

        _currentMoveCoroutine = StartCoroutine(FollowTarget());
    }
    
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            new Vector3(
                (_clampCameraPositionX.x + _clampCameraPositionX.y) / 2,
                (_clampCameraPositionY.x + _clampCameraPositionY.y) / 2,
                0
                ),
            new Vector3(
                _clampCameraPositionX.x - _clampCameraPositionX.y,
                _clampCameraPositionY.x - _clampCameraPositionY.y, 
                0.25F
                )
            );
        
        
        
        if  (!_drawGizmo || _target == null) return;
        
        Gizmos.color = Color.yellow;
        if (_isDeadZoneActive)
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, _target.position.z), new Vector3(_deadZoneSize.x * 2, _deadZoneSize.y * 2, 0));
        else
            Gizmos.DrawSphere(new Vector3(_target.position.x, _target.position.y, _target.position.z), 0.5f);
        
    }
    
        
    #region  Particle Management

    private void ManageParticle(bool value)
    {
        if (value)
            StartParticles();
        else
            StopParticles();
    }

    private void StartParticles()
    {
        foreach (ParticleSystem p in particles)
        {
            p.Play();
        }
    }

    private void StopParticles()
    {
        foreach (ParticleSystem p in particles)
        {
            p.Stop();
        }
    }
    
    #endregion
    
    #region  Particle Management

    private void ManageFog(bool value)
    {
        Debug.Log($"ManageFog: {value}");
        
        StartFogCoroutine(value);
    }

    private void StartFogCoroutine(bool value)
    {
        if (_currentFogCoroutine != null)
            StopCoroutine(_currentFogCoroutine);

        _currentFogCoroutine = StartCoroutine(ChangeFogCoroutine(value));
    }

    IEnumerator ChangeFogCoroutine(bool value)
    {
        float targetValue = value ? 1f : 0f;

        float startValue = _spriteFogMaterials[0].material.GetFloat("_AlphaValue");

        float elapsed = 0f;

        while (elapsed < _fogTransitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / _fogTransitionDuration;
            float currentValue = Mathf.Lerp(startValue, targetValue, t);

            for (int i = 0; i < _spriteFogMaterials.Count; i++)
            {
                _spriteFogMaterials[i].material.SetFloat("_AlphaValue", currentValue);
            }

            yield return null;
        }

        for (int i = 0; i < _spriteFogMaterials.Count; i++)
        {
            _spriteFogMaterials[i].material.SetFloat("_AlphaValue", targetValue);
        }

        _currentFogCoroutine = null;
    }

    #endregion
    
}
