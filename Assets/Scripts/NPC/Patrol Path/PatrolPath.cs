using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines how the patrol path should behave
/// </summary>
public enum PatrolPathType
{
    Once,       // Go through path once and stop at the end
    Loop,       // Loop back to the beginning after reaching the end
    PingPong    // Reverse direction at each end
}

/// <summary>
/// Manages a patrol path consisting of multiple patrol nodes
/// </summary>
public class PatrolPath : MonoBehaviour
{
    [Header("Patrol Nodes")]
    [Tooltip("List of patrol nodes that make up this path")]
    [SerializeField] private List<PatrolNode> patrolNodes = new List<PatrolNode>();
    
    [Header("Path Settings")]
    [Tooltip("How should the patrol path behave?")]
    [SerializeField] private PatrolPathType pathType = PatrolPathType.Loop;
    
    public List<PatrolNode> PatrolNodes => patrolNodes;
    public PatrolPathType PathType => pathType;
    
    /// <summary>
    /// Get the number of nodes in this path
    /// </summary>
    public int NodeCount => patrolNodes.Count;
    
    /// <summary>
    /// Get a specific node by index
    /// </summary>
    public PatrolNode GetNode(int index)
    {
        if (index >= 0 && index < patrolNodes.Count)
        {
            return patrolNodes[index];
        }
        return null;
    }
    
    /// <summary>
    /// Get the position of a specific node
    /// </summary>
    public Vector3 GetNodePosition(int index)
    {
        PatrolNode node = GetNode(index);
        return node != null ? node.Position : Vector3.zero;
    }
    
    /// <summary>
    /// Get the next node index based on current index
    /// </summary>
    public int GetNextNodeIndex(int currentIndex, ref bool isReversing)
    {
        if (patrolNodes.Count == 0) return -1;
        if (patrolNodes.Count == 1) return 0;
        
        switch (pathType)
        {
            case PatrolPathType.PingPong:
                // Ping-pong logic
                if (isReversing)
                {
                    currentIndex--;
                    if (currentIndex <= 0)
                    {
                        currentIndex = 0;
                        isReversing = false;
                    }
                }
                else
                {
                    currentIndex++;
                    if (currentIndex >= patrolNodes.Count - 1)
                    {
                        currentIndex = patrolNodes.Count - 1;
                        isReversing = true;
                    }
                }
                break;
                
            case PatrolPathType.Loop:
                // Loop logic
                currentIndex = (currentIndex + 1) % patrolNodes.Count;
                break;
                
            case PatrolPathType.Once:
                // One-way path
                currentIndex++;
                if (currentIndex >= patrolNodes.Count)
                {
                    currentIndex = patrolNodes.Count - 1; // Stay at last node
                }
                break;
        }
        
        return currentIndex;
    }
    
    // Draw the patrol path in the editor
    private void OnDrawGizmos()
    {
        if (patrolNodes == null || patrolNodes.Count == 0) return;
        
        Gizmos.color = Color.red;
        
        // Draw lines between nodes
        for (int i = 0; i < patrolNodes.Count - 1; i++)
        {
            if (patrolNodes[i] != null && patrolNodes[i + 1] != null)
            {
                Gizmos.DrawLine(patrolNodes[i].Position, patrolNodes[i + 1].Position);
            }
        }
        
        // Draw line back to start if looping
        if (pathType == PatrolPathType.Loop && patrolNodes.Count > 1)
        {
            if (patrolNodes[patrolNodes.Count - 1] != null && patrolNodes[0] != null)
            {
                Gizmos.DrawLine(patrolNodes[patrolNodes.Count - 1].Position, patrolNodes[0].Position);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (patrolNodes == null || patrolNodes.Count == 0) return;
        
        Gizmos.color = Color.green;
        
        // Draw lines between nodes
        for (int i = 0; i < patrolNodes.Count - 1; i++)
        {
            if (patrolNodes[i] != null && patrolNodes[i + 1] != null)
            {
                Gizmos.DrawLine(patrolNodes[i].Position, patrolNodes[i + 1].Position);
            }
        }
        
        // Draw line back to start if looping
        if (pathType == PatrolPathType.Loop && patrolNodes.Count > 1)
        {
            if (patrolNodes[patrolNodes.Count - 1] != null && patrolNodes[0] != null)
            {
                Gizmos.DrawLine(patrolNodes[patrolNodes.Count - 1].Position, patrolNodes[0].Position);
            }
        }
    }
}

