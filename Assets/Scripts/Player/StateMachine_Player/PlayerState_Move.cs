using UnityEngine;

/// <summary>
/// Move state - player is moving
/// </summary>
public class PlayerState_Move : PlayerState
{
    public PlayerState_Move(PlayerStateMachine stateMachine, Player player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Player entered Move state");
        // Add movement animation trigger here
    }

    public override void Update()
    {
        base.Update();
        
        // Example: Handle movement logic
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");
        
        // Example: Transition back to Idle if no input
        // if (horizontal == 0 && vertical == 0)
        // {
        //     stateMachine.ChangeState(new PlayerState_Idle(stateMachine, player));
        // }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Handle physics-based movement here
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Player exited Move state");
    }
}

