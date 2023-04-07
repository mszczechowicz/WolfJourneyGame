using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float drag = 0.3f;
    [SerializeField] private bool IsGroundedCheck;
    
    

    [field: SerializeField] public float GravityPower { get; private set; }



    private Vector3 impact;
    private Vector3 dampingVelocity;

    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;
    private void Update()
    {
        //Widocznoœæ ISGROUNDED  w inspektorze
        IsGroundedCheck = characterController.isGrounded;

        if (verticalVelocity < 0f && characterController.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime * GravityPower;
        }
        
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    
    }
}
