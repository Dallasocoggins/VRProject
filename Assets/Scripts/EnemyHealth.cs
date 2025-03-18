using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public GameObject projectilePrefab;
    public Transform firePoint; // Where bullets spawn
    public float shootInterval = 2f;
    public float projectileSpeed = 15f;
    public float fireSpreadRadius = 0.2f; // Adjust this to increase randomness

    private Transform playerHead;
    private float shootTimer;

    void Start()
    {
        GameObject xrHead = GameObject.FindGameObjectWithTag("MainCamera");
        if (xrHead != null)
        {
            playerHead = xrHead.transform;
        }
    }

    void Update()
    {
        if (playerHead != null)
        {
            transform.LookAt(playerHead);

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
            // Generate a random offset for the bullet spawn position
            Vector3 randomOffset = new Vector3(
                Random.Range(-fireSpreadRadius, fireSpreadRadius),
                Random.Range(-fireSpreadRadius, fireSpreadRadius),
                Random.Range(-fireSpreadRadius, fireSpreadRadius) * 0.5f // Reduce spread depth
            );

            // Calculate the new randomized spawn position
            Vector3 randomFirePoint = firePoint.position + firePoint.right * randomOffset.x +
                                      firePoint.up * randomOffset.y +
                                      firePoint.forward * randomOffset.z;

            // Instantiate the bullet at the randomized fire position
            GameObject bullet = Instantiate(projectilePrefab, randomFirePoint, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb)
            {
                rb.linearVelocity = (playerHead.position - randomFirePoint).normalized * projectileSpeed;
            }
        }
    }
}
