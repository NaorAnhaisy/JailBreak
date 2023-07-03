using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController controller;
    public Animator animator;
    public Transform cameraTransform;
    public float playerSpeed = 5;

    public float mouseSensivity = 3;
    Vector2 look;

    bool wasWalking = false;
    Vector3 velocity;
    float mass = 1f;
    public float jumpSpeed = 5f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Check if character is grounded in the start
        if (!controller.isGrounded)
        {
            // Cast a ray downwards to find the ground
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                // Set character position to the ground position
                transform.position = hit.point;
            }
        }
    }

    void Update()
    {
        UpdateLook();
        UpdateMovement();
        UpdateGravity();
    }

    void UpdateLook()
    {
        look.x += Input.GetAxis("Mouse X") * mouseSensivity;
        look.y += Input.GetAxis("Mouse Y") * mouseSensivity;
        look.y = Mathf.Clamp(look.y, -90, 90);
        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    void UpdateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var input = new Vector3();
        input += transform.forward * z;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += jumpSpeed;
        }
        controller.Move((input * playerSpeed + velocity) * Time.deltaTime);

        if (!wasWalking && input.magnitude > 0.1f)
        {
            wasWalking = true;
            animator.SetBool("isWalking", true);
        }
        else if (wasWalking && input.magnitude <= 0.1f)
        {
            wasWalking = false;
            animator.SetBool("isWalking", false);
        }
    }

    private void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1 : velocity.y + gravity.y;

        // Reset vertical velocity if character is on the ground
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -1f;
        }
    }
}
