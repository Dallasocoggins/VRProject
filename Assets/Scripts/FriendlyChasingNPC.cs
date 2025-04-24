using UnityEngine;
using UnityEngine.AI;

public class FriendlyChasingNPC : MonoBehaviour
{
    public Transform player;
    public Transform exit;
    public float switchDistance = 1.2f;
    public bool isExiting = true;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null || exit == null) return;

        Transform target = isExiting ? exit : player;
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= switchDistance)
        {
            isExiting = !isExiting;
        }

        agent.SetDestination(target.position);
    }
}
