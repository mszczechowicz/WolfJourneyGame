using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{

    private readonly int DodgeHash = Animator.StringToHash("Dodge");

    private const float CrossFadeDuration = 0.1f;

    public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    

    public override void Enter()
    {
     
        stateMachine.Animator.CrossFadeInFixedTime(DodgeHash, CrossFadeDuration);

    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        Vector3 dashDirection = CalculateDodgeDirection();








        ReturnToLocomotion();
    }

    public override void Exit()
    {
       
    }
    private Vector3 CalculateDodgeDirection()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputHandler.MovementValue.y + right * stateMachine.InputHandler.MovementValue.x;


    }



}
