using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MazeChasingNPC : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 3.5f;
    public float slowSpeed = 1.5f;
    public float closeRange = 5f;
    public float killDistance = 1.2f;
    public float killTimeThreshold = 2f;

    private NavMeshAgent agent;
    private float timeNearPlayer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        agent.SetDestination(player.position);

        // Adjust speed based on distance
        agent.speed = distance <= closeRange ? slowSpeed : chaseSpeed;

        // Check if within kill range
        if (distance <= killDistance)
        {
            timeNearPlayer += Time.deltaTime;

            if (timeNearPlayer >= killTimeThreshold)
            {
                KillPlayer();
            }
        }
        else
        {
            timeNearPlayer = 0f; // Reset timer if player escapes
        }
    }

    void KillPlayer()
    {
        // You can add animations or effects here
        Debug.Log("Player caught!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
