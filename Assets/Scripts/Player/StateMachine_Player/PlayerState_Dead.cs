using UnityEngine;

/// <summary>
/// Dead state - player has died
/// </summary>
public class PlayerState_Dead : PlayerState
{
    public PlayerState_Dead(PlayerStateMachine stateMachine, Player player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Player entered Dead state");
        // Add death animation trigger here
        // Disable player controls
        // Play death effects
    }

    public override void Update()
    {
        base.Update();
        
        // Dead state typically doesn't transition to other states
        // unless you want to implement respawn logic
        
        // Example: Respawn after delay
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     // Respawn logic
        //     stateMachine.ChangeState(new PlayerState_Idle(stateMachine, player));
        // }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Player exited Dead state");
        // Re-enable player controls if respawning
    }
}

