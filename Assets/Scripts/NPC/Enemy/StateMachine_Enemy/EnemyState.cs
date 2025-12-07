using UnityEngine;

/// <summary>
/// Base class for all enemy states
/// </summary>
public abstract class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
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

