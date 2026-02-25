using UnityEngine;

public class PlayerDashEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystemOnGround;
    [SerializeField] float _animationDuration = 0.5f;
    Coroutine _currentRoutine;
    
    private bool _isGrounded = false;
    
    private void OnEnable()
    {
        PlayerDashScript.IsDashingEvent += PerformStartDashEffect;
        GroundCheck.IsNowGroundedEvent += CheckGrounded;
    }

    private void OnDisable()
    {
        PlayerDashScript.IsDashingEvent -= PerformStartDashEffect;
        GroundCheck.IsNowGroundedEvent -= CheckGrounded;
    }
    
    private void CheckGrounded(bool isGrounded)
    {
        _isGrounded  = isGrounded;
    }

    private void PerformStartDashEffect(bool isDashing)
    {
        if (isDashing)
        {
            if (_isGrounded) _particleSystemOnGround.Play();
        }
        else
        {
            // if I want to do something at the end of the Dash
        }
        
    }
}
