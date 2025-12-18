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
        
        // Set animation state to Dead
        EnemyAnimationStateController animController = enemy.GetComponent<EnemyAnimationStateController>();
        if (animController != null)
        {
            animController.SetState(EnemyAnimationStateController.STATE_DEAD);
            animController.PlayDeathAnimation();
        }
        
        // Disable NavMesh agent
        if (enemy.NavMeshAgent != null)
        {
            enemy.NavMeshAgent.isStopped = true;
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Enemy exited Dead state");
    }
}

