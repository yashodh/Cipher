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
        // Add idle animation trigger here
    }

    public override void Update()
    {
        base.Update();
        
        // Example: Check for movement input to transition to Move state
        // if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        // {
        //     stateMachine.ChangeState(new PlayerState_Move(stateMachine, player));
        // }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Player exited Idle state");
    }
}

