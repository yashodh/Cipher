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
        Debug.Log("Enemy entered Pursue state - actively chasing player");
        lostTargetTimer = 0f;
        
        // Set animation state to Pursue
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.SetState(EnemyAnimationStateController.STATE_PURSUE);
            animController.SetMovementSpeed(enemy.PursueSpeed);
        }
        
        // Set pursue detection parameters (wider FOV, longer range)
        if (enemy.Detection != null)
        {
            enemy.Detection.SetDetectionParameters(enemy.PursueDetectionRange, enemy.PursueFieldOfView);
        }
        
        // Enable NavMesh agent for pursuit
        if (enemy.NavMeshAgent != null)
        {
            enemy.NavMeshAgent.isStopped = false;
            enemy.NavMeshAgent.speed = enemy.PursueSpeed;
        }
    }

    public override void Update()
    {
        base.Update();
        
        // Check if player is still in detection cone
        if (!enemy.CanSeePlayer())
        {
            // Player left the cone - return to Alert state
            Debug.Log("Enemy lost sight of player during pursue - returning to Alert");
            stateMachine.ChangeState(enemy.AlertState);
            return;
        }
        
        // Continuously update destination to player's current position
        if (enemy.Detection != null && enemy.Detection.HasDetectedPlayer)
        {
            Transform playerTransform = enemy.Detection.DetectedPlayer;
            
            if (playerTransform != null && enemy.NavMeshAgent != null)
            {
                // Update NavMesh destination every frame to actively chase player
                enemy.NavMeshAgent.SetDestination(playerTransform.position);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Pursue state");
        
        // Stop NavMesh agent
        if (enemy.NavMeshAgent != null)
        {
            enemy.NavMeshAgent.isStopped = true;
        }
    }
}

