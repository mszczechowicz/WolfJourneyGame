using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float timer;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {

       
    }
  
    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement.x = stateMachine.InputHandler.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputHandler.MovementValue.y;
        stateMachine.transform.Translate(movement * deltaTime);
        stateMachine.CharacterController.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if(stateMachine.InputHandler.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat("Velocity", 0, 0.1f, deltaTime);
            
            return;       
        }
        stateMachine.Animator.SetFloat("Velocity", 1, 0.1f, deltaTime);
        stateMachine.transform.rotation = Quaternion.LookRotation(movement);
    }
    public override void Exit()
    {
       
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerDashState(stateMachine));
    }


}
