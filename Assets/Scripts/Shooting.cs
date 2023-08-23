using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform gunEnd;
    public LayerMask layerMask;
    public GameObject bulletPrefab; // Reference to the bullet prefab.
    public float bulletSpeed = 100f; // Speed of the bullet.
    public AudioClip shotSound; // Reference to the shot sound effect.
    public float recoilDistance = 0.1f; // Distance the gun moves back during recoil.
    public float recoilDuration = 0.1f; // Duration of the recoil animation.

    private AudioSource audioSource;
    private bool isRecoiling = false; // Flag to prevent shooting during recoil animation.

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(gunEnd.position, gunEnd.forward, out hit, Mathf.Infinity, layerMask))
        {
            BearController bear = hit.collider.GetComponent<BearController>();

            if (bear != null)
            {
                Debug.Log("Hit a bear!");
                bear.OnShot();
            }
            /*else if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Hit an obstacle!");
            }*/
        }


        // Play the shot sound effect.
        if (audioSource != null && shotSound != null)
        {
            audioSource.PlayOneShot(shotSound);
        }

        if (!isRecoiling)
        {
            // Animate recoil effect.
            StartCoroutine(ShootWithRecoil());
        }

    }

    IEnumerator ShootWithRecoil()
    {
        isRecoiling = true; // Set the flag to prevent shooting during recoil animation.

        // Animate recoil effect.
        Vector3 originalPosition = gunEnd.localPosition;
        Vector3 recoilPosition = originalPosition - Vector3.forward * recoilDistance;

        float elapsedTime = 0f;
        while (elapsedTime < recoilDuration)
        {
            gunEnd.localPosition = Vector3.Lerp(originalPosition, recoilPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gunEnd.localPosition = originalPosition; // Reset position after recoil.

        // Instantiate and move the bullet.
        GameObject bullet = Instantiate(bulletPrefab, gunEnd.position, Quaternion.identity);
        bullet.transform.rotation = gunEnd.rotation;

        // Get the Bullet script component and set its bulletSpeed value.
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.bulletSpeed = bulletSpeed;
        }

        // Get the Rigidbody component of the bullet and give it a velocity.
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = gunEnd.forward * bulletSpeed;
        }

        yield return new WaitForSeconds(0.1f); // Wait a bit before allowing shooting again.
        isRecoiling = false; // Reset the flag to allow shooting.
    }
}