using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
//Components:
    private Rigidbody rb;
    private Camera playerCamera;
//Movement:
    public float movementSpeed;
    public float jumpForce;
    private bool isGrounded;
//Camera
    public float mouseSensitivity = 2.0f;
    public float minCameraAngle = -90.0f;
    public float maxCameraAngle = 90.0f;
//Stats, health, invincibility-frames:
    public int totalScore;  
    public int maxHealth = 100;
    public int currentHealth;
    private bool hasIFrames = false;
    private float iFrameTimer;
    private float iFrameDuration = 1.0f;
//UI Elements:
    public Canvas deathCanvas;
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI totalScoreText;
    public Canvas UI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    private void Start()
    {
//Initialize the following variables.
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        movementSpeed = 10f;
        jumpForce = 7f;
        currentHealth = maxHealth;
//Set the deathCanvas canvas state to false (making it invisible).
        deathCanvas.enabled = false;
    }
    private void Update()
    {
//Call the OnMove(), OnLook(), and OnJump() functions every frame.
        OnMove();
        OnLook();
        OnJump();
//Check if the player has i-frames.
        if (hasIFrames)
        {
//If player has i-frames, increase the iFrameTimer to the time passed since last frame (Time.deltaTime).
            iFrameTimer += Time.deltaTime;
//Check if the iFrameTimer is larger than or equal iFrameDuration, if so, set hasIFrames to false and reset iFrameTimer.
            if (iFrameTimer >= iFrameDuration)
            {
                hasIFrames = false;
                iFrameTimer = 0.0f;
            }
        }
//Use ToString() to convert the integer variables to string and update the text in scoreText and healthText.
        scoreText.text = "Score: " + totalScore.ToString();
        healthText.text = "Health: " + currentHealth.ToString();
//Check if the game is paused.
        if (Time.timeScale == 0f)
        {
//Check for any input to restart the game.
            if (Input.anyKeyDown)
            {
                RestartGame();
            }
        }
    }
//Function for controlling the player's movement using Unity's Input Manager. 
    private void OnMove()
    {
//Declare and initialize horizontalInput and verticalInput using Input.GetAxis() method for their respective axes.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
//Get the forward and right direction of the main camera in world space instead of local space.
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
//Set the y-axes of camerForward and camerRight to 0.0f so that the movement is parallel to the ground plane.
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;
//Normalize the camerForward and cameraRight vector so the movement speed is consistent regardless of where the player is looking.
//This is so that the player can walk in all direction without being affected by the camera direction while still walking in the same forward direction that the camera is looking.
        cameraForward.Normalize();
        cameraRight.Normalize();
//Calculate the movementDirection based on the player's input and camera orientation.
//The sum of the forward and horizontal movement vectors are normalized to ensure that when the player is walking diagonally, that they don't walk faster.
        Vector3 movementDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;
//Using the calculated movementDirection and the previously defined movementSpeed,, a velocity is calculated and applied to the player's rigidbody.
//Note that the vertical velocity of the player's rigidbody is retained. 
        rb.velocity = new Vector3(movementDirection.x * movementSpeed, rb.velocity.y, movementDirection.z * movementSpeed);
    }
//Function for handling the first-person camera movement.
    private void OnLook()
    {
//Locks the cursor to the center of the screen and sets the cursor's visibility to false.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
// Gets the horizontal rotation using the sum of the current horizontal rotation and horizontal mouse input, multiplied by the mouseSensitivity.
        float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
// The new horizontal rotation is then applied to the player's local transform using a rotation quaternion based on Euler angles.
// The rotational change is only applied to the horizontal axis.
        transform.localRotation = Quaternion.Euler(0.0f, rotationY, 0.0f);

//Gets the vertical rotation using the same method as the horizontal rotation.
        float rotationX = playerCamera.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity;
//Checks if the rotation is above 180.0f (degrees).
        if (rotationX > 180.0f)
        {
//Adjust the rotation to be within the range of -180.0f to 180.0f.
//This adjustment is made to maintain continous camera movement.
            rotationX -= 360.0f;
        }
//Using the Mathf.Clamp() method, the vertical rotation is locked between the predefined minimum and maximum angles.
        rotationX = Mathf.Clamp(rotationX, minCameraAngle, maxCameraAngle);
//The new vertical rotation is applied to the player's local transform using rotation quaternion based on Euler angles.
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
//Function for handling jumping.
    private void OnJump()
    {
//Checks if the player is grounded and the Jump button has been pressed.
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
//Applies an upwards force on the player's rigidbody.
//Note the ForceMode.Impulse parameter which applies the force immediately.
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//Sets the isGrounded bool to false (indicating the player has jumped).
            isGrounded = false;
        }
    }
//Functions for handling collisions with other game objects (using tags).
//Note that tags have been created in the Unity Inspector.
    private void OnCollisionEnter(Collision collision)
    {
//Checks if there is a collision with a game object tagged with "Ground".
        if (collision.gameObject.CompareTag("Ground"))
        {
//Sets the isGrounded bool to true.
            isGrounded = true;
        }
//Else check for if there is a collision with a game object tagged with "Enemy".
        else if (collision.gameObject.CompareTag("Enemy"))
        {
//Calls the TakeDamage() method with an integer argument of 25.
            TakeDamage(25);
        }
    }
//Function for handling when the player exits collision with the "Ground" tagged game object, isGrounded bool set to false.
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
//Function for handling taking damage
    private void TakeDamage(int damage)
    {
//Check that the player doesn't have i-frames.
        if (!hasIFrames)
        {
//If the player doesn't have i-frames, apply the damage integer argument to the player's current health.
            currentHealth -= damage;
//Set hasIFrames to true.
            hasIFrames = true;
//Clamp the player's current health to ensure it stays between 0 and maxHealth.
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
//Check if the player's current health is less than or equal to 0, if so, call Die() method.
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
// Function for increasing the player's health by the specified amount, clamped to not exceed the maximum health.
    public void GainHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
//Function for handling the player's death.
    private void Die()
    {
//Disable the player's UI (showing total score, health, and ammunition.)
        UI.enabled = false;
//Enable the deathCanvas (showing the death message and total score.)
        deathCanvas.enabled = true;
//Display the following text in the deathMessageText and totalScoreText.
        deathMessageText.text = "You have DIED!";
        totalScoreText.text = "Your total score was: " + totalScore.ToString();
//Set the timeScale to 0 (pausing the game).
        Time.timeScale = 0f;
    }
//Function for restarting the game.
    public void RestartGame()
    {
//Set the timeScale to 1 (unpausing the game).
        Time.timeScale = 1f;
//Use the SceneManager to reload the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
