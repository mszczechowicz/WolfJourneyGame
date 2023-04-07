using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{

    private readonly int VelocityHash = Animator.StringToHash("Velocity");

    private const float AnimatorDampTime = 0.1f;



    private readonly int JumpHash = Animator.StringToHash("Jump");

    private const float CrossFadeDuration = 0.1f;

    private Vector3 momentumJump;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentumJump = stateMachine.CharacterController.velocity * stateMachine.JumpForce;

        momentumJump.y = 0f;

       

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
       Move(momentumJump, deltaTime);
       Move(stateMachine.CharacterController.velocity, deltaTime);

        Vector3 jumpMovement = CalculateMovementInAir();

        Move(jumpMovement * stateMachine.AirMovementSpeed, deltaTime);


        stateMachine.transform.Translate(jumpMovement * deltaTime);
        stateMachine.CharacterController.Move(jumpMovement * stateMachine.AirMovementSpeed * deltaTime);


        if (stateMachine.CharacterController.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
       
    }

    private Vector3 CalculateMovementInAir()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputHandler.MovementValue.y + right * stateMachine.InputHandler.MovementValue.x;


    }

    private void FaceMovementDirectionInAir(Vector3 movement, float deltatime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltatime * stateMachine.RotationDamping);
    }
}
