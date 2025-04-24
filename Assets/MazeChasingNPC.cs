using UnityEngine;
using UnityEngine.AI;

public class MazeChasingNPC : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    public float chaseSpeed = 3.5f;
    public float slowSpeed = 1.5f;
    public float closeRange = 0f;
    public float killDistance = 1.2f;
    public float killTimeThreshold = 2f;
    public float startDelay = 3.0f;
    public int damageAmount = 1;

    private NavMeshAgent agent;
    private float timeNearPlayer = 0f;
    private bool isFrozen = false;
    private float acceleration = 0f;
    private Vector3 frozenPosition;
    private Rigidbody rb;

    private float delayTimer = 0f;
    private bool hasStarted = false;
    private bool hasDealtDamage = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        acceleration = agent.acceleration;
        rb = GetComponent<Rigidbody>();
        delayTimer = startDelay;

        if (animator)
        {
            animator.SetBool(0, isFrozen);
        }
    }

    void Update()
    {
        if (!hasStarted)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0f)
            {
                hasStarted = true;
            }
            else
            {
                return;
            }
        }

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

            if (timeNearPlayer >= killTimeThreshold && !hasDealtDamage)
            {
                DamagePlayer();
                hasDealtDamage = true;
            }
        }
        else
        {
            timeNearPlayer = 0f;
            hasDealtDamage = false;
        }
    }

    private void DamagePlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(damageAmount);
        }
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
