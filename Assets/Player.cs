using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        Debug.Log("Player hit! Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
