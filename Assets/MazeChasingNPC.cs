using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MazeChasingNPC : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    public float chaseSpeed = 3.5f;
    public float slowSpeed = 1.5f;
    public float closeRange = 0f;
    public float killDistance = 1.2f;
    public float killTimeThreshold = 2f;

    private NavMeshAgent agent;
    private float timeNearPlayer = 0f;
    private bool isFrozen = false;
    private float acceleration = 0f;
    private Vector3 frozenPosition;
    private Rigidbody rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        acceleration = agent.acceleration;
        rb = GetComponent<Rigidbody>();

        if (animator)
        {
            animator.SetBool(0, isFrozen);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        agent.SetDestination(player.position);

        if (!isFrozen)
        {
            agent.speed = distance <= closeRange ? slowSpeed : chaseSpeed;
            agent.acceleration = acceleration;
        }
        else
        {
            transform.position = frozenPosition;
        }

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
            timeNearPlayer = 0f;
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Player caught!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetFrozen(bool freeze)
    {
        isFrozen = freeze;
        if (animator)
        {
            animator.SetBool(0, isFrozen);
        }
        if (isFrozen)
        {
            frozenPosition = transform.position;
        }
    }
}
