using UnityEngine;

/// <summary>
/// Idle state - enemy is waiting before starting patrol
/// </summary>
public class EnemyState_Idle : EnemyState
{
    private Timer idleTimer;
    private const float IDLE_DURATION = 5f;
    
    public EnemyState_Idle(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
        idleTimer = new Timer(IDLE_DURATION);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy entered Idle state");
        idleTimer.Start();
        
        // Set idle animation (speed = 0)
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.PlayIdleAnimation();
        }
    }

    public override void Update()
    {
        base.Update();
        
        // Update idle timer
        idleTimer.Tick();
        
        // After timer completes, check if patrol path exists and transition
        if (idleTimer.IsFinished)
        {
            if (enemy.PatrolPath != null && enemy.PatrolPath.NodeCount > 0)
            {
                Debug.Log("Enemy idle time complete - starting patrol");
                stateMachine.ChangeState(enemy.PatrolState);
                return;
            }
        }
        
        // Example: Check for player detection to transition to Alert state
        // if (enemy.DetectPlayer())
        // {
        //     stateMachine.ChangeState(enemy.AlertState);
        // }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Idle state");
    }
}

