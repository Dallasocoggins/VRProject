using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyChasingNPC : MonoBehaviour
{
    public Transform player;
    public Transform exit;
    public float switchDistance = 1.2f;
    public float delay = 10.0f;
    public string dialogue;
    public TextMeshProUGUI dialogueTextMeshPro;

    private NavMeshAgent agent;
    private Transform currentTarget;
    private bool isExiting = true;
    private bool isWaiting = false;
    private bool waitingForPlayerConfirmation = false;
    private bool isFrozen = false;
    private Vector3 frozenPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = exit;
        dialogueTextMeshPro.text = dialogue;
        isWaiting = true;

        if (player == null)
        {
            player = GameObject.FindFirstObjectByType<Player>().transform;
        }

        if (exit == null)
        {
            exit = GameObject.FindAnyObjectByType<DoorTeleport>().transform;
        }

        StartCoroutine(StartDelayCoroutine());
    }

    IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        isWaiting = false;
    }

    void Update()
    {
        if (player == null || exit == null || isWaiting || waitingForPlayerConfirmation || isFrozen) return;

        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance <= switchDistance)
        {
            StartCoroutine(SwitchAfterDelay());
        }

        agent.SetDestination(currentTarget.position);
    }

    private IEnumerator SwitchAfterDelay()
    {
        isWaiting = true;
        agent.ResetPath();

        isExiting = !isExiting;

        if (isExiting)
        {
            dialogueTextMeshPro.text = "Come on! Follow me to the exit!";
            waitingForPlayerConfirmation = true;
        }
        else
        {
            dialogueTextMeshPro.text = "";
            yield return new WaitForSeconds(delay);
        }

        currentTarget = isExiting ? exit : player;
        isWaiting = false;
    }

    // Call this from your UI Button to resume escorting to exit
    public void PlayerConfirmedFollowing()
    {
        if (!isExiting && waitingForPlayerConfirmation)
        {
            waitingForPlayerConfirmation = false;
            isWaiting = false;
            dialogueTextMeshPro.text = "";
            currentTarget = exit;
            agent.SetDestination(currentTarget.position);
        }
    }

    public void SetFrozen(bool freeze)
    {
        isFrozen = freeze;

        if (isFrozen)
        {
            frozenPosition = transform.position;
            agent.ResetPath();
        }
        else
        {
            if (currentTarget != null)
            {
                agent.SetDestination(currentTarget.position);
            }
        }
    }

    void LateUpdate()
    {
        if (isFrozen)
        {
            transform.position = frozenPosition;
        }
    }
}
