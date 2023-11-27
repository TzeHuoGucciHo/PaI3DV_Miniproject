using UnityEngine;

public class BulletScript : MonoBehaviour
{
//Function to handle bullet collision.
    private void OnCollisionEnter(Collision collision)
    {
//Check if the bullet collides with a game object tagged with "Enemy".
        if (collision.gameObject.CompareTag("Enemy"))
        {
//Destroy the game object the bullet collided with.
            Destroy(collision.gameObject);
//Find the game object with the "Player" tag.
            GameObject playerObject = GameObject.FindWithTag("Player");
//Get the PlayerController component from the playerObject.
//PlayerController is a script that is attached to the playerObject.
            PlayerController playerController = playerObject.GetComponent<PlayerController>();
//Call the GainHealth() method from the PlayerController component, with an integer argument of 10.
            playerController.GainHealth(10);
//Increment the player's total score by 1.
            playerController.totalScore++;
//Destroy this game object.
            Destroy(gameObject);
        }
//Check if the bullet collides with a game object tagged with "Ground" or "Wall".
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
//Destroy this game object.
            Destroy(gameObject);
        }
    }
}