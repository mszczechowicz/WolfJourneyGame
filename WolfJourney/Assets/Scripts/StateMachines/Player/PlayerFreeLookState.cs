using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    //Stringtohash Szybsze w obliczaniu ni¿ string
    private readonly int VelocityHash = Animator.StringToHash("Velocity");
  

    private const float AnimatorDampTime = 0.1f;


    private readonly int FreeLookHash = Animator.StringToHash("PlayerFreeLookState");
 

    private const float CrossFadeDuration = 0.1f;

  



    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
    
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookHash, CrossFadeDuration);

        stateMachine.InputHandler.JumpEvent += OnJump;
        //stateMachine.InputHandler.DashEvent += OnDash;
    }

    

    public override void Tick(float deltaTime)
    {
       
        


        if (stateMachine.InputHandler.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (!stateMachine.ForceReceiver.IsGrounded)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }


        Vector3 movement = CalculateMovement();




        //Debug.DrawRay(stateMachine.transform.position, CalculateSlope(movement), Color.blue, 0.5f);
        Move(CalculateSlope(movement) * stateMachine.FreeLookMovementSpeed, deltaTime);

        
       

        if(stateMachine.InputHandler.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(VelocityHash, 0, AnimatorDampTime, deltaTime);
            
            return;       
        }
        stateMachine.Animator.SetFloat(VelocityHash, 1, AnimatorDampTime, deltaTime);

       
        FaceMovementDirection(movement, deltaTime);

        



    }
    public override void Exit()
    {
        stateMachine.InputHandler.JumpEvent -= OnJump;
        //stateMachine.InputHandler.DashEvent -= OnDash;

    }

    private Vector3 CalculateMovement()
    {
     Vector3 forward  =   stateMachine.MainCameraTransform.forward;
     Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputHandler.MovementValue.y + right * stateMachine.InputHandler.MovementValue.x;


    }
    private void FaceMovementDirection(Vector3 movement, float deltatime)
    {        
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,Quaternion.LookRotation(movement),deltatime* stateMachine.RotationDamping);            
    }

   
    

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }


    private Vector3 CalculateSlope(Vector3 movement)
    {
        return Vector3.ProjectOnPlane(movement, stateMachine.ForceReceiver.HitInfo.normal).normalized;
    }

    private void OnDash()
    {
        stateMachine.SwitchState(new PlayerDodgeState(stateMachine));

    }


}
