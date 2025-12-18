using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Main enemy controller with state machine
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyStateMachine StateMachine { get; private set; }
    
    // State instances
    public EnemyState_Patrol PatrolState { get; private set; }
    public EnemyState_Alert AlertState { get; private set; }
    public EnemyState_Pursue PursueState { get; private set; }
    public EnemyState_Dead DeadState { get; private set; }
    
    // Enemy properties (add as needed)
    [Header("Enemy Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float attackRange = 2f;
    
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float pursueSpeed = 4f;
    
    [Header("Detection Settings per State")]
    [Tooltip("Detection range for Idle/Patrol states")]
    [SerializeField] private float normalDetectionRange = 10f;
    [SerializeField] private float normalFieldOfView = 90f;
    
    [Tooltip("Detection range for Alert state")]
    [SerializeField] private float alertDetectionRange = 8f;
    [SerializeField] private float alertFieldOfView = 120f;
    
    [Tooltip("Detection range for Pursue state")]
    [SerializeField] private float pursueDetectionRange = 15f;
    [SerializeField] private float pursueFieldOfView = 150f;
    
    [Header("Components")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    
    // Patrol path (set by spawner at runtime)
    private PatrolPath patrolPath;
    
    // Detection component (auto-found)
    private EnemyDetection detection;
    private EnemyAlertMeter alertMeter;
    
    public float PatrolSpeed => patrolSpeed;
    public float PursueSpeed => pursueSpeed;
    public PatrolPath PatrolPath => patrolPath;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public EnemyDetection Detection => detection;
    public EnemyAlertMeter AlertMeter => alertMeter;
    
    // Detection parameters
    public float NormalDetectionRange => normalDetectionRange;
    public float NormalFieldOfView => normalFieldOfView;
    public float AlertDetectionRange => alertDetectionRange;
    public float AlertFieldOfView => alertFieldOfView;
    public float PursueDetectionRange => pursueDetectionRange;
    public float PursueFieldOfView => pursueFieldOfView;
    
    void Awake()
    {
        Debug.Log("Awake - Enemy");
        
        // Get or add NavMeshAgent
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
                Debug.Log("[Enemy] Added NavMeshAgent component");
            }
        }
        
        // Configure NavMeshAgent
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.angularSpeed = 120f;
        navMeshAgent.acceleration = 8f;
        
        // Get detection component
        detection = GetComponent<EnemyDetection>();
        
        // Get alert meter component
        alertMeter = GetComponent<EnemyAlertMeter>();
        
        // Initialize state machine
        StateMachine = new EnemyStateMachine();
        
        // Create state instances
        PatrolState = new EnemyState_Patrol(StateMachine, this);
        AlertState = new EnemyState_Alert(StateMachine, this);
        PursueState = new EnemyState_Pursue(StateMachine, this);
        DeadState = new EnemyState_Dead(StateMachine, this);
    }
    
    void Start()
    {
        Debug.Log("Start - Enemy");
        
        // Start in Patrol state - patrol path will be set by spawner
        StateMachine.Initialize(PatrolState);
    }
    
    /// <summary>
    /// Set the patrol path for this enemy (called by spawner)
    /// </summary>
    public void SetPatrolPath(PatrolPath path)
    {
        patrolPath = path;
    }

    void Update()
    {
        // Run detection every frame (across all states)
        if (detection != null)
        {
            detection.DetectPlayer();
        }
        
        // Update alert meter (only in certain states)
        if (alertMeter != null)
        {
            var currentState = StateMachine.CurrentState;
            
            // Only update alert meter in Patrol and Alert states
            if (currentState == PatrolState || currentState == AlertState)
            {
                alertMeter.UpdateAlertMeter();
            }
        }
        
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
    
    // Detection helper methods
    public bool DetectPlayer()
    {
        if (detection != null)
        {
            return detection.DetectPlayer();
        }
        return false;
    }
    
    public bool CanSeePlayer()
    {
        if (detection != null)
        {
            return detection.HasDetectedPlayer;
        }
        return false;
    }
    
    public Transform GetDetectedPlayer()
    {
        if (detection != null)
        {
            return detection.DetectedPlayer;
        }
        return null;
    }
    
    public bool IsPlayerInAttackRange()
    {
        if (detection != null && detection.HasDetectedPlayer)
        {
            return detection.GetDistanceToPlayer() <= attackRange;
        }
        return false;
    }
    
    // Add more helper methods as needed:
    // public void PerformAttack() { }
    // public void MoveTowardsPlayer() { }
    // etc.
    
    private void OnDestroy()
    {
        
    }
}
