using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform playerCamera;

    private Vector2 movementInput;
    private Vector3 currentVelocity;
    public float movementSpeed = 10f;
    public float acceleration = 2f;
    public float deceleration = 3f;
    
    public float jumpForce = 20f;
    public float coyoteTime = 0.3f;
    private bool isGrounded = false;
    private float lastTimeGrounded;

    private Vector2 mouseInput;
    public float mouseSensitivity = 1f;
    public float minCameraAngle = -90f;
    public float maxCameraAngle = 90f;
    private void Start()
    {
                                                                                                                            //Gets the components from the player object and assigns them to the corresponding variables.
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
                                                                                                                            //Locks the cursor to the center of the screen and makes it invisible.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
                                                                                                                            //Gets the input from the player using the Unity Input System and assigns them to the corresponding variables.
                                                                                                                            //Normalized makes sure that the player moves at the same speed in all directions.
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        OnMove();
        OnLook();
        OnJump();
    }

    private void OnMove()
    {
                                                                                                                            //Gets the forward direction of the camera and sets the y-value to 0 to make sure the player moves in the direction the camera is facing.
        Vector3 cameraForward = playerCamera.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
                                                                                                                            //Calculates the target velocity and uses Lerp to make the player accelerate and decelerate smoothly towards the target velocity.
        Vector3 targetVelocity = (cameraForward * movementInput.y + playerCamera.right * movementInput.x) * movementSpeed;
                                                                                                                            //Uses Lerp to make the player accelerate and decelerate smoothly towards the target velocity.
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);
                                                                                                                            //If the player's movement input is less than 0.1, the player will start to decelerate, lerping the current velocity towards zero.
        if (movementInput.magnitude < 0.1f)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * deceleration);
        }
                                                                                                                            //Sets the velocity of the player to the current velocity.
        rb.velocity = currentVelocity;
    }
    private void OnLook()
    {
                                                                                                                            //Calculates horizontal rotation based on the mouse input along the x-axis.
        float rotationY = transform.localEulerAngles.y + mouseInput.x * mouseSensitivity;
                                                                                                                            //Applies the horizontal rotation to the player object's local rotation.
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);
                                                                                                                            //Calculates the vertical rotation based on the mouse input along the y-axis.
        float rotationX = playerCamera.localEulerAngles.x - mouseInput.y * mouseSensitivity; 
                                                                                                                            //If the vertical rotation is greater than 180, it will be subtracted by 360 to make sure the rotation is always between -180 and 180.
                                                                                                                            //This makes sure that the player can you look up and down without the camera flipping.
                if (rotationX > 180f)
                {
                    rotationX -= 360f;
                }
                                                                                                                            //Clamps the vertical rotation between the min and max camera angle.
        rotationX = Mathf.Clamp(rotationX, minCameraAngle, maxCameraAngle);
                                                                                                                            //Applies the vertical rotation to the camera object's local rotation.
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    private void OnJump()
    {
                                                                                                                            //Checks if the "Jump" button is pressed and if the player is grounded or if the player was grounded less than the coyote time ago.
        if (Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastTimeGrounded <= coyoteTime))
        {
                                                                                                                            //Changes the velocity of the player object along the y-axis using the jump force and the velocity change force mode.
                                                                                                                            //Force mode makes sure that the player jumps the same height regardless of the framerate or the player's current velocity.
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
                                                                                                                            //Checks if the player collides with an object with the "Ground" tag.
        if (collision.gameObject.CompareTag("Ground"))
        {
                                                                                                                            //Sets the isGrounded variable to true and sets the last time grounded to the current time.
            isGrounded = true;
            lastTimeGrounded = Time.time;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
                                                                                                                            //Update the grounded status when leaving the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
