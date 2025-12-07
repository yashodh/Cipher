using UnityEngine;

/// <summary>
/// Base class for all player states
/// </summary>
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    public PlayerState(PlayerStateMachine stateMachine, Player player)
    {
        this.stateMachine = stateMachine;
        this.player = player;
    }

    /// <summary>
    /// Called when entering the state
    /// </summary>
    public virtual void Enter()
    {
        // Override in derived classes
    }

    /// <summary>
    /// Called every frame while in this state
    /// </summary>
    public virtual void Update()
    {
        // Override in derived classes
    }

    /// <summary>
    /// Called every fixed frame while in this state
    /// </summary>
    public virtual void FixedUpdate()
    {
        // Override in derived classes
    }

    /// <summary>
    /// Called when exiting the state
    /// </summary>
    public virtual void Exit()
    {
        // Override in derived classes
    }
}

