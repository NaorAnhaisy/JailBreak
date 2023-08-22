using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform gunEnd;
    public LayerMask layerMask;
    public GameObject bulletPrefab; // Reference to the bullet prefab.
    public float bulletSpeed = 10f; // Speed of the bullet.

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
            if (hit.collider.CompareTag("Bear"))
            {
                Debug.Log("Hit an enemy!");
            }
            /*else if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Hit an obstacle!");
            }*/
        }

        // Instantiate the bullet and give it a velocity.
        GameObject bullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = gunEnd.forward * bulletSpeed;
        }
    }
}