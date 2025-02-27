using UnityEngine;

public class Scrambler : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Try to find the AntiGravityController on the player
            AntiGravityController antiGravityController = other.GetComponent<AntiGravityController>();

            if (antiGravityController != null)
            {
                // Call the StartFloating method on the AntiGravityController
                antiGravityController.StartFloating();
                Debug.Log("Player entered trigger. Starting floating!");
                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogError("AntiGravityController component not found on the player!");
            }
        }
    }
}