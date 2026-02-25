using System;
using System.Collections;
using UnityEngine;

public class PlayerJumpEffect : MonoBehaviour
{
    private Vector3 _initialScale = new Vector3(1,1,1);
    [SerializeField] AnimationCurve _animationCurveX;
    [SerializeField] AnimationCurve _animationCurveY;
    
    
    [SerializeField] AnimationCurve _animationCurveGroundedX;
    [SerializeField] AnimationCurve _animationCurveGroundedY;
    
    [SerializeField] ParticleSystem _particleSystem;
    
    [SerializeField] float _animationDuration = 0.5f;
    private float _timer = 0f;
    Coroutine _currentRoutine;

    private void Awake()
    {
        _initialScale =  transform.localScale;
    }

    private void OnEnable()
    {
        PlayerJumpScript.JumpPerformedEvent += PerformStartJumpEffect;
        GroundCheck.IsNowGroundedEvent += CheckGrounded;
    }

    private void OnDisable()
    {
        PlayerJumpScript.JumpPerformedEvent -= PerformStartJumpEffect;
        GroundCheck.IsNowGroundedEvent -= CheckGrounded;
    }

    private void CheckGrounded(bool isGrounded)
    {
        if (!isGrounded) return;
        
        _particleSystem.Play();
        
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);
        
        _currentRoutine = StartCoroutine(ScaleRoutine(_animationCurveGroundedX, _animationCurveGroundedY));
    }

    public void PerformStartJumpEffect()
    {
        _particleSystem.Play();

        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);
        
        _currentRoutine = StartCoroutine(ScaleRoutine(_animationCurveX, _animationCurveY));

    }

    IEnumerator ScaleRoutine(AnimationCurve curveX, AnimationCurve curveY)
    {
        float timer = 0f;
                
        transform.localScale = _initialScale;

        while (timer < _animationDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.InverseLerp(0, _animationDuration, timer);

            transform.localScale = new Vector3(
                curveX.Evaluate(t),
                curveY.Evaluate(t),
                transform.localScale.z
            );

            yield return null;
        }

        // Assure qu’on finit à la valeur finale exacte
        transform.localScale = new Vector3(
            curveX.Evaluate(1f),
            curveY.Evaluate(1f),
            transform.localScale.z
        );

        _currentRoutine = null;
    }

}
