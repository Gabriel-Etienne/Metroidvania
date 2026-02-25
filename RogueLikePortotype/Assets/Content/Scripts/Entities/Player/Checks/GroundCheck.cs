using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _groundCheckPos;
    [SerializeField] Vector2 _groundChecSize;
    
    [Header("Coyote Time")]
    [SerializeField] float _coyoteTimeDuration = 0.1f;
    private float _coyoteTimeCounter;
    
    
    [SerializeField] bool _drawGizmos = true;
    [SerializeField] Color  _groundCheckGizmoColor = Color.red;
    
    private bool isGrounded;
    public bool IsGrounded => isGrounded;
    public bool CanUseCoyote => _coyoteTimeCounter > 0f;
    
    public static Action<bool> IsNowGroundedEvent;

    private void Awake()
    {
        if (_groundCheckPos == null) 
            _groundCheckPos = transform;
    }

    private void Update()
    {
        bool lastCheck = isGrounded;
        isGrounded = Check();
        
        if (lastCheck != isGrounded && isGrounded) // pour faire un effet lors de l'aterrissage du joueur
        {
            IsNowGroundedEvent?.Invoke(true);
        }
        else if (lastCheck != isGrounded && !isGrounded)
        {
            IsNowGroundedEvent?.Invoke(false);
        }

        if (isGrounded)
        {
            _coyoteTimeCounter = _coyoteTimeDuration;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    bool Check()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_groundCheckPos.position, _groundChecSize,0,_groundLayer);
        return collider2Ds.Length > 0;
    }
    
    public void ConsumeCoyote()
    {
        _coyoteTimeCounter = 0f;
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        
        if (_groundCheckPos == null) 
            _groundCheckPos = transform;
        
        Gizmos.color = _groundCheckGizmoColor;
        Gizmos.DrawWireCube(_groundCheckPos.position, _groundChecSize);
    }
}
