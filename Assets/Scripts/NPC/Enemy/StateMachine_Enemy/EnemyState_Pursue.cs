using UnityEngine;

/// <summary>
/// Pursue state - enemy is chasing the player
/// </summary>
public class EnemyState_Pursue : EnemyState
{
    private float lostTargetTimer;
    private float lostTargetDuration = 3f; // How long before giving up chase

    public EnemyState_Pursue(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Pursue state");
        lostTargetTimer = 0f;
        // Add run/chase animation trigger here
        // Increase movement speed
    }

    public override void Update()
    {
        base.Update();
        
        // Example: Chase player
        // enemy.MoveTowardsPlayer();
        
        // Example: Get in range to attack
        // if (enemy.IsPlayerInAttackRange())
        // {
        //     stateMachine.ChangeState(enemy.AttackState);
        // }
        
        // Example: Lost sight of player
        // if (!enemy.CanSeePlayer())
        // {
        //     lostTargetTimer += Time.deltaTime;
        //     if (lostTargetTimer >= lostTargetDuration)
        //     {
        //         stateMachine.ChangeState(enemy.AlertState);
        //     }
        // }
        // else
        // {
        //     lostTargetTimer = 0f;
        // }
        
        // Example: Health low - hide
        // if (enemy.HealthLow())
        // {
        //     stateMachine.ChangeState(enemy.HideState);
        // }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Pursue state");
        // Reset movement speed
    }
}

