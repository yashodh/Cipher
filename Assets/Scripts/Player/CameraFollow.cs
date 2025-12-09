using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Camera controller that follows the player from behind at a fixed offset with mouse control
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The player transform to follow")]
    [SerializeField] private Transform target;
    
    [Header("Camera Distance")]
    [Tooltip("Distance from the target")]
    [SerializeField] private float distance = 5f;
    
    [Header("Mouse Control")]
    [Tooltip("Mouse sensitivity for horizontal rotation")]
    [SerializeField] private float mouseSensitivityX = 2f;
    
    [Tooltip("Mouse sensitivity for vertical rotation")]
    [SerializeField] private float mouseSensitivityY = 2f;
    
    [Tooltip("Minimum vertical angle (looking down)")]
    [SerializeField] private float minVerticalAngle = -20f;
    
    [Tooltip("Maximum vertical angle (looking up)")]
    [SerializeField] private float maxVerticalAngle = 60f;
    
    [Header("Smoothing")]
    [Tooltip("How smoothly the camera follows (0 = instant, higher = smoother)")]
    [SerializeField] private float smoothSpeed = 10f;
    
    [Header("Look At Settings")]
    [Tooltip("Height offset for what the camera looks at on the target")]
    [SerializeField] private float lookAtHeightOffset = 1.5f;
    
    private Vector3 currentVelocity;
    private float currentHorizontalAngle;
    private float currentVerticalAngle;
    
    /// <summary>
    /// Set the target for the camera to follow
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        
        if (target != null)
        {
            // Initialize angles based on target's current rotation
            currentHorizontalAngle = target.eulerAngles.y;
            currentVerticalAngle = 20f; // Start with slight downward angle
            
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
        
        // Read mouse input
        HandleMouseInput();
        
        // Follow target
        FollowTarget();
    }
    
    /// <summary>
    /// Handle mouse input for camera rotation
    /// </summary>
    private void HandleMouseInput()
    {
        // Get mouse delta using new Input System
        Vector2 mouseDelta = Vector2.zero;
        
        if (Mouse.current != null)
        {
            mouseDelta = Mouse.current.delta.ReadValue();
        }
        
        // Update angles based on mouse movement
        currentHorizontalAngle += mouseDelta.x * mouseSensitivityX * 0.1f;
        currentVerticalAngle -= mouseDelta.y * mouseSensitivityY * 0.1f;
        
        // Clamp vertical angle to prevent flipping
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
    }
    
    private void FollowTarget()
    {
        // Calculate desired position based on mouse angles
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
        
        // Always look at target
        Vector3 lookAtPosition = target.position + Vector3.up * lookAtHeightOffset;
        transform.LookAt(lookAtPosition);
    }
    
    private Vector3 GetDesiredPosition()
    {
        // Calculate camera position based on horizontal and vertical angles
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0f);
        Vector3 offset = rotation * Vector3.back * distance;
        
        return target.position + offset + Vector3.up * lookAtHeightOffset;
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

