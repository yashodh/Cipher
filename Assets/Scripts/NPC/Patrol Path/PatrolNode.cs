using UnityEngine;

/// <summary>
/// Represents a single patrol point in a patrol path
/// </summary>
public class PatrolNode : MonoBehaviour
{
    [Header("Node Settings")]
    [Tooltip("Time to wait at this node before moving to the next")]
    [SerializeField] private float waitTime = 2f;
    
    [Tooltip("Custom behavior at this node (optional)")]
    [SerializeField] private bool lookAround = false;
    
    public float WaitTime => waitTime;
    public bool LookAround => lookAround;
    
    /// <summary>
    /// Get the world position of this patrol node
    /// </summary>
    public Vector3 Position => transform.position;
    
    // Visual feedback in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);

        #if UNITY_EDITOR
                UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, this.name);
        #endif
    }
}

