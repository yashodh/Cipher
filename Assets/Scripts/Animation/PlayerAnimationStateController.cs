using UnityEngine;

/// <summary>
/// Animation controller for the Player character
/// Handles animations for: Idle, Move, Dead states
/// </summary>
public class PlayerAnimationStateController : AnimationStateController
{
    [Header("Player Animation Parameters")]
    [Tooltip("Name of the speed parameter in the Animator")]
    [SerializeField] private string speedParamName = "Speed";
    
    [Tooltip("Name of the is moving parameter in the Animator")]
    [SerializeField] private string isMovingParamName = "IsMoving";
    
    [Tooltip("Name of the is crouched parameter in the Animator")]
    [SerializeField] private string isCrouchedParamName = "IsCrouched";
    
    [Tooltip("Name of the is grounded parameter in the Animator")]
    [SerializeField] private string isGroundedParamName = "IsGrounded";
    
    private Player player;
    
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        
        if (player == null)
        {
            Debug.LogWarning($"[PlayerAnimationStateController] No Player component found!");
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
        SetBool(isMovingParamName, false);
        SetFloat(speedParamName, 0f);
    }
    
    /// <summary>
    /// Play movement animation
    /// </summary>
    public void PlayMoveAnimation(float speed)
    {
        SetBool(isMovingParamName, true);
        SetFloat(speedParamName, speed);
    }
    
    /// <summary>
    /// Play movement animation with direction
    /// </summary>
    public void PlayMoveAnimation(Vector3 velocity)
    {
        float speed = velocity.magnitude;
        SetBool(isMovingParamName, speed > 0.1f);
        SetFloat(speedParamName, speed);
    }
    
    /// <summary>
    /// Set crouched state
    /// </summary>
    public void SetCrouched(bool crouched)
    {
        SetBool(isCrouchedParamName, crouched);
    }
    
    /// <summary>
    /// Play death animation
    /// </summary>
    public override void PlayDeathAnimation()
    {
        SetBool(isMovingParamName, false);
        SetFloat(speedParamName, 0f);
        SetTrigger("Death");
    }
    
    /// <summary>
    /// Set grounded state
    /// </summary>
    public void SetGrounded(bool grounded)
    {
        SetBool(isGroundedParamName, grounded);
    }
    
    /// <summary>
    /// Trigger jump animation
    /// </summary>
    public void PlayJumpAnimation()
    {
        SetTrigger("Jump");
    }
    
    /// <summary>
    /// Trigger land animation
    /// </summary>
    public void PlayLandAnimation()
    {
        SetTrigger("Land");
    }
}

