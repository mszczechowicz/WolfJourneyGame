using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerAttackingState : PlayerBaseState
{

    private float previousFrameTime;
    private bool alreadyAppliedForce;

    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine,int attackIndex) : base(stateMachine)
    {

        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
   

        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);

        
        Debug.Log(attack.AnimationName);
    }

   

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);



        float normalizedTime = GetNormalizedTime();

        if ( normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }


         if (stateMachine.InputHandler.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            stateMachine.SwitchState(new PlayerArmedState(stateMachine));
            Debug.Log("BacktoMornalState");
        }


        previousFrameTime = normalizedTime;
    }

    

    public override void Exit()
    {
  
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));                       
    }

    private void TryApplyForce()
    {   
        if (alreadyAppliedForce)
        {
            return;
        }
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }


    private float GetNormalizedTime()
    {
       AnimatorStateInfo currentInfo =  stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack") )
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    
}
