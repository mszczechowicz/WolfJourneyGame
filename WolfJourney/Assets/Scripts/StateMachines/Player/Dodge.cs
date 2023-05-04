using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : PlayerStateMachine
{
  
    PlayerStateMachine stateMachine;
    public float dashSpeed;
    public float dashTime;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }
    IEnumerator Dash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            stateMachine.CharacterController.Move(stateMachine.ForceReceiver.Movement * dashSpeed * Time.deltaTime);
            yield return null;

        }
    }
}
