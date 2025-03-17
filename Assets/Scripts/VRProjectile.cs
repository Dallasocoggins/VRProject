using UnityEngine;

public class VRProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public float damage = 50f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after X seconds
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the hit object is an enemy
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy bullet on impact
    }
}
