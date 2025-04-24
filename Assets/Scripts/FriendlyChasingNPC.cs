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
    private bool isExiting = false;
    private bool isWaiting = true;
    private bool waitingForPlayerConfirmation = true;
    private bool isFrozen = false;
    private Vector3 frozenPosition;
    private bool hasArrivedAtPlayer = true;
    private Canvas dialogueCanvas;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = exit;
        isWaiting = true;
        if (player == null)
        {
            player = GameObject.FindFirstObjectByType<Player>().transform;
        }
        if (exit == null)
        {
            exit = GameObject.FindFirstObjectByType<DoorTeleport>().transform;
        }
        if (dialogueTextMeshPro != null)
        {
            dialogueTextMeshPro.text = dialogue;
            dialogueCanvas = dialogueTextMeshPro.GetComponentInParent<Canvas>();
        }
    }

    void Update()
    {
        float distance = Mathf.Abs(Vector3.Distance(transform.position, player.position));
        if (waitingForPlayerConfirmation && distance > switchDistance)
        {
            currentTarget = player;
            isExiting = false;
            hasArrivedAtPlayer = false;
            isWaiting = false;
            dialogueTextMeshPro.text = dialogue;
            dialogueCanvas.enabled = true;
            waitingForPlayerConfirmation = false;
        }

        if (player == null || exit == null || isWaiting || waitingForPlayerConfirmation || isFrozen) return;

        distance = Mathf.Abs(Vector3.Distance(transform.position, currentTarget.position));
        // Reached target
        if (distance <= switchDistance)
        {
            if (!isExiting && !hasArrivedAtPlayer)
            {
                // Reached the player - show dialogue and wait for confirmation
                hasArrivedAtPlayer = true;
                waitingForPlayerConfirmation = true;
                agent.ResetPath();

                if (dialogueTextMeshPro != null && dialogueCanvas != null)
                {
                    dialogueTextMeshPro.text = dialogue;
                    dialogueCanvas.enabled = true;
                }
                return;
            }

            // Only switch after reaching the exit and not waiting on player
            else if (isExiting && !isWaiting)
            {
                StartCoroutine(SwitchAfterDelay());
            }
        }

        // Only set destination if not waiting for player confirmation
        if (!waitingForPlayerConfirmation && !isWaiting)
        {
            agent.SetDestination(currentTarget.position);
        }
    }
    private IEnumerator SwitchAfterDelay()
    {
        isWaiting = true;
        agent.ResetPath();
        yield return new WaitForSeconds(delay);
        isExiting = false;
        currentTarget = player;
        isWaiting = false;
        hasArrivedAtPlayer = false;
    }
    public void PlayerConfirmedFollowing()
    {
        if (waitingForPlayerConfirmation)
        {
            waitingForPlayerConfirmation = false;
            isExiting = true;
            isWaiting = false;
            currentTarget = exit;

            if (dialogueTextMeshPro != null)
                dialogueTextMeshPro.text = "";
            if (dialogueCanvas != null)
                dialogueCanvas.enabled = false;

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
        else if (currentTarget != null && !waitingForPlayerConfirmation)
        {
            agent.SetDestination(currentTarget.position);
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