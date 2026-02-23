using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] protected Stats stats;
    
    public virtual void TakeDamage(float damage)
    {
        
    }

    public virtual void Heal(float heal)
    {
        
    }

    public virtual void Death()
    {
        
    }
}
