using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform[] waypoints;
    public float speed = 5f;

    private int currentWaypointIndex = 0;
    private bool movingForward = true;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned.");
            return;
        }

        transform.position = waypoints[0].position;
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        MoveTowardsWaypoint();
    }

    private void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (movingForward)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    movingForward = false;
                    currentWaypointIndex = waypoints.Length - 2;
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    movingForward = true;
                    currentWaypointIndex = 1;
                }
            }
        }
    }
}