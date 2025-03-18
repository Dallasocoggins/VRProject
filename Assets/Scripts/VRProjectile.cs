using UnityEngine;
using UnityEngine.SceneManagement;

public class VRProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public float damage = 50f;
    public float impactForce = 500f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after X seconds
    }

    void OnCollisionEnter(Collision collision)
    {
        // Apply force if hitting a physics object
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            Vector3 forceDirection = collision.contacts[0].normal * -1;
            rb.AddForce(forceDirection * impactForce, ForceMode.Impulse);
        }

        // Check if hitting the player
        if (collision.gameObject.CompareTag("Player"))
        {
            RestartLevel();
        }

        // Check if hitting an enemy
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy bullet on impact
    }

    void RestartLevel()
    {
        Debug.Log("Player hit! Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
