using UnityEngine;


[CreateAssetMenu(fileName = "Stats", menuName = "Game/Stats")]
public class Stats : ScriptableObject
{

    #region Health Stats
    
    [Header("Health related Variables")]
    [SerializeField] private float health;
    public float Health => health;
    [Space(5)]
    [SerializeField] private float baseDamage;
    public float BaseDamage => baseDamage;
    #endregion
    
    #region Movement Stats
    
    [Space(30)]
    [Header("Movements related Variables")]
    [SerializeField] private float walkSpeed;
    public float WalkSpeed => walkSpeed;
    [SerializeField] private float accelWalkSpeed;
    public float AccelWalkSpeed => accelWalkSpeed;
    [SerializeField] private float decelWalkSpeed;
    public float DecelWalkSpeed => decelWalkSpeed;
    
    [Space(5)]
    [SerializeField] private float runSpeed;
    public float RunSpeed => runSpeed;
    [SerializeField] private float accelRunSpeed;
    public float AccelRunSpeed => accelRunSpeed;
    
    
    [Space(5)]
    [SerializeField] private float dashSpeed;
    public float DashSpeed => dashSpeed;
    [Space(5)]
    [SerializeField] private float jumpForce;
    public float JumpForce => jumpForce;
    
    #endregion
    
}
