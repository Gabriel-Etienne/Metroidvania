using System;
using UnityEngine;

public class WaterTriggerHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _waterMask;
    [SerializeField] private GameObject _splashParticles;
    
    private EdgeCollider2D _edgeColl;
    private InteractableWater _water;

    private void Awake()
    {
        _edgeColl = GetComponent<EdgeCollider2D>();
        _water = GetComponent<InteractableWater>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if our collision is within the waterMask LayerMask
        if ((_waterMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            
            if (rb == null) return;
            
            //spawn particles
            
            
            
            //splash effect
            
            int multiplier = rb.linearVelocityY < 0 ? -1 : 1;

            float vel = rb.linearVelocityY * _water.ForceMultiplier;
            vel = Mathf.Clamp(Mathf.Abs(vel), 0, _water.MaxForce);
            vel *= multiplier;
            
            _water.Splash(collision, vel);
        }
    }
}
