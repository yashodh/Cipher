using UnityEngine;

/// <summary>
/// Visual representation of enemy detection cone using wireframe
/// </summary>
[RequireComponent(typeof(EnemyDetection))]
public class EnemyDetectionVisual : MonoBehaviour
{
    [Header("Visual Settings")]
    [Tooltip("Number of segments for cone arc")]
    [SerializeField] private int arcSegments = 20;
    
    [Tooltip("Height of wireframe above ground")]
    [SerializeField] private float wireframeHeight = 0.1f;
    
    [Tooltip("Wireframe line width")]
    [SerializeField] private float wireframeWidth = 0.05f;
    
    [Header("Colors")]
    [Tooltip("Color when patrolling (unaware)")]
    [SerializeField] private Color patrolColor = new Color(0f, 0.5f, 1f, 0.8f); // Blue
    
    [Tooltip("Color when alert (suspicious)")]
    [SerializeField] private Color alertColor = new Color(1f, 1f, 0f, 0.8f); // Yellow
    
    [Tooltip("Color when detected (chasing)")]
    [SerializeField] private Color detectedColor = new Color(1f, 0f, 0f, 0.8f); // Red
    
    private EnemyDetection detection;
    private Enemy enemy;
    
    // Wireframe components
    private GameObject wireframeObject;
    private LineRenderer leftEdgeLine;
    private LineRenderer rightEdgeLine;
    private LineRenderer arcLine;
    
    [Header("Debug")]
    [Tooltip("Show wireframe (for debugging)")]
    [SerializeField] private bool showWireframe = false;
    
    private void Awake()
    {
        detection = GetComponent<EnemyDetection>();
        enemy = GetComponent<Enemy>();
        
        if (showWireframe)
        {
            CreateWireframe();
        }
    }
    
    private void Update()
    {
        if (showWireframe)
        {
            UpdateWireframe();
        }
    }
    
    private void CreateWireframe()
    {
        // Create parent object for wireframe lines
        wireframeObject = new GameObject("DetectionWireframe");
        wireframeObject.transform.SetParent(transform);
        wireframeObject.transform.localPosition = Vector3.zero;
        
        // Create left edge line
        GameObject leftEdgeObj = new GameObject("LeftEdge");
        leftEdgeObj.transform.SetParent(wireframeObject.transform);
        leftEdgeObj.transform.localPosition = Vector3.zero;
        leftEdgeLine = leftEdgeObj.AddComponent<LineRenderer>();
        SetupLineRenderer(leftEdgeLine);
        
        // Create right edge line
        GameObject rightEdgeObj = new GameObject("RightEdge");
        rightEdgeObj.transform.SetParent(wireframeObject.transform);
        rightEdgeObj.transform.localPosition = Vector3.zero;
        rightEdgeLine = rightEdgeObj.AddComponent<LineRenderer>();
        SetupLineRenderer(rightEdgeLine);
        
        // Create arc line
        GameObject arcObj = new GameObject("Arc");
        arcObj.transform.SetParent(wireframeObject.transform);
        arcObj.transform.localPosition = Vector3.zero;
        arcLine = arcObj.AddComponent<LineRenderer>();
        SetupLineRenderer(arcLine);
        arcLine.positionCount = arcSegments;
    }
    
    private void SetupLineRenderer(LineRenderer line)
    {
        line.startWidth = wireframeWidth;
        line.endWidth = wireframeWidth;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.white;
        line.endColor = Color.white;
        line.useWorldSpace = false;
        line.positionCount = 2;
    }
    
    private void UpdateWireframe()
    {
        if (detection == null || leftEdgeLine == null || rightEdgeLine == null || arcLine == null) return;
        
        float radius = detection.DetectionRange;
        float angle = detection.FieldOfViewAngle;
        float halfAngle = angle / 2f;
        
        Vector3 origin = Vector3.up * detection.ViewHeight;
        
        // Calculate edge endpoints
        Vector3 leftDirection = Quaternion.Euler(0, -halfAngle, 0) * Vector3.forward * radius;
        Vector3 rightDirection = Quaternion.Euler(0, halfAngle, 0) * Vector3.forward * radius;
        
        // Project to ground level
        leftDirection.y = 0;
        rightDirection.y = 0;
        
        // Update left edge
        leftEdgeLine.SetPosition(0, origin);
        leftEdgeLine.SetPosition(1, leftDirection + Vector3.up * wireframeHeight);
        
        // Update right edge
        rightEdgeLine.SetPosition(0, origin);
        rightEdgeLine.SetPosition(1, rightDirection + Vector3.up * wireframeHeight);
        
        // Update arc
        float angleStep = angle / (arcSegments - 1);
        for (int i = 0; i < arcSegments; i++)
        {
            float currentAngle = -halfAngle + (angleStep * i);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * radius;
            direction.y = 0;
            arcLine.SetPosition(i, direction + Vector3.up * wireframeHeight);
        }
        
        // Update colors based on current state
        Color wireColor = GetCurrentStateColor();
        wireColor.a = 0.8f; // Make wireframe more opaque
        
        leftEdgeLine.startColor = wireColor;
        leftEdgeLine.endColor = wireColor;
        rightEdgeLine.startColor = wireColor;
        rightEdgeLine.endColor = wireColor;
        arcLine.startColor = wireColor;
        arcLine.endColor = wireColor;
    }
    
    private Color GetCurrentStateColor()
    {
        // Color based on alert meter level and state
        if (enemy != null && enemy.StateMachine != null)
        {
            var currentState = enemy.StateMachine.CurrentState;
            
            // Red if in Pursue state (fully alerted)
            if (currentState == enemy.PursueState)
            {
                return detectedColor;
            }
            
            // Yellow if player in cone or in Alert state (suspicious)
            if (currentState == enemy.AlertState || (detection != null && detection.HasDetectedPlayer))
            {
                return alertColor;
            }
        }
        
        // Blue - normal patrol/idle
        return patrolColor;
    }
    
    /// <summary>
    /// Show or hide the detection cone wireframe
    /// </summary>
    public void SetVisible(bool visible)
    {
        if (wireframeObject != null)
        {
            wireframeObject.SetActive(visible);
        }
    }
    
    private void OnDestroy()
    {
        if (wireframeObject != null)
        {
            Destroy(wireframeObject);
        }
    }
}

