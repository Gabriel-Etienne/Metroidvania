using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // ground check 
        // walk
        // X et Y
    }

    private void OnDisable()
    {
        
    }
}

 