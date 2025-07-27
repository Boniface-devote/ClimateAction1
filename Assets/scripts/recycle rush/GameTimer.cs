using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameTimer : MonoBehaviour
{
    public float gameDuration = 60f;
    private float remainingTime;

    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI aiMessageText; // Add this in your Game Over panel

    private bool isGameOver = false;

    void Start()
    {
        remainingTime = gameDuration;
        Time.timeScale = 1f;
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
        Time.timeScale = 0f;
        SoundManager.Instance.PlayGameOver();
        gameOverPanel.SetActive(true);
        ShowDisposalSummary();
    }

    void ShowDisposalSummary()
    {
        var summary = DisposalTracker.Instance.GetDisposalSummary();

        string result = "Disposal Summary:\n";
        foreach (var entry in summary)
        {
            result += $"{entry.Key}: {entry.Value}\n";
        }

        if (summaryText != null)
            summaryText.text = result;

        StartCoroutine(GenerateMessageAfterDelay(summary));
    }

    IEnumerator GenerateMessageAfterDelay(Dictionary<string, int> summary)
    {
        yield return new WaitForSecondsRealtime(5f); // Wait 5 seconds

        // Step 1: Find the highest category
        var highest = summary.OrderByDescending(x => x.Value).FirstOrDefault();
        string topCategory = highest.Key;

        // Step 2: Create a prompt to send to AI
        string aiPrompt = $"Based on the summary, please create a short and simple educational message (for children aged 6–12 in Africa) that explains the importance of sorting the type of rubbish that is highest: {topCategory}.";

        // Step 3: (Placeholder) Simulate AI response — replace this with your actual AI call
        string generatedMessage = GenerateEducationalMessageMock(topCategory);

        // Step 4: Display the message
        if (aiMessageText != null)
            aiMessageText.text = generatedMessage;
    }

    // TEMPORARY MOCK - Replace this with actual AI call logic (e.g., API)
    string GenerateEducationalMessageMock(string category)
    {
        switch (category.ToLower())
        {
            case "plastic":
                return "Great job sorting plastic! Plastic can harm animals and block drains. When we recycle it, we keep our environment clean and safe!";
            case "organic":
                return "Well done on sorting organic waste! Things like food scraps can become compost to help plants grow. That means more gardens and fresh food!";
            case "glass":
                return "Awesome work with glass! Glass can be melted and reused. Recycling it saves energy and keeps sharp pieces off the ground.";
            case "paper":
                return "Paper recycling saves trees and forests. The more you sort paper, the more we protect places where animals live!";
            case "hazardous":
                return "Sorting hazardous waste like batteries helps protect people and nature. These things must go to special places to stay safe!";
            default:
                return $"You're doing a great job sorting {category} waste! Keep being an Eco Hero!";
        }
    }
}
