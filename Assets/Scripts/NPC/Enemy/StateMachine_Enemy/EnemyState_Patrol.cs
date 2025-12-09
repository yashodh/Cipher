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
        
        // Get patrol path from enemy
        patrolPath = enemy.PatrolPath;
        
        if (patrolPath == null || patrolPath.NodeCount == 0)
        {
            Debug.LogWarning("[EnemyState_Patrol] No patrol path assigned or patrol path is empty!");
            // Fall back to Idle state if no patrol path
            stateMachine.ChangeState(enemy.IdleState);
            return;
        }
        
        // Start at the first node
        currentNodeIndex = 0;
        isWaiting = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (patrolPath == null || patrolPath.NodeCount == 0) return;
    
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
        if (distanceToNode < 0.5f)
        {
            // Reached node, start waiting
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
            // Move towards node
            direction.Normalize();
            
            // Move enemy
            float patrolSpeed = enemy.PatrolSpeed;
            enemy.transform.position += direction * patrolSpeed * Time.deltaTime;
            
            // Update animation with patrol speed
            EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
            if (animController != null)
            {
                animController.SetMovementSpeed(patrolSpeed);
            }
            
            // Rotate to face movement direction
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(
                    enemy.transform.rotation, 
                    targetRotation, 
                    5f * Time.deltaTime
                );
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

