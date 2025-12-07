using UnityEngine;

/// <summary>
/// Hide state - enemy is taking cover or hiding from threat
/// </summary>
public class EnemyState_Hide : EnemyState
{
    public EnemyState_Hide(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Hide state");
        // Add hide animation trigger here
        // Find nearest cover point
        // Move to cover
    }

    public override void Update()
    {
        base.Update();
        
        // Example: Peek from cover and check if safe
        // if (!enemy.IsInDanger())
        // {
        //     stateMachine.ChangeState(enemy.AlertState);
        // }
        
        // Example: Player gets too close - run to new cover
        // if (enemy.PlayerTooClose())
        // {
        //     // Find new hiding spot
        // }
        
        // Example: Opportunity to attack from cover
        // if (enemy.HasClearShot())
        // {
        //     stateMachine.ChangeState(enemy.AttackState);
        // }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Hide state");
        // Leave cover
    }
}

