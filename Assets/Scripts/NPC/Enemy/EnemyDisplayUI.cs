using UnityEngine;

/// <summary>
/// Unified display system for enemy - shows state and alert meter above enemy's head
/// </summary>
public class EnemyDisplayUI : MonoBehaviour
{
    [Header("Display Options")]
    [Tooltip("Show current enemy state")]
    [SerializeField] private bool showState = true;
    
    [Tooltip("Show alert meter percentage")]
    [SerializeField] private bool showAlertMeter = true;
    
    [Header("Position Settings")]
    [Tooltip("Height above enemy to display state text")]
    [SerializeField] private float stateTextHeight = 2.8f;
    
    [Tooltip("Height above enemy to display alert meter")]
    [SerializeField] private float alertMeterHeight = 3.3f;
    
    [Header("Style Settings")]
    [Tooltip("Font size for state text")]
    [SerializeField] private int stateFontSize = 12;
    
    [Tooltip("Font size for alert meter")]
    [SerializeField] private int alertFontSize = 14;
    
    [Header("State Text Colors")]
    [Tooltip("State text color when not alerted")]
    [SerializeField] private Color stateNormalColor = Color.white;
    
    [Tooltip("Use alert-based coloring for state text")]
    [SerializeField] private bool useAlertColorForState = true;
    
    [Header("Alert Meter Colors")]
    [SerializeField] private Color lowAlertColor = Color.yellow;
    [SerializeField] private Color highAlertColor = Color.red;
    [SerializeField] private Color normalAlertColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    
    [Header("Options")]
    [Tooltip("Only show alert meter when alert level > 0")]
    [SerializeField] private bool hideAlertWhenEmpty = true;
    
