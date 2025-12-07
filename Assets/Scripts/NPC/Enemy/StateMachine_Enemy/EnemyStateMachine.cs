using UnityEngine;

/// <summary>
/// State machine for managing enemy states
/// </summary>
public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    /// <summary>
    /// Initialize the state machine with a starting state
    /// </summary>
    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Change to a new state
    /// </summary>
    public void ChangeState(EnemyState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Update the current state
    /// </summary>
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    /// <summary>
    /// Fixed update the current state
    /// </summary>
    public void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdate();
        }
    }
}

