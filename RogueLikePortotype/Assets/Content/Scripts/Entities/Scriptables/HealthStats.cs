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
    [Space(5)]
    [SerializeField] private float runSpeed;
    public float RunSpeed => runSpeed;
    [Space(5)]
    [SerializeField] private float dashSpeed;
    public float DashSpeed => dashSpeed;
    [Space(5)]
    [SerializeField] private float jumpForce;
    public float JumpForce => jumpForce;
    
    #endregion
    
}
