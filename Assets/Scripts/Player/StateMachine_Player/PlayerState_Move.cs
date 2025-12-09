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
    }

    public override void Update()
    {
        base.Update();
        
        // Transition back to Idle if no input
        if (!player.IsMoving)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }
        
        // Handle movement relative to camera
        Vector2 input = player.InputVector;
        
        // Get camera forward and right directions (flatten to horizontal plane)
        Camera mainCamera = Camera.main;
        Vector3 cameraForward = Vector3.forward;
        Vector3 cameraRight = Vector3.right;
        
        if (mainCamera != null)
        {
            cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();
            
            cameraRight = mainCamera.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();
        }
        
        // Calculate movement direction relative to camera
        Vector3 moveDirection = (cameraRight * input.x + cameraForward * input.y).normalized;
        
        if (moveDirection != Vector3.zero)
        {
            // Move player
            Vector3 movement = moveDirection * player.MoveSpeed;
            player.Move(movement);
            
            // Rotate player to face movement direction
            player.RotateToDirection(moveDirection);
        }
        
        // Update animation with current speed and crouch state
        if (player.AnimationController != null)
        {
            float speed = input.magnitude * player.MoveSpeed;
            player.AnimationController.PlayMoveAnimation(speed);
            player.AnimationController.SetCrouched(player.IsCrouched);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Player exited Move state");
    }
}

