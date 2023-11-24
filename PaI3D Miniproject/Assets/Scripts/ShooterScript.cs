using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrelTransform;
    public float bulletSpeed = 50f;

    public int maxAmmo = 6;
    private int currentAmmo;
    private bool isReloading = false;
    public float reloadTime = 2f;

    public GameObject revolverObject;
    private Animator animator;

    private static readonly int ReloadTriggerHash = Animator.StringToHash("ReloadTrigger");

    private void Start()
    {
        currentAmmo = maxAmmo;
        animator = revolverObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                Reload();
            }
        }
    }

    private void Shoot()
    {
        currentAmmo--;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 shootDirection = hit.point - barrelTransform.position;
            GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<Rigidbody>().velocity = shootDirection.normalized * bulletSpeed;
        }
    }

    private void Reload()
    {
        isReloading = true;

        if (animator != null)
        {
            animator.SetTrigger(ReloadTriggerHash);
        }

        Invoke("ResetReload", reloadTime);
    }

    private void ResetReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}