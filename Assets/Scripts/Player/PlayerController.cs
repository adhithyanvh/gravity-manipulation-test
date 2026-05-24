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

    [Header("Ghost Preview")]
    public GameObject gravityGhost;

    // START WITH NORMAL DOWN GRAVITY
    Vector3 gravityDirection = Vector3.down;

    // STORE SELECTED DIRECTION
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

        x = (Input.GetKey(KeyCode.D) ? 1f : 0f)
            - (Input.GetKey(KeyCode.A) ? 1f : 0f);

        z = (Input.GetKey(KeyCode.W) ? 1f : 0f)
            - (Input.GetKey(KeyCode.S) ? 1f : 0f);

        // MOVEMENT BASED ON CAMERA
        Vector3 camForward = Vector3.ProjectOnPlane(
            cameraTransform.forward,
            gravityDirection
        ).normalized;

        Vector3 camRight = Vector3.ProjectOnPlane(
            cameraTransform.right,
            gravityDirection
        ).normalized;

        Vector3 move = camForward * z + camRight * x;

        characterController.Move(move * moveSpeed * Time.deltaTime);

        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        // STOP EXTRA PUSHING WHEN GROUNDED
        if (characterController.isGrounded)
        {
            gravityVelocity = Vector3.zero;
        }

        gravityVelocity += gravityDirection * gravityValue * Time.deltaTime;

        gravityVelocity = Vector3.ClampMagnitude(gravityVelocity, 15f);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            gravityVelocity = Vector3.zero;

            // JUMP OPPOSITE TO GRAVITY
            gravityVelocity = -gravityDirection * jumpForce;
        }
    }

    void GravityInput()
    {
        // LEFT RELATIVE TO PLAYER
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedGravityDirection = -transform.right;

            ShowGravityGhost(-transform.right);
        }

        // RIGHT RELATIVE TO PLAYER
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedGravityDirection = transform.right;

            ShowGravityGhost(transform.right);
        }

        // DOWN RELATIVE TO PLAYER
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedGravityDirection = -transform.up;

            ShowGravityGhost(-transform.up);
        }

        // APPLY GRAVITY
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectedGravityDirection == gravityDirection)
            {
                return;
            }

            gravityVelocity = Vector3.zero;

            gravityDirection = selectedGravityDirection;

            RotatePlayerToGravity();

            HideGravityGhost();
        }
    }

    void RotatePlayerToGravity()
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(
                transform.up,
                -gravityDirection
            ) * transform.rotation;

        transform.rotation = targetRotation;
    }

    void ShowGravityGhost(Vector3 direction)
    {
        gravityGhost.SetActive(true);

        gravityGhost.transform.localPosition = Vector3.zero;
        gravityGhost.transform.localRotation = Quaternion.identity;

        // LEFT SIDE
        if (direction == -transform.right)
        {
            gravityGhost.transform.localPosition =
                new Vector3(-2f, 2f, 0f);

            gravityGhost.transform.localRotation =
                Quaternion.Euler(0f, 0f, 90f);
        }

        // RIGHT SIDE
        else if (direction == transform.right)
        {
            gravityGhost.transform.localPosition =
                new Vector3(2f, 2f, 0f);

            gravityGhost.transform.localRotation =
                Quaternion.Euler(0f, 0f, -90f);
        }

        // FLOOR
        else if (direction == -transform.up)
        {
            gravityGhost.transform.localPosition =
                Vector3.zero;

            gravityGhost.transform.localRotation =
                Quaternion.identity;
        }
    }
    void HideGravityGhost()
    {
        gravityGhost.SetActive(false);
    }
}