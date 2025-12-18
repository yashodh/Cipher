using UnityEngine;

/// <summary>
/// Animation controller for Enemy characters
/// Handles animations for: Idle, Alert, Hide, Pursue, Attack, Dead states
/// </summary>
public class EnemyAnimationStateController : AnimationStateController
{
    [Header("Enemy Animation Parameters")]
    [Tooltip("Name of the speed parameter in the Animator")]
    [SerializeField] private string speedParamName = "Speed";
    
    [Tooltip("Name of the state parameter in the Animator")]
    [SerializeField] private string stateParamName = "State";
    
    [Tooltip("Name of the attack trigger in the Animator")]
    [SerializeField] private string attackTriggerName = "Attack";
    
    // State values
    public const int STATE_PATROL = 0;
    public const int STATE_ALERT = 1;
    public const int STATE_PURSUE = 2;
    public const int STATE_DEAD = 3;
    
    private Enemy enemy;
    
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
        
        if (enemy == null)
        {
            Debug.LogWarning($"[EnemyAnimationStateController] No Enemy component found!");
        }
    }
    
    protected override void Start()
    {
        base.Start();
        
        // Initialize with idle animation
        PlayIdleAnimation();
    }
    
    /// <summary>
    /// Play idle animation
    /// </summary>
    public override void PlayIdleAnimation()
    {
        SetFloat(speedParamName, 0f);
    }
    
    /// <summary>
    /// Play alert animation
    /// </summary>
    public void PlayAlertAnimation()
    {
        SetFloat(speedParamName, 0f);
    }
    
    /// <summary>
    /// Play hide animation
    /// </summary>
    public void PlayHideAnimation()
    {
        SetFloat(speedParamName, 0f);
    }
    
    /// <summary>
    /// Play pursue animation with speed
    /// </summary>
    public void PlayPursueAnimation(float speed)
    {
        SetFloat(speedParamName, speed);
    }
    
    /// <summary>
    /// Play pursue animation
    /// </summary>
    public void PlayPursueAnimation()
    {
        SetFloat(speedParamName, 1f);
    }
    
    /// <summary>
    /// Play attack animation
    /// </summary>
    public void PlayAttackAnimation()
    {
        SetTrigger(attackTriggerName);
    }
    
    /// <summary>
    /// Play death animation
    /// </summary>
    public override void PlayDeathAnimation()
    {
        SetFloat(speedParamName, 0f);
        SetTrigger("Death");
    }
    
    /// <summary>
    /// Set movement speed (for blending between walk/run)
    /// </summary>
    public void SetMovementSpeed(float speed)
    {
        SetFloat(speedParamName, speed);
    }
    
    /// <summary>
    /// Set the current state in the animator
    /// </summary>
    /// <param name="stateValue">State integer: 0=Patrol, 1=Alert, 2=Pursue, 3=Dead</param>
    public void SetState(int stateValue)
    {
        SetInt(stateParamName, stateValue);
    }
    
    /// <summary>
    /// Check if attack animation is complete
    /// </summary>
    public bool IsAttackAnimationComplete()
    {
        if (!IsAnimatorValid()) return true;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 1.0f;
    }
    
    /// <summary>
    /// Check if death animation is complete
    /// </summary>
    public bool IsDeathAnimationComplete()
    {
        if (!IsAnimatorValid()) return true;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Death") && stateInfo.normalizedTime >= 1.0f;
    }
}

