using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius;
    public float spawnCooldown;
    public int maxEnemies;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnCooldown);
        spawnRadius = 10f;
        spawnCooldown = 1.5f;
        maxEnemies = 25;
    }

    private void SpawnEnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            Vector3 randomPosition = Random.insideUnitSphere.normalized * spawnRadius;

            Ray ray = new Ray(transform.position + randomPosition + Vector3.up * 10f, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 spawnPosition = hit.point;
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}