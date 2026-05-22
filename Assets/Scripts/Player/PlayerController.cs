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
    public float gravityValue = -9.81f;
    public float jumpHeight = 2f;

    Vector3 velocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        Move();

        ApplyGravity();

        Jump();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * moveSpeed * Time.deltaTime);

        characterController.Move(velocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravityValue * Time.deltaTime;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            velocity.y = jumpForce;
        }
    }
}