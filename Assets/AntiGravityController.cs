using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.UI;
using TMPro; // Import UI namespace for displaying timer

public class AntiGravityController : MonoBehaviour
{
    public float minFlyingSpeed = 0.3f; // Minimum flying speed (set in the Inspector)
    public float maxFlyingSpeed = 0.7f; // Maximum flying speed (set in the Inspector)
    public float flyingDuration = 10f; // Duration for flying before it stops automatically
    public float momentumDecayFactor = 0.9f; // Factor to slow down velocity after release
    public float continueTimeAfterRelease = 2f; // Time to continue moving after release
    public float fallThreshold = -100f; // Y-position threshold for "falling too far" (in ft)
    public TextMeshProUGUI timerText; // UI Text component to display the timer
    public float levelTime = 300f; // Level time in seconds (e.g., 5 minutes)

    private ContinuousMoveProvider moveProvider; // Reference to the ContinuousMoveProvider
    private bool isFloating = false;
    private float flyingTimer = 0f; // Timer to track flying duration
    private Vector3 velocity; // Current flying velocity
    private float continueTimer = 0f; // Timer to track time after release
    private float gameTimer; // Game timer to complete the level

    void Start()
    {
        moveProvider = GetComponentInChildren<ContinuousMoveProvider>(); // Get the ContinuousMoveProvider component
        gameTimer = levelTime; // Initialize the game timer
    }

    void Update()
    {
        if (isFloating)
        {
            // Update the flying timer
            flyingTimer += Time.deltaTime;

            // If we are still flying after releasing the forward button, apply momentum decay
            if (continueTimer > 0)
            {
                continueTimer -= Time.deltaTime;
                velocity *= momentumDecayFactor; // Gradually decrease velocity
            }

            // If the flying duration has been reached, stop flying
            if (flyingTimer >= flyingDuration)
            {
                StopFloating();
            }
        }

        // Check if the player has fallen below the threshold
        if (transform.position.y < fallThreshold)
        {
            Die();
        }

        // Update game timer and display it on UI
        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            DisplayTimer(gameTimer);
        }
        else
        {
            Die();
        }
    }

    public void StartFloating()
    {
        if (isFloating) return; // Prevent starting multiple flight sequences

        isFloating = true;
        flyingTimer = 0f; // Reset the timer

        // Randomize the flying speed between minFlyingSpeed and maxFlyingSpeed
        float initialSpeed = Random.Range(minFlyingSpeed, maxFlyingSpeed);
        velocity = new Vector3(0, initialSpeed, 0); // Set the initial velocity for upward movement

        continueTimer = continueTimeAfterRelease; // Allow continued movement after button release
        moveProvider.enableFly = true; // Enable the ContinuousMoveProvider

        Debug.Log($"Floating started! Flying speed: {velocity.y}");
    }

    void StopFloating()
    {
        isFloating = false;
        moveProvider.enableFly = false; // Disable the ContinuousMoveProvider
        Debug.Log("Floating stopped!");
    }

    // Die function: Called if the player falls below the fall threshold
    void Die()
    {
        Debug.Log("Player has fallen too far! You died.");
        // You can implement your own logic here, such as restarting the level or showing a game over screen
        StopFloating(); // Stop floating if the player "dies"
        // Implement level restart or other logic here
    }

    // Display the game timer on the UI
    void DisplayTimer(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format the timer as MM:SS
    }

    void OnDestroy()
    {
    }
}
