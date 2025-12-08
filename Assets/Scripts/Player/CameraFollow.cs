using UnityEngine;

/// <summary>
/// Camera controller that follows the player from behind at a fixed offset
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The player transform to follow")]
    [SerializeField] private Transform target;
    
    [Header("Camera Offset")]
    [Tooltip("Distance behind the target")]
    [SerializeField] private float distanceBehind = 5f;
    
    [Tooltip("Height above the target")]
    [SerializeField] private float heightAbove = 2f;
    
    [Header("Smoothing")]
    [Tooltip("How smoothly the camera follows (0 = instant, higher = smoother)")]
    [SerializeField] private float smoothSpeed = 10f;
    
    [Tooltip("How smoothly the camera rotates to follow target rotation")]
    [SerializeField] private float rotationSmoothSpeed = 5f;
    
    [Header("Look At Settings")]
    [Tooltip("Height offset for what the camera looks at on the target")]
    [SerializeField] private float lookAtHeightOffset = 1.5f;
    
    private Vector3 currentVelocity;
    
    /// <summary>
    /// Set the target for the camera to follow
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        
        if (target != null)
        {
            // Immediately position camera behind target when first set
            Vector3 desiredPosition = GetDesiredPosition();
            transform.position = desiredPosition;
            transform.LookAt(target.position + Vector3.up * lookAtHeightOffset);
        }
    }
    
    public Transform Target => target;
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        FollowTarget();
    }
    
    private void FollowTarget()
    {
        // Calculate desired position behind the target
        Vector3 desiredPosition = GetDesiredPosition();
        
        // Smoothly move camera to desired position
        if (smoothSpeed > 0)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                desiredPosition, 
                ref currentVelocity, 
                1f / smoothSpeed
            );
        }
        else
        {
            transform.position = desiredPosition;
        }
        
        // Calculate look at position (target position + height offset)
        Vector3 lookAtPosition = target.position + Vector3.up * lookAtHeightOffset;
        
        // Smoothly rotate to look at target
        Vector3 direction = lookAtPosition - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            if (rotationSmoothSpeed > 0)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    rotationSmoothSpeed * Time.deltaTime
                );
            }
            else
            {
                transform.rotation = targetRotation;
            }
        }
    }
    
    private Vector3 GetDesiredPosition()
    {
        // Calculate position behind target based on target's forward direction
        Vector3 offset = -target.forward * distanceBehind + Vector3.up * heightAbove;
        return target.position + offset;
    }
    
    // Visualize camera position in editor
    private void OnDrawGizmos()
    {
        if (target == null) return;
        
        Gizmos.color = Color.yellow;
        Vector3 desiredPos = GetDesiredPosition();
        Gizmos.DrawWireSphere(desiredPos, 0.3f);
        Gizmos.DrawLine(desiredPos, target.position + Vector3.up * lookAtHeightOffset);
    }
}

