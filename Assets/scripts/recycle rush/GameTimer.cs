using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float gameDuration = 60f; // 60 seconds
    private float remainingTime;

    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Start()
    {
        remainingTime = gameDuration;
        Time.timeScale = 1f; // Ensure game runs
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (remainingTime <= 0)
        {
            EndGame();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Pause game
        gameOverPanel.SetActive(true);
    }
}
