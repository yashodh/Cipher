using UnityEngine;

/// <summary>
/// Idle state - player is standing still
/// </summary>
public class PlayerState_Idle : PlayerState
{
    public PlayerState_Idle(PlayerStateMachine stateMachine, Player player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Player entered Idle state");
        
        // Trigger idle animation
        if (player.AnimationController != null)
        {
            player.AnimationController.PlayIdleAnimation();
        }
    }

    public override void Update()
    {
        base.Update();
        
        // Decelerate speed while idle (works for both standing and crouched)
        player.UpdateSpeed(false);
        
        // Update crouch state and animation
        if (player.AnimationController != null)
        {
            player.AnimationController.SetCrouched(player.IsCrouched);
            
            // Update animation speed to show deceleration
            if (player.CurrentSpeed > 0.1f)
            {
                player.AnimationController.PlayMoveAnimation(player.CurrentSpeed);
            }
            else
            {
                player.AnimationController.PlayIdleAnimation();
            }
        }
        
        // Check for movement input to transition to Move state
        if (player.IsMoving)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Player exited Idle state");
    }
}

