using TMPro;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrelTransform;
    public float bulletSpeed = 50f;

    public int maxAmmo = 6;
    private int currentAmmo;
    private bool isReloading;
    public float reloadTime = 1.5f;

    private Animator revolverAnimator;
    private static readonly int IsReloadingHash = Animator.StringToHash("Reloading");
    
    public TextMeshProUGUI ammoText;

    private void Start()
    {
        currentAmmo = maxAmmo;
        revolverAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isReloading || Input.GetMouseButtonDown(0) && (currentAmmo > 0 || Reload()))
            Shoot();
        
        ammoText.text = "Ammunition: " + currentAmmo.ToString();

    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 shootDirection = hit.point - barrelTransform.position;
                GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.LookRotation(shootDirection));
                bullet.GetComponent<Rigidbody>().velocity = shootDirection.normalized * bulletSpeed;
            }

            currentAmmo = Mathf.Max(0, currentAmmo - 1);
        }
    }


    private bool Reload()
    {
        isReloading = true;
        revolverAnimator.SetBool(IsReloadingHash, true);
        Invoke("FinishReload", reloadTime);
        return true;
    }

    private void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        revolverAnimator.SetBool(IsReloadingHash, false);
    }
}