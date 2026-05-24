using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f);

        float z = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);

        float speed = 0f;

        // ONLY FORWARD MOVEMENT
        if (Input.GetKey(KeyCode.W))
        {
            speed = 1f;
        }

        animator.SetFloat("Speed", speed);
    }
}