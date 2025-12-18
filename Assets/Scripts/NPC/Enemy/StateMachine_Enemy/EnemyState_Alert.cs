using UnityEngine;

/// <summary>
/// Alert state - enemy has detected something suspicious and investigates last known position
/// </summary>
public class EnemyState_Alert : EnemyState
{
    private float alertTimer;
    private float alertDuration = 5f; // How long to stay alert before returning to patrol
    private Vector3 investigationPoint;
    private bool hasReachedInvestigationPoint;
    private float investigationRadius = 1.5f; // How close to get to investigation point

    public EnemyState_Alert(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Alert state - investigating last known position...");
        alertTimer = 0f;
        hasReachedInvestigationPoint = false;
        
        // Set animation state to Alert
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.SetState(EnemyAnimationStateController.STATE_ALERT);
        }
        
        // Set alert detection parameters (wider FOV, shorter range)
        if (enemy.Detection != null)
        {
            enemy.Detection.SetDetectionParameters(enemy.AlertDetectionRange, enemy.AlertFieldOfView);
            
            // Get last known player position
            investigationPoint = enemy.Detection.LastKnownPlayerPosition;
            Debug.Log($"[Alert] Moving to investigate position: {investigationPoint}");
        }
        
        // Resume NavMesh agent to move to investigation point
        if (enemy.NavMeshAgent != null)
        {
            enemy.NavMeshAgent.isStopped = false;
            enemy.NavMeshAgent.speed = enemy.PatrolSpeed; // Move at patrol speed
            enemy.NavMeshAgent.SetDestination(investigationPoint);
        }
        
        // Set walking animation
        if (animController != null)
        {
            animController.SetMovementSpeed(enemy.PatrolSpeed);
        }
        
        // Add alert animation trigger here
        // Play alert sound
    }

    public override void Update()
    {
        base.Update();
        
        if (CheckForFullAlert()) return;
        
        UpdateInvestigationPoint();
        
        CheckIfReachedInvestigationPoint();
        
        if (HandleInvestigationTimeout()) return;
    }
    
    /// <summary>
    /// Check if alert meter is full and transition to Pursue state
    /// </summary>
    /// <returns>True if transitioning to Pursue</returns>
    private bool CheckForFullAlert()
    {
        if (enemy.AlertMeter != null && enemy.AlertMeter.IsFullyAlerted)
        {
            Debug.Log("Enemy fully alerted - pursuing player!");
            stateMachine.ChangeState(enemy.PursueState);
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Update investigation point if player is detected again
    /// </summary>
    private void UpdateInvestigationPoint()
    {
        if (!enemy.CanSeePlayer() || enemy.Detection == null) return;
        
        Vector3 newInvestigationPoint = enemy.Detection.LastKnownPlayerPosition;
        
        // If investigation point changed significantly, update it
        if (Vector3.Distance(investigationPoint, newInvestigationPoint) > 0.5f)
        {
            investigationPoint = newInvestigationPoint;
            hasReachedInvestigationPoint = false;
            Debug.Log($"[Alert] Player detected again - updating investigation point to: {investigationPoint}");
            
            MoveToInvestigationPoint();
        }
    }
    
    /// <summary>
    /// Move enemy to the investigation point and set walking animation
    /// </summary>
    private void MoveToInvestigationPoint()
    {
        if (enemy.NavMeshAgent == null) return;
        
        enemy.NavMeshAgent.isStopped = false;
        enemy.NavMeshAgent.SetDestination(investigationPoint);
        
        // Set walking animation
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.SetMovementSpeed(enemy.PatrolSpeed);
        }
    }
    
    /// <summary>
    /// Check if enemy has reached the investigation point
    /// </summary>
    private void CheckIfReachedInvestigationPoint()
    {
        if (hasReachedInvestigationPoint || enemy.NavMeshAgent == null) return;
        
        float distanceToPoint = Vector3.Distance(enemy.transform.position, investigationPoint);
        
        if (distanceToPoint <= investigationRadius)
        {
            Debug.Log("[Alert] Reached investigation point - looking around");
            hasReachedInvestigationPoint = true;
            
            StopAndLookAround();
            
            // Reset timer to start countdown now that we've reached the point
            alertTimer = 0f;
        }
    }
    
    /// <summary>
    /// Stop the enemy and set idle animation
    /// </summary>
    private void StopAndLookAround()
    {
        if (enemy.NavMeshAgent != null)
        {
            enemy.NavMeshAgent.isStopped = true;
        }
        
        // Set idle animation
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.SetMovementSpeed(0f);
        }
    }
    
    /// <summary>
    /// Handle investigation timeout and return to patrol
    /// </summary>
    /// <returns>True if transitioning to Patrol</returns>
    private bool HandleInvestigationTimeout()
    {
        if (!hasReachedInvestigationPoint) return false;
        
        alertTimer += Time.deltaTime;
        
        if (alertTimer >= alertDuration)
        {
            Debug.Log("Enemy alert timeout - player not found, resuming patrol");
            
            // Reset alert meter
            if (enemy.AlertMeter != null)
            {
                enemy.AlertMeter.ResetMeter();
            }
            
            // Return to Patrol state
            stateMachine.ChangeState(enemy.PatrolState);
            return true;
        }
        
        return false;
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Alert state");
        
        // Stop movement
        if (enemy.NavMeshAgent != null)
        {
            enemy.NavMeshAgent.isStopped = true;
        }
        
        // Note: Don't reset meter here - it should stay if transitioning to Pursue
    }
}

