using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float health = 100f;
    public GameObject projectilePrefab;
    public Transform firePoint; // Where bullets spawn
    public float shootInterval = 2f;
    public float projectileSpeed = 15f;
    public float fireSpreadRadius = 0.2f; // Adjust this to increase randomness
    public float flashDuration = 0.2f; // Duration of the flash effect
    private Color originalColor;
    private Renderer enemyRenderer;
    private bool isFlashing = false;

    private Transform playerHead;
    private float shootTimer;

    void Start()
    {
        GameObject xrHead = GameObject.FindGameObjectWithTag("MainCamera");
        if (xrHead != null)
        {
            playerHead = xrHead.transform;
        }

        // Get the Renderer component to change the material color
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color; // Store the original color
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

        // Flash the color to red
        if (!isFlashing && enemyRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (health <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        isFlashing = true;

        // Change color to red
        enemyRenderer.material.color = Color.red;

        // Wait for the specified duration
        yield return new WaitForSeconds(flashDuration);

        // Revert to the original color
        enemyRenderer.material.color = originalColor;

        isFlashing = false;
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
