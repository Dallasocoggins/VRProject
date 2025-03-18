using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public GameObject projectilePrefab;
    public Transform firePoint; // Where bullets spawn
    public float shootInterval = 2f;
    public float projectileSpeed = 15f;

    private Transform playerHead; // Player's headset transform
    private float shootTimer;

    void Start()
    {
        GameObject xrHead = GameObject.FindGameObjectWithTag("MainCamera"); // Find the VR headset
        if (xrHead != null)
        {
            playerHead = xrHead.transform;
        }
    }

    void Update()
    {
        if (playerHead != null)
        {
            // Aim directly at the player's head
            transform.LookAt(playerHead);

            // Shoot at intervals
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                Shoot();
                shootTimer = 0;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has been killed!");
        Destroy(gameObject);
    }

    void Shoot()
    {
        if (projectilePrefab && firePoint)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.linearVelocity = (playerHead.position - firePoint.position).normalized * projectileSpeed; // Fire towards the headset
            }
        }
    }
}
