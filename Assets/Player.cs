using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public int healthMax = 10;
    public Image hpBar;

    private int health;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
        health = healthMax;
        if (hpBar != null)
        {
            originalColor = hpBar.color;
            UpdateHealthBar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health--;
            UpdateHealthBar();
            FlashRed();
            Destroy(other.gameObject);

            if (health <= 0)
            {
                RestartLevel();
            }
        }
    }

    void UpdateHealthBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)health / healthMax;
        }
    }

    void FlashRed()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        hpBar.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        hpBar.color = originalColor;
    }

    void RestartLevel()
    {
        Debug.Log("Player hit! Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthBar(); // Assuming this updates UI
        if (health <= 0)
        {
            RestartLevel();
        }
    }

}
