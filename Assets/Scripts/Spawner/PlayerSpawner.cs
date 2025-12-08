using UnityEngine;

/// <summary>
/// Spawner specifically for spawning the player
/// </summary>
public class PlayerSpawner : Spawner
{
    [Header("Player Spawner Settings")]
    [Tooltip("Should the camera follow the spawned player?")]
    [SerializeField] private bool setCameraTarget = true;
    
    [Tooltip("Camera that should follow the player (if null, will find Main Camera)")]
    [SerializeField] private CameraFollow cameraFollow;
    
    protected override void OnSpawned(GameObject spawnedObj)
    {
        base.OnSpawned(spawnedObj);
        
        // Ensure the spawned object has a Player component
        Player player = spawnedObj.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning($"[PlayerSpawner] Spawned object does not have a Player component!");
        }
        else
        {
            Debug.Log($"[PlayerSpawner] Player spawned successfully");
        }
        
        // Optional: Set up camera to follow player
        if (setCameraTarget)
        {
            SetupCameraTarget(spawnedObj);
        }
        
        // Add any other player-specific initialization here
    }
    
    private void SetupCameraTarget(GameObject player)
    {
        // Find CameraFollow component if not assigned
        if (cameraFollow == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraFollow = mainCamera.GetComponent<CameraFollow>();
                
                // Add CameraFollow component if it doesn't exist
                if (cameraFollow == null)
                {
                    cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
                    Debug.Log($"[PlayerSpawner] Added CameraFollow component to Main Camera");
                }
            }
            else
            {
                Debug.LogWarning($"[PlayerSpawner] No Main Camera found in scene!");
                return;
            }
        }
        
        // Set the player as the camera target
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(player.transform);
            Debug.Log($"[PlayerSpawner] Camera now following player");
        }
    }
    
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 pos = GetSpawnPosition();
        Gizmos.DrawWireSphere(pos, 0.5f);
        Gizmos.DrawLine(pos, pos + Vector3.up * 2f);
    }
    
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 pos = GetSpawnPosition();
        Gizmos.DrawSphere(pos, 0.5f);
        Gizmos.DrawLine(pos, pos + Vector3.up * 2f);
    }
}

