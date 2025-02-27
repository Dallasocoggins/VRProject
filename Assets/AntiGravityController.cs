using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class AntiGravityController : MonoBehaviour
{
    public InputActionReference launchButton; // Assign this in the Inspector
    public float minFlyingSpeed = 0.3f; // Minimum flying speed (set in the Inspector)
    public float maxFlyingSpeed = 0.7f; // Maximum flying speed (set in the Inspector)
    public float flyingDuration = 10f; // Duration for flying before it stops automatically

    private ContinuousMoveProvider moveProvider; // Reference to the ContinuousMoveProvider
    private bool isFloating = false;
    private float flyingTimer = 0f; // Timer to track flying duration

    void Start()
    {
        moveProvider = GetComponentInChildren<ContinuousMoveProvider>(); // Get the ContinuousMoveProvider component
    }

    void Update()
    {
        if (isFloating)
        {
            // Update the flying timer
            flyingTimer += Time.deltaTime;

            // Check if the flying duration has been reached
            if (flyingTimer >= flyingDuration)
            {
                StopFloating();
            }
        }
    }

    public void StartFloating()
    {
        if (isFloating) return; // Prevent starting multiple flight sequences

        isFloating = true;
        flyingTimer = 0f; // Reset the timer

        // Randomize the flying speed between minFlyingSpeed and maxFlyingSpeed
        moveProvider.moveSpeed = Random.Range(minFlyingSpeed, maxFlyingSpeed);
        moveProvider.enableFly = true; // Enable the ContinuousMoveProvider

        Debug.Log($"Floating started! Flying speed: {moveProvider.moveSpeed}");
    }

    void StopFloating()
    {
        isFloating = false;
        moveProvider.enableFly = false; // Disable the ContinuousMoveProvider
        Debug.Log("Floating stopped!");
    }

    void OnDestroy()
    {
        // Clean up the input action
        if (launchButton != null)
        {
            launchButton.action.performed -= _ => StartFloating();
        }
    }
}