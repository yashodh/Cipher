using UnityEngine;

/// <summary>
/// Main enemy controller with state machine
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyStateMachine StateMachine { get; private set; }
    
    // State instances
    public EnemyState_Idle IdleState { get; private set; }
    public EnemyState_Alert AlertState { get; private set; }
    public EnemyState_Hide HideState { get; private set; }
    public EnemyState_Pursue PursueState { get; private set; }
    public EnemyState_Attack AttackState { get; private set; }
    public EnemyState_Dead DeadState { get; private set; }
    
    // Enemy properties (add as needed)
    [Header("Enemy Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    
    void Awake()
    {
        Debug.Log("Awake - Enemy");
        
        // Initialize state machine
        StateMachine = new EnemyStateMachine();
        
        // Create state instances
        IdleState = new EnemyState_Idle(StateMachine, this);
        AlertState = new EnemyState_Alert(StateMachine, this);
        HideState = new EnemyState_Hide(StateMachine, this);
        PursueState = new EnemyState_Pursue(StateMachine, this);
        AttackState = new EnemyState_Attack(StateMachine, this);
        DeadState = new EnemyState_Dead(StateMachine, this);
    }
    
    void Start()
    {
        Debug.Log("Start - Enemy");
        
        // Set initial state to Idle
        StateMachine.Initialize(IdleState);
    }

    void Update()
    {
        StateMachine.Update();
    }
    
    void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
    
    // Example helper methods (implement based on your game logic)
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            StateMachine.ChangeState(DeadState);
        }
    }
    
    public bool HealthLow()
    {
        return health < 30f;
    }
    
    // Add more helper methods as needed:
    // public bool DetectPlayer() { }
    // public bool CanSeePlayer() { }
    // public bool IsPlayerInAttackRange() { }
    // public void PerformAttack() { }
    // public void MoveTowardsPlayer() { }
    // etc.
    
    private void OnDestroy()
    {
        
    }
}
