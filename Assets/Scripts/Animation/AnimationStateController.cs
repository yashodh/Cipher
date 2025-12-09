using UnityEngine;

/// <summary>
/// Base animation controller with common functionality for all characters
/// </summary>
public abstract class AnimationStateController : MonoBehaviour
{
    [Header("Animator")]
    [Tooltip("The Animator component to control")]
    [SerializeField] protected Animator animator;
    
    [Header("Settings")]
    [Tooltip("Auto-find Animator component if not assigned")]
    [SerializeField] protected bool autoFindAnimator = true;
    
    protected virtual void Awake()
    {
        // Find animator if not assigned
        if (animator == null && autoFindAnimator)
        {
            animator = GetComponent<Animator>();
            
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            
            if (animator == null)
            {
                Debug.LogWarning($"[{GetType().Name}] No Animator component found!");
            }
        }
    }
    
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }
    
    /// <summary>
    /// Check if animator is valid and ready to use
    /// </summary>
    protected bool IsAnimatorValid()
    {
        return animator != null && animator.runtimeAnimatorController != null;
    }
    
    /// <summary>
    /// Set a float parameter on the animator
    /// </summary>
    protected void SetFloat(string paramName, float value)
    {
        if (IsAnimatorValid())
        {
            animator.SetFloat(paramName, value);
        }
    }
    
    /// <summary>
    /// Set an integer parameter on the animator
    /// </summary>
    protected void SetInt(string paramName, int value)
    {
        if (IsAnimatorValid())
        {
            animator.SetInteger(paramName, value);
        }
    }
    
    /// <summary>
    /// Set a boolean parameter on the animator
    /// </summary>
    protected void SetBool(string paramName, bool value)
    {
        if (IsAnimatorValid())
        {
            animator.SetBool(paramName, value);
        }
    }
    
    /// <summary>
    /// Trigger an animation
    /// </summary>
    protected void SetTrigger(string paramName)
    {
        if (IsAnimatorValid())
        {
            animator.SetTrigger(paramName);
        }
    }
    
    /// <summary>
    /// Reset a trigger
    /// </summary>
    protected void ResetTrigger(string paramName)
    {
        if (IsAnimatorValid())
        {
            animator.ResetTrigger(paramName);
        }
    }
    
    /// <summary>
    /// Common animation for death - override for custom behavior
    /// </summary>
    public virtual void PlayDeathAnimation()
    {
        SetTrigger("Death");
    }
    
    /// <summary>
    /// Common animation for idle - override for custom behavior
    /// </summary>
    public virtual void PlayIdleAnimation()
    {
        SetBool("IsIdle", true);
    }
}
