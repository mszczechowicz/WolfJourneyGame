using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int LandHash = Animator.StringToHash("Land"); //TODO
    private readonly int IdleFallHash = Animator.StringToHash("IdleFall");

    private const float CrossFadeDuration = 0.2f;

    private Vector3 momentumFalling;

   

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }



    public override void Enter()
    {
        
        momentumFalling = stateMachine.CharacterController.velocity;
        momentumFalling.y = 0f;

        stateMachine.Animator.SetBool("Landing",false);

        stateMachine.Animator.CrossFadeInFixedTime(IdleFallHash, CrossFadeDuration);

        // stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        //Move(momentumFalling, deltaTime);
        //Move(stateMachine.CharacterController.velocity, deltaTime);



        
        if (stateMachine.CharacterController.isGrounded)
        {
            stateMachine.Animator.SetBool("Landing", true);


            ReturnToLocomotion();
        }


        Vector3 jumpMovement = CalculateMovementInAir();

        Move(jumpMovement * stateMachine.AirMovementSpeed, deltaTime);


        stateMachine.transform.Translate(jumpMovement * deltaTime);
        stateMachine.CharacterController.Move(jumpMovement * stateMachine.AirMovementSpeed * deltaTime);


        // FaceMovementDirectionInAir(jumpMovement, deltaTime);




       
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
