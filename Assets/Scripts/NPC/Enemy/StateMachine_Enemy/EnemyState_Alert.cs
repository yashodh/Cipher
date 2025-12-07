using UnityEngine;

/// <summary>
/// Alert state - enemy has detected something suspicious
/// </summary>
public class EnemyState_Alert : EnemyState
{
    private float alertTimer;
    private float alertDuration = 2f; // How long to stay alert before investigating

    public EnemyState_Alert(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Alert state");
        alertTimer = 0f;
        // Add alert animation trigger here
        // Play alert sound
        // Look around behavior
    }

    public override void Update()
    {
        base.Update();
        
        alertTimer += Time.deltaTime;
        
        // Example: Confirm player sighting - transition to Pursue
        // if (enemy.CanSeePlayer())
        // {
        //     stateMachine.ChangeState(enemy.PursueState);
        // }
        
        // Example: Alert timeout - return to Idle
        // if (alertTimer >= alertDuration)
        // {
        //     stateMachine.ChangeState(enemy.IdleState);
        // }
        
        // Example: Decide to hide if threatened
        // if (enemy.IsThreatened())
        // {
        //     stateMachine.ChangeState(enemy.HideState);
        // }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Alert state");
    }
}

