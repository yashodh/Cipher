using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }
    
    // State instances
    public PlayerState_Idle IdleState { get; private set; }
    public PlayerState_Move MoveState { get; private set; }
    public PlayerState_Dead DeadState { get; private set; }
    
    void Awake()
    {
        Debug.Log("Awake - Player");
        
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
        StateMachine.Update();
    }
    
    void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void OnDestroy()
    {
        
    }
}
