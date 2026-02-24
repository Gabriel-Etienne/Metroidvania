using System;
using System.Collections;
using UnityEngine;

public class PlayerJumpEffect : MonoBehaviour
{
    private float _initialScale = 1f;
    [SerializeField] AnimationCurve _animationCurveX;
    [SerializeField] AnimationCurve _animationCurveY;
    
    [SerializeField] ParticleSystem _particleSystem;
    
    [SerializeField] float _animationDuration = 0.5f;
    private float _timer = 0f;
    Coroutine _currentRoutine;

    private void Awake()
    {
        _initialScale =  transform.localScale.x;
    }

    private void OnEnable()
    {
        PlayerJumpScript.JumpPerformedEvent += PerformEffect;
    }

    private void OnDisable()
    {
        PlayerJumpScript.JumpPerformedEvent -= PerformEffect;
    }

    public void PerformEffect()
    {
        _particleSystem.Play();

        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(ScaleRoutine());
    }

    IEnumerator ScaleRoutine()
    {
        float timer = 0f;

        while (timer < _animationDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.InverseLerp(0, _animationDuration, timer);

            transform.localScale = new Vector3(
                _animationCurveX.Evaluate(t),
                _animationCurveY.Evaluate(t),
                transform.localScale.z
            );

            yield return null;
        }

        // Assure qu’on finit à la valeur finale exacte
        transform.localScale = new Vector3(
            _animationCurveX.Evaluate(1f),
            _animationCurveY.Evaluate(1f),
            transform.localScale.z
        );

        _currentRoutine = null;
    }

}
