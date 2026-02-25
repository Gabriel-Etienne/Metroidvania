using System;
using UnityEngine;

public class PlayerChangeFacingDirection : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer =  GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"No sprite renderer attached on the same gameobject as this script : {name}");
        }
    }

    private void Update()
    {
        ChangeFacingDirection();
    }

    void ChangeFacingDirection()
    {
        if (_rb.linearVelocityX > 0.2f)
        { 
            _spriteRenderer.flipX = false;
        }
        else if (_rb.linearVelocityX < -0.2f)
        {
            _spriteRenderer.flipX = true;
        }
            
    }
}