    private Enemy enemy;
    private EnemyAlertMeter alertMeter;
    private Camera mainCamera;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        alertMeter = GetComponent<EnemyAlertMeter>();
        mainCamera = Camera.main;
    }
    
    private void OnGUI()
    {
        if (mainCamera == null) return;
        
        // Draw state text
        if (showState && enemy != null && enemy.StateMachine != null)
        {
            DrawStateText();
        }
        
        // Draw alert meter
        if (showAlertMeter && alertMeter != null)
        {
            DrawAlertMeter();
        }
    }
    
    private void DrawStateText()
    {
        string stateName = GetCurrentStateName();
        Vector3 worldPosition = transform.position + Vector3.up * stateTextHeight;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        
        // Only draw if in front of camera
        if (screenPosition.z > 0)
        {
            screenPosition.y = Screen.height - screenPosition.y;
            
            // Determine color based on alert level
            Color textColor = stateNormalColor;
            if (useAlertColorForState && alertMeter != null)
            {
                float alertLevel = alertMeter.AlertLevel;
                if (alertLevel > 0.01f)
                {
                    textColor = Color.Lerp(lowAlertColor, highAlertColor, alertLevel);
                }
            }
            
            // Set up GUI style
            GUIStyle style = new GUIStyle();
            style.fontSize = stateFontSize;
            style.normal.textColor = textColor;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            
            // Add shadow for better readability
            GUIStyle shadowStyle = new GUIStyle(style);
            shadowStyle.normal.textColor = Color.black;
            
            // Calculate text size and position
            Vector2 textSize = style.CalcSize(new GUIContent(stateName));
            Rect textRect = new Rect(screenPosition.x - textSize.x / 2f, screenPosition.y - textSize.y / 2f, textSize.x, textSize.y);
            Rect shadowRect = new Rect(textRect.x + 1, textRect.y + 1, textRect.width, textRect.height);
            
            // Draw shadow then text
            GUI.Label(shadowRect, stateName, shadowStyle);
            GUI.Label(textRect, stateName, style);
        }
    }
    
    private void DrawAlertMeter()
    {
        float alertLevel = alertMeter.AlertLevel;
        
        // Hide when empty if desired
        if (hideAlertWhenEmpty && alertLevel < 0.01f) return;
        
        Vector3 worldPosition = transform.position + Vector3.up * alertMeterHeight;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        
        // Only draw if in front of camera
        if (screenPosition.z > 0)
        {
            screenPosition.y = Screen.height - screenPosition.y;
            
            // Calculate percentage
            int percentage = Mathf.RoundToInt(alertLevel * 100f);
            string displayText = $"{percentage}%";
            
            // Determine color based on alert level
            Color textColor;
            if (alertLevel > 0.01f)
            {
                textColor = Color.Lerp(lowAlertColor, highAlertColor, alertLevel);
            }
            else
            {
                textColor = normalAlertColor;
            }
            
            // Set up GUI style
            GUIStyle style = new GUIStyle();
            style.fontSize = alertFontSize;
            style.normal.textColor = textColor;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            
            // Add shadow for better readability
            GUIStyle shadowStyle = new GUIStyle(style);
            shadowStyle.normal.textColor = Color.black;
            
            // Calculate text size and position
            Vector2 textSize = style.CalcSize(new GUIContent(displayText));
            Rect textRect = new Rect(screenPosition.x - textSize.x / 2f, screenPosition.y - textSize.y / 2f, textSize.x, textSize.y);
            Rect shadowRect = new Rect(textRect.x + 1, textRect.y + 1, textRect.width, textRect.height);
            
            // Draw shadow then text
            GUI.Label(shadowRect, displayText, shadowStyle);
            GUI.Label(textRect, displayText, style);
        }
    }
    
    private string GetCurrentStateName()
    {
        if (enemy == null || enemy.StateMachine == null) return "Unknown";
        
        var currentState = enemy.StateMachine.CurrentState;
        
        if (currentState == enemy.PatrolState) return "PATROL";
        if (currentState == enemy.AlertState) return "ALERT";
        if (currentState == enemy.PursueState) return "PURSUE";
        if (currentState == enemy.DeadState) return "DEAD";
        
        return "UNKNOWN";
    }
    
    // Also draw in Scene view using Handles
    private void OnDrawGizmos()
    {
        if (enemy == null) return;
        
        #if UNITY_EDITOR
        // Draw state text in scene view
        if (showState && enemy.StateMachine != null)
        {
            string stateName = GetCurrentStateName();
            Vector3 worldPosition = transform.position + Vector3.up * stateTextHeight;
            
            // Determine color based on alert level
            Color textColor = stateNormalColor;
            if (useAlertColorForState && alertMeter != null)
            {
                float alertLevel = alertMeter.AlertLevel;
                if (alertLevel > 0.01f)
                {
                    textColor = Color.Lerp(lowAlertColor, highAlertColor, alertLevel);
                }
            }
            
            GUIStyle stateStyle = new GUIStyle();
            stateStyle.normal.textColor = textColor;
            stateStyle.fontSize = stateFontSize;
            stateStyle.fontStyle = FontStyle.Bold;
            stateStyle.alignment = TextAnchor.MiddleCenter;
            
            UnityEditor.Handles.Label(worldPosition, stateName, stateStyle);
        }
        
        // Draw alert meter in scene view
        if (showAlertMeter && alertMeter != null)
        {
            float alertLevel = alertMeter.AlertLevel;
            
            // Hide when empty if desired
            if (hideAlertWhenEmpty && alertLevel < 0.01f) return;
            
            int percentage = Mathf.RoundToInt(alertLevel * 100f);
            string displayText = $"{percentage}%";
            Vector3 worldPosition = transform.position + Vector3.up * alertMeterHeight;
            
            // Determine color based on alert level
            Color textColor;
            if (alertLevel > 0.01f)
            {
                textColor = Color.Lerp(lowAlertColor, highAlertColor, alertLevel);
            }
            else
            {
                textColor = normalAlertColor;
            }
            
            GUIStyle alertStyle = new GUIStyle();
            alertStyle.normal.textColor = textColor;
            alertStyle.fontSize = alertFontSize;
            alertStyle.fontStyle = FontStyle.Bold;
            alertStyle.alignment = TextAnchor.MiddleCenter;
            
            UnityEditor.Handles.Label(worldPosition, displayText, alertStyle);
        }
        #endif
    }
}

