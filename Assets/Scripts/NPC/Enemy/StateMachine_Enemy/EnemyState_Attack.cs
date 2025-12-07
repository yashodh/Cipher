using UnityEngine;

/// <summary>
/// Attack state - enemy is attacking the player
/// </summary>
public class EnemyState_Attack : EnemyState
{
    private float attackCooldown;
    private float attackCooldownDuration = 1.5f; // Time between attacks

    public EnemyState_Attack(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Attack state");
        attackCooldown = 0f;
        // Add attack animation trigger here
        // Face the player
    }

    public override void Update()
    {
        base.Update();
        
        attackCooldown += Time.deltaTime;
        
        // Example: Perform attack
        // if (attackCooldown >= attackCooldownDuration)
        // {
        //     enemy.PerformAttack();
        //     attackCooldown = 0f;
        // }
        
        // Example: Player moved out of range - pursue
        // if (!enemy.IsPlayerInAttackRange())
        // {
        //     stateMachine.ChangeState(enemy.PursueState);
        // }
        
        // Example: Player lost - alert
        // if (!enemy.CanSeePlayer())
        // {
        //     stateMachine.ChangeState(enemy.AlertState);
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
        Debug.Log("Enemy exited Attack state");
        // Cancel attack animations
    }
}

