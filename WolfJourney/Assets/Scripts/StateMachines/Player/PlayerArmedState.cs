using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerArmedState : PlayerBaseState
{
    //Stringtohash Szybsze w obliczaniu ni¿ string
    private readonly int VelocityHash = Animator.StringToHash("Velocity");

    private const float AnimatorDampTime = 0.1f;

    



    public PlayerArmedState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
       // stateMachine.Animator.Play("PlayerArmedState");
        stateMachine.Animator.CrossFadeInFixedTime("PlayerArmedState",0.2f);
    }

    

    public override void Tick(float deltaTime)
    {

        if (stateMachine.InputHandler.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        


        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
       





        



      
        


        stateMachine.transform.Translate(movement * deltaTime);
        stateMachine.CharacterController.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

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

   


}
