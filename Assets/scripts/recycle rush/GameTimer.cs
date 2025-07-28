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
        ScoreManager.Instance.ViewPrefs();
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

        var highest = summary.OrderByDescending(x => x.Value).FirstOrDefault();
        string topCategory = highest.Key;

        string prompt = $"Based on the summary, please create a short and simple educational message(48 words) for children aged 6�12 in Africa that explains the importance of sorting the type of rubbish that is highest: {topCategory}.";

        yield return StartCoroutine(SendPromptToGemini(prompt));
    }

    IEnumerator SendPromptToGemini(string prompt)
    {
        string apiKey = "AIzaSyByKlUcYPfpHElSj3Lj1B89xBs1EMv0CzQ";
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

        string jsonPayload = JsonUtility.ToJson(new GeminiRequest
        {
            contents = new GeminiContent[]
            {
            new GeminiContent
            {
                parts = new GeminiPart[]
                {
                    new GeminiPart { text = prompt }
                }
            }
            }
        });

        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.PostWwwForm(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(request.downloadHandler.text);
                string output = response?.candidates?[0]?.content?.parts?[0]?.text;

                if (!string.IsNullOrEmpty(output) && aiMessageText != null)
                {
                    aiMessageText.text = output;
                }
                else
                {
                    aiMessageText.text = "Could not generate message. Please try Again later";
                }
            }
            else
            {
                Debug.LogError("Gemini API Error: " + request.error);
                aiMessageText.text = "Failed to get AI message.AI currenty Unaccessible";
            }
        }
    }

}
[System.Serializable]
public class GeminiRequest
{
    public GeminiContent[] contents;
}

[System.Serializable]
public class GeminiContent
{
    public GeminiPart[] parts;
}

[System.Serializable]
public class GeminiPart
{
    public string text;
}

[System.Serializable]
public class GeminiResponse
{
    public GeminiCandidate[] candidates;
}

[System.Serializable]
public class GeminiCandidate
{
    public GeminiContent content;
}
