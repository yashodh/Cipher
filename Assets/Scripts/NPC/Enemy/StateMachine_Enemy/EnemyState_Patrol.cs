using UnityEngine;

/// <summary>
/// Patrol state - enemy is patrolling along a patrol path
/// </summary>
public class EnemyState_Patrol : EnemyState
{
    private PatrolPath patrolPath;
    private int currentNodeIndex = 0;
    private bool isReversing = false;
    private Timer waitTimer;
    private bool isWaiting = false;
    
    public EnemyState_Patrol(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
        waitTimer = new Timer(0f);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Patrol state");
        
        // Set animation state to Patrol
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.SetState(EnemyAnimationStateController.STATE_PATROL);
        }
        
        // Get patrol path from enemy
        patrolPath = enemy.PatrolPath;
        
        if (patrolPath == null || patrolPath.NodeCount == 0)
        {
            Debug.LogWarning("[EnemyState_Patrol] No patrol path assigned or patrol path is empty! Enemy will stand still.");
            // If no patrol path, just stand still and keep watching
            if (enemy.NavMeshAgent != null)
            {
                enemy.NavMeshAgent.isStopped = true;
            }
        }
        
        // Set normal detection parameters
        if (enemy.Detection != null)
        {
            enemy.Detection.SetDetectionParameters(enemy.NormalDetectionRange, enemy.NormalFieldOfView);
        }
        
        // Resume NavMeshAgent and set patrol speed (only if we have a path)
        if (patrolPath != null && patrolPath.NodeCount > 0)
        {
            if (enemy.NavMeshAgent != null)
            {
                enemy.NavMeshAgent.isStopped = false;
                enemy.NavMeshAgent.speed = enemy.PatrolSpeed;
                Debug.Log($"[Patrol] NavMeshAgent resumed - Speed: {enemy.NavMeshAgent.speed}, isStopped: {enemy.NavMeshAgent.isStopped}");
                
                // Check if agent is on NavMesh
                if (!enemy.NavMeshAgent.isOnNavMesh)
                {
                    Debug.LogError("[Patrol] NavMeshAgent is NOT on NavMesh! Make sure NavMesh is baked and enemy is on it.");
                }
            }
            else
            {
                Debug.LogError("[Patrol] NavMeshAgent is NULL!");
            }
            
            // Start at the first node
            currentNodeIndex = 0;
            isWaiting = false;
        }
    }

    public override void Update()
    {
        base.Update();
        
        if (patrolPath == null || patrolPath.NodeCount == 0) return;
        
        // Check if player is detected and alert meter is filling
        if (enemy.CanSeePlayer())
        {
            // Transition to Alert state when player first detected
            if (enemy.AlertMeter != null && enemy.AlertMeter.AlertLevel > 0f)
            {
                stateMachine.ChangeState(enemy.AlertState);
                return;
            }
        }
    
        // Get current patrol node
        PatrolNode currentNode = patrolPath.GetNode(currentNodeIndex);
        if (currentNode == null) return;
        
        // If waiting at node
        if (isWaiting)
        {
            waitTimer.Tick();
            
            // Update animation - speed 0 while waiting
            EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
            if (animController != null)
            {
                animController.SetMovementSpeed(0f);
            }
            
            if (waitTimer.IsFinished)
            {
                // Finished waiting, move to next node
                isWaiting = false;
                currentNodeIndex = patrolPath.GetNextNodeIndex(currentNodeIndex, ref isReversing);
                
                // Resume NavMesh agent
                if (enemy.NavMeshAgent != null)
                {
                    enemy.NavMeshAgent.isStopped = false;
                }
            }
            return;
        }
        
        // Move towards current node
        Vector3 targetPosition = currentNode.Position;
        Vector3 currentPosition = enemy.transform.position;
        Vector3 direction = (targetPosition - currentPosition);
        direction.y = 0f; // Keep movement on horizontal plane
        
        float distanceToNode = direction.magnitude;
        
        // Check if reached node
        if (distanceToNode < 0.1f)
        {
            // Reached node, stop and start waiting
            if (enemy.NavMeshAgent != null)
            {
                enemy.NavMeshAgent.isStopped = true;
            }
            
            isWaiting = true;
            waitTimer.Start(currentNode.WaitTime);
            
            // Look around if specified
            if (currentNode.LookAround)
            {
                // Add look around behavior here
                Debug.Log("Enemy looking around at patrol point");
            }
        }
        else
        {
            // Move towards node using NavMesh
            if (enemy.NavMeshAgent != null && enemy.NavMeshAgent.isOnNavMesh)
            {
                enemy.NavMeshAgent.isStopped = false;
                enemy.NavMeshAgent.speed = enemy.PatrolSpeed;
                enemy.NavMeshAgent.SetDestination(targetPosition);
                
                Debug.Log($"[Patrol] Moving to node {currentNodeIndex} at {targetPosition}, Distance: {distanceToNode:F2}, Velocity: {enemy.NavMeshAgent.velocity.magnitude:F2}");
                
                // Update animation with current velocity
                EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
                if (animController != null)
                {
                    float currentSpeed = enemy.NavMeshAgent.velocity.magnitude;
                    animController.SetMovementSpeed(currentSpeed);
                }
            }
            else
            {
                Debug.LogWarning("[Patrol] Cannot move - NavMeshAgent is null or not on NavMesh!");
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Patrol state");
        
        // Reset speed to 0
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.SetMovementSpeed(0f);
        }
    }
}

