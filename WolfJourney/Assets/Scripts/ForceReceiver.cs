using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float drag = 0.3f;
    [SerializeField] private bool isGroundedCheck;
    [SerializeField] private Transform GroundCheckLocation;
    [SerializeField] private float RayDistance = 5f;
    [SerializeField] private LayerMask layers;
    [SerializeField] private float GroundCheckDistanceTolerance = 0.2f;
    [SerializeField] private float sphereRadius = 0.5f;
    private RaycastHit hitInfo;
    [field: SerializeField] public float GravityPower { get; private set; }



    private Vector3 impact;
    private Vector3 dampingVelocity;


   
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    public RaycastHit HitInfo { get => hitInfo; set => hitInfo = value; }
    public bool IsGrounded { get => isGroundedCheck; }

    private void Update()
    {
        //Widocznoœæ ISGROUNDED  w inspektorze
        //IsGroundedCheck = characterController.isGrounded;

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

    private void FixedUpdate()
    {
       
        isGroundedCheck = IsGroundedCheck();
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    
    }

    private float currenRayDistance;
    private bool IsGroundedCheck()
    {
        if (Physics.SphereCast(GroundCheckLocation.position, sphereRadius, Vector3.down, out hitInfo, RayDistance, layers))
        {


            Debug.DrawRay(GroundCheckLocation.position, Vector3.down * RayDistance, Color.red);

            currenRayDistance = hitInfo.distance;

            bool result = Mathf.Abs(GroundCheckLocation.position.y - HitInfo.point.y) < GroundCheckDistanceTolerance;
            return result;
        }
        return false;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GroundCheckLocation.position, hitInfo.point);
        Gizmos.DrawWireSphere(GroundCheckLocation.position + Vector3.down * currenRayDistance, sphereRadius);
    }

}
