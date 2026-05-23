using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Gravity")]
    public float gravityValue = 9.81f;

    Vector3 gravityVelocity;

    Vector3 gravityDirection = Vector3.down;
    Vector3 selectedGravityDirection;

    public Transform cameraTransform;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        selectedGravityDirection = gravityDirection;
    }

    void Update()
    {
        Move();

        ApplyGravity();

        Jump();

        GravityInput();
    }

    void Move()
    {
        float x = 0f;
        float z = 0f;

        x = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f);

        z = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * z + camRight * x;

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,10f * Time.deltaTime);
        }

        characterController.Move(move * moveSpeed * Time.deltaTime);

        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        
        gravityVelocity += gravityDirection * gravityValue * Time.deltaTime;

        gravityVelocity = Vector3.ClampMagnitude(gravityVelocity, 15f);
        Debug.Log("Gravity Velocity =" + gravityVelocity);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            gravityVelocity = Vector3.zero;

            gravityVelocity = -gravityDirection * jumpForce;
        }
    }

    void GravityInput()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //gravityDirection = -cameraTransform.right;
            Debug.Log("gravity dir = left");
            selectedGravityDirection = -cameraTransform.right;
        }

        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //gravityDirection = cameraTransform.right;
            Debug.Log("gravity dir = right");
            selectedGravityDirection = cameraTransform.right;

        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 forwardDir = cameraTransform.forward;

            
            forwardDir.y = 0f;

            forwardDir.Normalize();

            selectedGravityDirection = forwardDir;
            Debug.Log("gravity dir = up");
        }

        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 backwardDir = -cameraTransform.forward;

            backwardDir.y = 0f;

            backwardDir.Normalize();

            selectedGravityDirection = backwardDir;
            Debug.Log("gravity dir = down");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            gravityDirection = selectedGravityDirection;
            Debug.Log("Gravity Confirmed!");
           
        }
    }
}