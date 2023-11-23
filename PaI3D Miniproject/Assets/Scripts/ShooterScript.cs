using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrelTransform;
    public float bulletSpeed = 50f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 shootDirection = hit.point - barrelTransform.position;

                Vector3 spawnPoint = barrelTransform.position + shootDirection * 0.1f;

                GameObject bullet = Instantiate(bulletPrefab, spawnPoint, Quaternion.LookRotation(shootDirection));
                bullet.GetComponent<Rigidbody>().velocity = shootDirection.normalized * bulletSpeed;
            }
        }
    }
}