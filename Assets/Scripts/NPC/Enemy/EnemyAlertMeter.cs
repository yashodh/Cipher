using UnityEngine;

/// <summary>
/// Alert meter that fills when player is detected
/// Fills faster when player is closer
/// </summary>
public class EnemyAlertMeter : MonoBehaviour
{
    [Header("Alert Settings")]
    [Tooltip("Time to fully alert when player is at maximum range (slower)")]
    [SerializeField] private float alertTimeAtMaxRange = 3f;
    
    [Tooltip("Time to fully alert when player is very close (faster)")]
    [SerializeField] private float alertTimeAtMinRange = 1f;
    
    [Tooltip("How fast the meter decreases when player leaves cone")]
    [SerializeField] private float meterDecayRate = 0.5f;
    
    private float currentAlertLevel = 0f; // 0 to 1
    private Enemy enemy;
    private EnemyDetection detection;
    
    public float AlertLevel => currentAlertLevel;
    public bool IsFullyAlerted => currentAlertLevel >= 1f;
    public float AlertProgress => currentAlertLevel; // 0 to 1
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        detection = GetComponent<EnemyDetection>();
    }
    
    /// <summary>
    /// Update the alert meter based on player detection
    /// </summary>
    public void UpdateAlertMeter()
    {
        if (detection == null) return;
        
        if (detection.HasDetectedPlayer)
        {
            // Player is in cone - increase alert
            float distanceToPlayer = detection.GetDistanceToPlayer();
            float detectionRange = detection.DetectionRange;
            
            // Calculate fill rate based on distance (closer = faster)
            // Normalize distance: 0 (very close) to 1 (max range)
            float normalizedDistance = Mathf.Clamp01(distanceToPlayer / detectionRange);
            
            // Lerp between fast fill (close) and slow fill (far)
            float fillTime = Mathf.Lerp(alertTimeAtMinRange, alertTimeAtMaxRange, normalizedDistance);
            float fillRate = 1f / fillTime;
            
            // Increase alert level
            currentAlertLevel += fillRate * Time.deltaTime;
            currentAlertLevel = Mathf.Clamp01(currentAlertLevel);
        }
        else
        {
            // Player not in cone - decrease alert
            currentAlertLevel -= meterDecayRate * Time.deltaTime;
            currentAlertLevel = Mathf.Max(0f, currentAlertLevel);
        }
    }
    
    /// <summary>
    /// Reset the alert meter to zero
    /// </summary>
    public void ResetMeter()
    {
        currentAlertLevel = 0f;
    }
    
    /// <summary>
    /// Set the alert meter to a specific value (0-1)
    /// </summary>
    public void SetAlertLevel(float level)
    {
        currentAlertLevel = Mathf.Clamp01(level);
    }
}

