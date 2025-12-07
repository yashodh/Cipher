using UnityEngine;

/// <summary>
/// Idle state - enemy is patrolling or waiting
/// </summary>
public class EnemyState_Idle : EnemyState
{
    public EnemyState_Idle(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Idle state");
        // Add idle animation trigger here
        // Set patrol behavior
    }

    public override void Update()
    {
        base.Update();
        
        // Example: Check for player detection to transition to Alert state
        // if (enemy.DetectPlayer())
        // {
        //     stateMachine.ChangeState(enemy.AlertState);
        // }
        
        // Example: Continue patrol logic
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Idle state");
    }
}

