using UnityEngine;

/// <summary>
/// Spawner specifically for spawning enemies
/// </summary>
public class EnemySpawner : Spawner
{
    [Header("Enemy Spawner Settings")]
    [Tooltip("Patrol path to assign to the spawned enemy")]
    [SerializeField] private PatrolPath assignedPatrolPath;
    
    [Tooltip("Should the enemy be activated immediately after spawn?")]
    [SerializeField] private bool activateImmediately = true;
    
    [Tooltip("Delay before spawning (in seconds)")]
    [SerializeField] private float spawnDelay = 0f;
    
    protected override void Start()
    {
        if (spawnOnStart)
        {
            if (spawnDelay > 0)
            {
                Invoke(nameof(Spawn), spawnDelay);
            }
            else
            {
                Spawn();
            }
        }
    }
    
    protected override void OnSpawned(GameObject spawnedObj)
    {
        base.OnSpawned(spawnedObj);
        
        // Ensure the spawned object has an Enemy component
        Enemy enemy = spawnedObj.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning($"[EnemySpawner] Spawned object does not have an Enemy component!");
            return;
        }
        
        Debug.Log($"[EnemySpawner] Enemy spawned successfully");
        
        // Assign patrol path (enemy will start patrolling after 5 seconds of idle)
        if (assignedPatrolPath != null)
        {
            enemy.SetPatrolPath(assignedPatrolPath);
            Debug.Log($"[EnemySpawner] Patrol path assigned to enemy - will start patrol after idle period");
        }
        else
        {
            Debug.LogWarning($"[EnemySpawner] No patrol path assigned - enemy will remain idle");
        }
        
        // Activate or deactivate enemy based on settings
        if (!activateImmediately)
        {
            // Example: Put enemy in idle state or disable AI
            Debug.Log($"[EnemySpawner] Enemy spawned but not activated");
        }
        
        // Add any other enemy-specific initialization here
    }
    
    /// <summary>
    /// Spawn enemy with a specific patrol path
    /// </summary>
    public GameObject SpawnWithPatrolPath(PatrolPath patrolPath)
    {
        assignedPatrolPath = patrolPath;
        return Spawn();
    }
    
    /// <summary>
    /// Respawn the enemy after death
    /// </summary>
    public void Respawn(float delay = 0f)
    {
        DespawnObject();
        
        if (delay > 0)
        {
            Invoke(nameof(Spawn), delay);
        }
        else
        {
            Spawn();
        }
    }
    
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = GetSpawnPosition();
        Gizmos.DrawWireSphere(pos, 0.5f);
        Gizmos.DrawLine(pos, pos + Vector3.up * 2f);
        
        // Draw connection to patrol path if assigned
        if (assignedPatrolPath != null && assignedPatrolPath.NodeCount > 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pos, assignedPatrolPath.GetNodePosition(0));
        }
    }
    
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Vector3 pos = GetSpawnPosition();
        Gizmos.DrawSphere(pos, 0.5f);
        Gizmos.DrawLine(pos, pos + Vector3.up * 2f);
        
        // Draw connection to patrol path if assigned
        if (assignedPatrolPath != null && assignedPatrolPath.NodeCount > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, assignedPatrolPath.GetNodePosition(0));
        }
    }
}

