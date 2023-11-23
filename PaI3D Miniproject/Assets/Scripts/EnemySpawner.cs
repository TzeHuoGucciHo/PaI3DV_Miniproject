using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 10f;
    public float spawnCooldown = 3.0f;
    public int maxEnemies = 10;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnCooldown);
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