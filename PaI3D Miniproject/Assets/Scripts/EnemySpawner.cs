using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemySpawner : MonoBehaviour
{
//Components:
    public GameObject enemyPrefab;
//Spawn variables:
    public float spawnRadius;
    public float spawnCooldown;
//Other:
    public int maxEnemies;

    private void Start()
    {
//Initialize the following variables:
        spawnRadius = 10f;
        spawnCooldown = 1.5f;
        maxEnemies = 25;
//Invoke the SpawnEnemy() function every spawnCooldown seconds.
//InvokeRepeating() is a function that calls a function every x seconds.
        InvokeRepeating("SpawnEnemy", 0f, spawnCooldown);
    }
//Function to handle spawning enemy prefabs.
    private void SpawnEnemy()
    {
//Check if the number of maximum enemies has been reached, if not, spawn an enemy.
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
//Generate a random position within the spawnRadius in a sphere around the spawner (empty game object).
            Vector3 randomPosition = Random.insideUnitSphere.normalized * spawnRadius;
//Create a ray from the randomPosition, with a direction of Vector3.down.
            Ray ray = new Ray(transform.position + randomPosition + Vector3.up * 10f, Vector3.down);
//Create a RaycastHit variable to store the information of the raycast.
            RaycastHit hit;
//Check if the raycast hits anything on the "Ground" layer.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
//If the raycast hits something on the "Ground" layer, store the hit position in a Vector3 variable.
                Vector3 spawnPosition = hit.point;
//Instantiate an enemy prefab at the spawnPosition, with a rotation of Quaternion.identity.
//Quaternion.identity is the same as Quaternion.Euler(0f, 0f, 0f).
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}