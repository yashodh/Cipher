using UnityEngine;

/// <summary>
/// Detection system for enemy with FOV cone
/// </summary>
public class EnemyDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("Maximum distance to detect player (can be changed at runtime)")]
    [SerializeField] private float detectionRange = 10f;
    
    [Tooltip("Field of view angle (degrees) (can be changed at runtime)")]
    [SerializeField] private float fieldOfViewAngle = 90f;
    
    [Tooltip("Height at which detection originates")]
    [SerializeField] private float viewHeight = 1.5f;
    
    [Header("Runtime Info")]
    [Tooltip("Current active detection range")]
    [SerializeField] private float currentDetectionRange;
    
    [Tooltip("Current active FOV angle")]
    [SerializeField] private float currentFieldOfView;
    
    [Header("Detection Layers")]
    [Tooltip("Layer for obstacles that block vision (leave at 'Nothing' to ignore obstacles)")]
    [SerializeField] private LayerMask obstacleLayer;
    
    [Header("Debug")]
    [Tooltip("Show debug logs")]
    [SerializeField] private bool showDebugLogs = false;
    
    private Transform detectedPlayer;
    private Enemy enemy;
    private Vector3 lastKnownPlayerPosition;
    
    public Transform DetectedPlayer => detectedPlayer;
    public bool HasDetectedPlayer => detectedPlayer != null;
    public Vector3 LastKnownPlayerPosition => lastKnownPlayerPosition;
    public float DetectionRange => currentDetectionRange;
    public float FieldOfViewAngle => currentFieldOfView;
    public float ViewHeight => viewHeight;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        
        // Initialize current values
        currentDetectionRange = detectionRange;
        currentFieldOfView = fieldOfViewAngle;
    }
    
    /// <summary>
    /// Set detection parameters dynamically
    /// </summary>
    public void SetDetectionParameters(float range, float fovAngle)
    {
        currentDetectionRange = range;
        currentFieldOfView = fovAngle;
    }
    
    /// <summary>
    /// Reset to default detection parameters
    /// </summary>
    public void ResetDetectionParameters()
    {
        currentDetectionRange = detectionRange;
        currentFieldOfView = fieldOfViewAngle;
    }
    
    /// <summary>
    /// Check if player is in detection cone
    /// </summary>
    public bool DetectPlayer()
    {
        // Find player in scene (cache this for performance in real game)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            if (showDebugLogs)
            {
                Debug.LogWarning("[EnemyDetection] No GameObject with tag 'Player' found!");
            }
            detectedPlayer = null;
            return false;
        }
        
        Transform playerTransform = playerObj.transform;
        Vector3 detectionOrigin = transform.position + Vector3.up * viewHeight;
        Vector3 directionToPlayer = playerTransform.position - detectionOrigin;
        float distanceToPlayer = directionToPlayer.magnitude;
        
        // 1. Check if player is within detection range
        if (distanceToPlayer > currentDetectionRange)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[EnemyDetection] Player out of range: {distanceToPlayer:F2} > {currentDetectionRange}");
            }
            detectedPlayer = null;
            return false;
        }
        
        // 2. Check if player is within FOV angle
        directionToPlayer.y = 0; // Flatten to horizontal plane for angle check
        Vector3 forward = transform.forward;
        forward.y = 0;
        
        float angleToPlayer = Vector3.Angle(forward, directionToPlayer);
        if (angleToPlayer > currentFieldOfView / 2f)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[EnemyDetection] Player outside FOV: {angleToPlayer:F2}° > {currentFieldOfView / 2f}°");
            }
            detectedPlayer = null;
            return false;
        }
        
        // 3. Check line of sight (raycast for obstacles) - only if obstacleLayer is set
        if (obstacleLayer != 0)
        {
            Vector3 rayDirection = (playerTransform.position + Vector3.up * 1f) - detectionOrigin;
            if (Physics.Raycast(detectionOrigin, rayDirection.normalized, out RaycastHit hit, distanceToPlayer, obstacleLayer))
            {
                // Hit an obstacle, player is blocked
                if (showDebugLogs)
                {
                    Debug.Log($"[EnemyDetection] Line of sight blocked by: {hit.transform.name}");
                }
                detectedPlayer = null;
                return false;
            }
        }
        
        // Player detected!
        if (showDebugLogs)
        {
            Debug.Log($"[EnemyDetection] PLAYER DETECTED! Distance: {distanceToPlayer:F2}, Angle: {angleToPlayer:F2}°");
        }
        detectedPlayer = playerTransform;
        lastKnownPlayerPosition = playerTransform.position;
        return true;
    }
    
    /// <summary>
    /// Get direction to detected player
    /// </summary>
    public Vector3 GetDirectionToPlayer()
    {
        if (detectedPlayer != null)
        {
            return (detectedPlayer.position - transform.position).normalized;
        }
        return Vector3.zero;
    }
    
    /// <summary>
    /// Get distance to detected player
    /// </summary>
    public float GetDistanceToPlayer()
    {
        if (detectedPlayer != null)
        {
            return Vector3.Distance(transform.position, detectedPlayer.position);
        }
        return Mathf.Infinity;
    }
    
    // Debug visualization (shows current detection parameters)
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * viewHeight;
        
        // Use current detection parameters if available, otherwise use defaults
        float drawRange = Application.isPlaying ? currentDetectionRange : detectionRange;
        float drawAngle = Application.isPlaying ? currentFieldOfView : fieldOfViewAngle;
        
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, drawRange);
        
        // Draw FOV cone edges
        Vector3 forward = transform.forward;
        float halfAngle = drawAngle / 2f;
        
        Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * forward * drawRange;
        Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * forward * drawRange;
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, leftBoundary);
        Gizmos.DrawRay(origin, rightBoundary);
        Gizmos.DrawRay(origin, forward * drawRange);
    }
}

