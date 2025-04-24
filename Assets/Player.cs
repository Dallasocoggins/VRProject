using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    public int healthMax = 10;
    public Image hpBar;
    public Image countdownBackground;
    public TextMeshProUGUI countdown;
    public Canvas canvas;

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
        countdown.enabled = false;
        countdownBackground.enabled = false;
        canvas = countdownBackground.GetComponentInParent<Canvas>();
        canvas.enabled = false;
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

    public void EnableCountdown(int number)
    {
        canvas.enabled = true;
        countdownBackground.enabled = true;
        countdown.enabled = true;
        StartCoroutine(SetCountDown(number));
    }

    IEnumerator SetCountDown(int number)
    {
        int i = number;
        while (i > 0)
        {
            countdown.text = "" + i;
            yield return new WaitForSeconds(1.0f);
            i--;
        }
        canvas.enabled = false;
        countdown.enabled = false;
        countdownBackground.enabled = false;
    }

}
