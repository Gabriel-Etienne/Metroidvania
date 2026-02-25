using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public static Action<bool> ManageParticleEvent;
    [SerializeField] private List<ParticleSystem> particles;

    
    public static Action<bool> ManageFogEvent;
    [SerializeField] private List<SpriteRenderer> _spriteFogMaterials;
    private Coroutine _currentFogCoroutine;
    [SerializeField] private float _fogTransitionDuration = 3f;
    
    
    public static Action<Transform> ChangeTransformTargetEvent;
    public static Action<bool> ChangeDeadZoneEvent;
    
    
    private Transform _target = null;
    private Coroutine _followTargetCoroutine;
    
    private bool _isDeadZoneActive = true;
    [SerializeField] private Vector2 _deadZoneSize = new Vector2(3f, 2f);
    [SerializeField] private float _cameraSpeed = 5f;
    [SerializeField] private bool _drawGizmo = true;
    
    [SerializeField] float smoothTime = 0.2f;
    private Vector3 velocity;

    private void Awake()
    {
        for (int i = 0; i < _spriteFogMaterials.Count; i++)
        {
            _spriteFogMaterials[i].material = new Material(_spriteFogMaterials[i].material);
        }
    }
    

    private void OnEnable()
    {
        ChangeTransformTargetEvent += ChangeTransformTarget;
        ChangeDeadZoneEvent += ChangeDeadZone;
        ManageParticleEvent += ManageParticle;
        ManageFogEvent +=  ManageFog;

        PlayerControllerScript.OnDebugAction += ManageFog;
    }

    private void OnDisable()
    {
        ChangeTransformTargetEvent -= ChangeTransformTarget;
        ChangeDeadZoneEvent -= ChangeDeadZone;
        ManageParticleEvent -= ManageParticle;
        ManageFogEvent -=  ManageFog; 
        
        PlayerControllerScript.OnDebugAction -= ManageFog;
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
            if (_followTargetCoroutine != null)
                StopCoroutine(_followTargetCoroutine);
            
            _followTargetCoroutine = StartCoroutine(FollowTarget());
        }
    }

    IEnumerator FollowTarget()
    {
        
        while (_target != null)
        {
            
            Vector3 camPos = transform.position;
            Vector3 targetPos = _target.position;

            Vector3 desiredPos = camPos;
            Vector3 delta = targetPos - camPos;

            Vector2 deadZoneSize = _isDeadZoneActive ? _deadZoneSize: new Vector2(0.1f, 0.1f);
            
            if (delta.x > deadZoneSize.x)
                desiredPos.x += delta.x - deadZoneSize.x;
            else if (delta.x < -deadZoneSize.x)
                desiredPos.x += delta.x + deadZoneSize.x;

            if (delta.y > deadZoneSize.y)
                desiredPos.y += delta.y - deadZoneSize.y;
            else if (delta.y < -deadZoneSize.y)
                desiredPos.y += delta.y + deadZoneSize.y;
            
            
            desiredPos.z = camPos.z;

            Vector3 newPos = Vector3.SmoothDamp(
                camPos,
                desiredPos,
                ref velocity,
                smoothTime
            );
            
            transform.position =  newPos;

            yield return null;
        }
    }
    
    private void OnDrawGizmos()
    {
        if  (!_drawGizmo || _target == null) return;
        
        Gizmos.color = Color.yellow;
        if (_isDeadZoneActive)
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, _target.position.z), new Vector3(_deadZoneSize.x * 2, _deadZoneSize.y * 2, 0));
        else
            Gizmos.DrawSphere(new Vector3(_target.position.x, _target.position.y, _target.position.z), 0.25f);
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
