using TMPro;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
//Components:
    public GameObject bulletPrefab;
    public Transform barrelTransform;
    private Animator revolverAnimator;
//Other:
    public float bulletSpeed;
    public int maxAmmo = 6;
    private int currentAmmo;
    private bool isReloading;
    public float reloadTime;
//Hash:
    private static readonly int IsReloadingHash = Animator.StringToHash("Reloading");
//UI:
    public TextMeshProUGUI ammoText;
    private void Start()
    {
//Initialize the following variables:
        bulletSpeed = 75f;
        reloadTime = 1.25f;
        currentAmmo = maxAmmo;
//Get the Animator component from the child game object (from the Revolver_PaI3DV game object).
        revolverAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
//Check if the player is reloading or if the player presses the left mouse button and the player has ammo or the player reloads.
        if (isReloading || Input.GetMouseButtonDown(0) && (currentAmmo > 0 || Reload()))
        {
//Call the Shoot() function.
            Shoot();
//Update the ammoText text.
            ammoText.text = "Ammunition: " + currentAmmo.ToString();
        }
    }
//Function to handle shooting.
    private void Shoot()
    {
//Check if the player has ammo.
        if (currentAmmo > 0)
        {
//Create a ray from the center of the screen.
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
//Create a RaycastHit variable to store the information of the raycast.
            RaycastHit hit;
//Check if the raycast hits anything.
            if (Physics.Raycast(ray, out hit))
            {
//Create a Vector3 variable to store the direction of the bullet.
                Vector3 shootDirection = hit.point - barrelTransform.position;
//Instantiate a bullet prefab at the barrelTransform position, with a rotation of Quaternion.LookRotation(shootDirection).
                GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.LookRotation(shootDirection));
//Set the velocity of the bullet to the shootDirection normalized (length of 1) multiplied by the bulletSpeed.
                bullet.GetComponent<Rigidbody>().velocity = shootDirection.normalized * bulletSpeed;
            }
//Decrement the currentAmmo by 1 and clamp the value between 0 and maxAmmo.
            currentAmmo = Mathf.Clamp(currentAmmo - 1, 0, maxAmmo);
        }
    }
//Function to handle reloading.
    private bool Reload()
    {
//Set isReloading to true and set the IsReloading parameter in the revolverAnimator to true.
//This will trigger the reload animation.
        isReloading = true;
        revolverAnimator.SetBool(IsReloadingHash, true);
//Invoke the FinishReload() function after reloadTime seconds.
//Invoke() is a function that calls a function after x seconds.
        Invoke("FinishReload", reloadTime);
//Return true.
        return true;
    }
//Function to handle finishing the reload.
    private void FinishReload()
    {
//Set the currentAmmo to maxAmmo and set isReloading to false and set the IsReloading parameter in the revolverAnimator to false.
//This will stop the reload animation.
        currentAmmo = maxAmmo;
        isReloading = false;
        revolverAnimator.SetBool(IsReloadingHash, false);
    }
}