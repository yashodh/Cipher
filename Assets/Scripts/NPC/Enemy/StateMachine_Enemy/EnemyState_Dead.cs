using UnityEngine;

/// <summary>
/// Dead state - enemy has been killed
/// </summary>
public class EnemyState_Dead : EnemyState
{
    public EnemyState_Dead(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Dead state");
        // Add death animation trigger here
        // Disable AI
        // Drop loot
        // Play death effects
        // Disable colliders or set to ragdoll
    }

    public override void Update()
    {
        base.Update();
        
        // Dead state typically doesn't transition to other states
        // Unless you have revival mechanics
        
        // Example: Fade out and destroy after animation
        // if (enemy.DeathAnimationComplete())
        // {
        //     GameObject.Destroy(enemy.gameObject, 2f);
        // }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Dead state");
    }
}

