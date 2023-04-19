using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{

    private readonly int IdleFallHash = Animator.StringToHash("IdleFall");

    private const float CrossFadeDuration = 0.5f;


    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }



    public override void Enter()
    {


        stateMachine.Animator.CrossFadeInFixedTime(IdleFallHash, CrossFadeDuration);

    }

    public override void Tick(float deltaTime)
    {

        if (stateMachine.CharacterController.isGrounded)
        {
            ReturnToLocomotion();
        }


        Vector3 inAirMovement = CalculateMovementInAir();
        FaceMovementDirectionInAir(inAirMovement, deltaTime);

        Move(inAirMovement * stateMachine.AirMovementSpeed, deltaTime);
        stateMachine.transform.Translate(inAirMovement * deltaTime);
      


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

        if (movement != Vector3.zero)
        {

            stateMachine.transform.rotation = Quaternion.Lerp
                (stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltatime * stateMachine.RotationDamping);
        }
    }
}
