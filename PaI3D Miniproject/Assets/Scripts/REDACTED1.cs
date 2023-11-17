using System;
using UnityEngine;

//This script has been redacted as a result of an inability to solve an issue with the OnMove() function interfering with the OnJump() function.
//This may be because of the use of the Lerp function, but I am not sure.

public class REDACTED1 : MonoBehaviour
{
    private Rigidbody rb;
    private Transform playerCamera;

    private Vector2 movementInput;
    private Vector3 currentVelocity;
    public float movementSpeed;
    public float acceleration;
    public float deceleration;
    
    public float jumpForce;
    public float coyoteTime;
    private float timeSinceGrounded;
    private bool isGrounded;

    private Vector2 mouseInput;
    public float mouseSensitivity = 1.0f;
    public float minCameraAngle = -90.0f;
    public float maxCameraAngle = 90.0f;

    private void Start()
    {
        movementSpeed = 10.0f;
        acceleration = 10.0f;
        deceleration = 10.0f;
        
        jumpForce = 50.0f;
        coyoteTime = 0.1f;

        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        OnLook();
        OnJump();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    private void OnMove()
    {
        Vector3 cameraForward = playerCamera.forward;
        cameraForward.y = 0.0f;
        cameraForward.Normalize();

        Vector3 targetVelocity = (cameraForward * movementInput.y + playerCamera.right * movementInput.x) * movementSpeed;

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * acceleration);

        if (movementInput.magnitude < 0.1f)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        }

        rb.velocity = currentVelocity;
    }

    private void OnLook()
    {
        float rotationY = transform.localEulerAngles.y + mouseInput.x * mouseSensitivity;
        transform.localRotation = Quaternion.Euler(0.0f, rotationY, 0.0f);

        float rotationX = playerCamera.localEulerAngles.x - mouseInput.y * mouseSensitivity;

        if (rotationX > 180.0f)
        {
            rotationX -= 360.0f;
        }

        rotationX = Mathf.Clamp(rotationX, minCameraAngle, maxCameraAngle);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
    
    private void OnJump()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || Time.time - timeSinceGrounded < coyoteTime))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            timeSinceGrounded = Time.time;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
