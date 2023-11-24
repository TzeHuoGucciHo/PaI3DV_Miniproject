using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera playerCamera;
    public float movementSpeed;
    public float jumpForce;
    private bool isGrounded;
    public float mouseSensitivity = 2.0f;
    public float minCameraAngle = -90.0f;
    public float maxCameraAngle = 90.0f;
    public int maxHealth = 100;
    public int currentHealth;
    private bool hasIFrames = false;
    private float iFrameTimer = 0.0f;
    private float iFrameDuration = 1.0f;
    public int totalScore;
    
    public Canvas deathCanvas;
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI totalScoreText;

    public Canvas UI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        movementSpeed = 10f;
        jumpForce = 7f;
        currentHealth = maxHealth;
        deathCanvas.enabled = false;
    }

    private void Update()
    {
        OnMove();
        OnLook();
        OnJump();
        
        if (hasIFrames)
        {
            iFrameTimer += Time.deltaTime;
            
 
            if (iFrameTimer >= iFrameDuration)
            {
                hasIFrames = false;
                iFrameTimer = 0.0f;
            }
        }
        
        scoreText.text = "Score: " + totalScore.ToString();
        healthText.text = "Health: " + currentHealth.ToString();
        
    }
    
    private void OnMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Vector3 movementDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;
        rb.velocity = new Vector3(movementDirection.x * movementSpeed, rb.velocity.y, movementDirection.z * movementSpeed);
    }

    private void OnLook()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localRotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
        float rotationX = playerCamera.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity;
        if (rotationX > 180.0f)
        {
            rotationX -= 360.0f;
        }
        rotationX = Mathf.Clamp(rotationX, minCameraAngle, maxCameraAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }

    private void OnJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(25);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void TakeDamage(int damage)
    {
        if (!hasIFrames)
        {
            currentHealth -= damage;
            hasIFrames = true;
            
            currentHealth = Mathf.Max(0, currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        UI.enabled = false;
        deathCanvas.enabled = true;
        deathMessageText.text = "You have DIED!";
        totalScoreText.text = "Your total score was: " + totalScore.ToString();
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
