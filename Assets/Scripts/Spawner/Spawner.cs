using UnityEngine;

/// <summary>
/// Base spawner class for spawning game objects in the world
/// </summary>
public abstract class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The prefab to spawn")]
    [SerializeField] protected GameObject prefab;
    
    [Tooltip("Should spawn automatically on Start?")]
    [SerializeField] protected bool spawnOnStart = false;
    
    [Tooltip("Use spawner's position or custom spawn point?")]
    [SerializeField] protected bool useSpawnerPosition = true;
    
    [Tooltip("Custom spawn position (if not using spawner position)")]
    [SerializeField] protected Transform spawnPoint;
    
    protected GameObject spawnedObject;
    
    public GameObject SpawnedObject => spawnedObject;
    
    protected virtual void Start()
    {
        if (spawnOnStart)
        {
            Spawn();
        }
    }
    
    /// <summary>
    /// Spawn the prefab at the designated location
    /// </summary>
    public virtual GameObject Spawn()
    {
        if (prefab == null)
        {
            Debug.LogError($"[{GetType().Name}] No prefab assigned to spawner!");
            return null;
        }
        
        Vector3 spawnPosition = GetSpawnPosition();
        Quaternion spawnRotation = GetSpawnRotation();
        
        spawnedObject = Instantiate(prefab, spawnPosition, spawnRotation);
        
        OnSpawned(spawnedObject);
        
        Debug.Log($"[{GetType().Name}] Spawned {prefab.name} at {spawnPosition}");
        
        return spawnedObject;
    }
    
    /// <summary>
    /// Get the position where the object should spawn
    /// </summary>
    protected virtual Vector3 GetSpawnPosition()
    {
        if (useSpawnerPosition)
        {
            return transform.position;
        }
        else if (spawnPoint != null)
        {
            return spawnPoint.position;
        }
        
        return transform.position;
    }
    
    /// <summary>
    /// Get the rotation for the spawned object
    /// </summary>
    protected virtual Quaternion GetSpawnRotation()
    {
        if (useSpawnerPosition)
        {
            return transform.rotation;
        }
        else if (spawnPoint != null)
        {
            return spawnPoint.rotation;
        }
        
        return Quaternion.identity;
    }
    
    /// <summary>
    /// Called after the object is spawned - override for custom behavior
    /// </summary>
    protected virtual void OnSpawned(GameObject spawnedObj)
    {
        // Override in derived classes for custom post-spawn behavior
    }
    
    /// <summary>
    /// Destroy the currently spawned object
    /// </summary>
    public virtual void DespawnObject()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
            Debug.Log($"[{GetType().Name}] Despawned object");
        }
    }
    
    // Visualize spawn point in editor
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = GetSpawnPosition();
        Gizmos.DrawWireSphere(pos, 0.5f);
        Gizmos.DrawLine(pos, pos + Vector3.up * 2f);
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = GetSpawnPosition();
        Gizmos.DrawSphere(pos, 0.5f);
        Gizmos.DrawLine(pos, pos + Vector3.up * 2f);
    }
}

