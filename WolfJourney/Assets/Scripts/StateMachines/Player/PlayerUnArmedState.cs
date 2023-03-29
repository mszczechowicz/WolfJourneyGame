using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnArmedState : PlayerBaseState
{
    //Stringtohash Szybsze w obliczaniu ni¿ string
    private readonly int VelocityHash = Animator.StringToHash("Velocity");


    private const float AnimatorDampTime = 0.1f;



    public PlayerUnArmedState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {

       
    }
  
    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        


        stateMachine.transform.Translate(movement * deltaTime);
        stateMachine.CharacterController.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if(stateMachine.InputHandler.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(VelocityHash, 0, AnimatorDampTime, deltaTime);
            
            return;       
        }
        stateMachine.Animator.SetFloat(VelocityHash, 1, AnimatorDampTime, deltaTime);
        //stateMachine.transform.rotation = Quaternion.LookRotation(movement);

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
