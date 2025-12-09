using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerAnimationStateController AnimationController { get; private set; }
    
    // State instances
    public PlayerState_Idle IdleState { get; private set; }
    public PlayerState_Move MoveState { get; private set; }
    public PlayerState_Dead DeadState { get; private set; }
    
    // Input properties
    public Vector2 InputVector { get; private set; }
    public bool IsMoving => InputVector.magnitude > 0.1f;
    public bool IsCrouched { get; private set; }
    public bool IsSprinting { get; private set; }
    
    // Speed tracking
    private float currentSpeed = 0f;
    public float CurrentSpeed => currentSpeed;
    
    // Movement properties
    public float MoveSpeed => moveSpeed;
    public float MaxSpeed => maxSpeed;
    public float Acceleration => acceleration;
    public float Deceleration => deceleration;
    public float CrouchSpeed => crouchSpeed;
    public float SprintSpeedMultiplier => sprintSpeedMultiplier;
    public float RotationSpeed => rotationSpeed;
    public CharacterController CharacterController => characterController;
    
    /// <summary>
    /// Get target speed based on current state
    /// </summary>
    public float GetTargetSpeed()
    {
        if (IsCrouched) return crouchSpeed;
        if (IsSprinting) return maxSpeed;
        return moveSpeed;
    }
    
    void Awake()
    {
        Debug.Log("Awake - Player");
        
        // Get or add components
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
            }
        }
        
        // Get animation controller
        AnimationController = GetComponent<PlayerAnimationStateController>();
        if (AnimationController == null)
        {
            Debug.LogWarning("[Player] No PlayerAnimationStateController found! Add one for animations.");
        }
        
        // Initialize state machine
        StateMachine = new PlayerStateMachine();
        
        // Create state instances
        IdleState = new PlayerState_Idle(StateMachine, this);
        MoveState = new PlayerState_Move(StateMachine, this);
        DeadState = new PlayerState_Dead(StateMachine, this);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start - Player");
        
        // Set initial state to Idle
        StateMachine.Initialize(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        // Read input
        ReadInput();
        
        // Update state machine
        StateMachine.Update();
    }
    
    void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
    
    /// <summary>
    /// Read player input from keyboard using new Input System
    /// </summary>
    private void ReadInput()
    {
        float horizontal = 0f;
        float vertical = 0f;
        
        // WASD keys using new Input System
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            if (UnityEngine.InputSystem.Keyboard.current.wKey.isPressed) vertical += 1f;
            if (UnityEngine.InputSystem.Keyboard.current.sKey.isPressed) vertical -= 1f;
            if (UnityEngine.InputSystem.Keyboard.current.aKey.isPressed) horizontal -= 1f;
            if (UnityEngine.InputSystem.Keyboard.current.dKey.isPressed) horizontal += 1f;

            IsSprinting = UnityEngine.InputSystem.Keyboard.current.leftShiftKey.isPressed ? true : false;
            IsCrouched = (UnityEngine.InputSystem.Keyboard.current.cKey.isPressed || UnityEngine.InputSystem.Keyboard.current.leftCtrlKey.isPressed) ? true : false;
        }
        
        InputVector = new Vector2(horizontal, vertical);
        
        // Normalize to prevent faster diagonal movement
        if (InputVector.magnitude > 1f)
        {
            InputVector.Normalize();
        }
    }
    
    /// <summary>
    /// Move the player in the given direction
    /// </summary>
    public void Move(Vector3 direction)
    {
        if (characterController != null && characterController.enabled)
        {
            // Apply gravity
            direction.y -= 9.81f * Time.deltaTime;
            
            // Move character
            characterController.Move(direction * Time.deltaTime);
        }
        else
        {
            // Fallback if no CharacterController
            transform.position += direction * Time.deltaTime;
        }
    }
    
    /// <summary>
    /// Rotate the player to face a direction
    /// </summary>
    public void RotateToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Update current speed with acceleration/deceleration
    /// </summary>
    public void UpdateSpeed(bool isMoving)
    {
        currentSpeed = isMoving ? Mathf.MoveTowards(currentSpeed, GetTargetSpeed(), acceleration * Time.deltaTime)
                                : Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
    }
    
    /// <summary>
    /// Reset current speed to zero
    /// </summary>
    public void ResetSpeed()
    {
        currentSpeed = 0f;
    }

    private void OnDestroy()
    {
        
    }
}
