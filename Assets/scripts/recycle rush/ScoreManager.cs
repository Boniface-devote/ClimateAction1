using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public int items = 0;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemText2;

    private const string ScoreKey = "PlayerScore";
    private const string SceneIndexKey = "SceneIndex";
    private const string UsernameKey = "PlayerName";

    private const string SUPABASE_URL = "https://unlbdjmgglhdvgnwhlyj.supabase.co";
    private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVubGJkam1nZ2xoZHZnbndobHlqIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTM2ODQ2MDUsImV4cCI6MjA2OTI2MDYwNX0.W5nO-hgBQoaWgKNHNnKk1tRAkjew9guPf7pEnoRwfBo";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        PlayerPrefs.DeleteKey(ScoreKey);
        score = PlayerPrefs.GetInt(ScoreKey, 0);
        StoreSceneIndex();
    }

    public void AddPoint()
    {
        score += 10;
        items++;
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
        if (scoreText2 != null)
            scoreText2.text = score.ToString();
        if (itemText != null)
            itemText.text = items.ToString();
        if (itemText2 != null)
            itemText2.text = items.ToString();
    }

    void StoreSceneIndex()
    {
        string currentScene = SceneManager.GetActiveScene().name.ToLower();
        int levelIndex = 0;

        if (currentScene == "rubbishrushbeginnerlevel") levelIndex = 1;
        else if (currentScene == "rubbishrushmidlevel") levelIndex = 2;
        else if (currentScene == "rubbishrushexpertlevel") levelIndex = 3;

        PlayerPrefs.SetInt(SceneIndexKey, levelIndex);
        PlayerPrefs.Save();
    }

    public void ViewPrefs()
    {
        string username = PlayerPrefs.GetString(UsernameKey);
        int savedScore = PlayerPrefs.GetInt(ScoreKey, 0);
        int sceneIndex = PlayerPrefs.GetInt(SceneIndexKey, 0);

        Debug.Log($"PlayerPrefs Saved:\nUsername: {username}\nScore: {savedScore}\nScene Index (Level): {sceneIndex}");

        StartCoroutine(CheckAndUpdateScore(username, savedScore, sceneIndex));
    }

    IEnumerator CheckAndUpdateScore(string username, int currentScore, int sceneIndex)
    {
        string url = $"{SUPABASE_URL}/rest/v1/player_scores?username=eq.{username}&select=score";

        UnityWebRequest getRequest = UnityWebRequest.Get(url);
        getRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);
        getRequest.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");

        yield return getRequest.SendWebRequest();

        if (getRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error fetching existing score: {getRequest.error}\n{getRequest.downloadHandler.text}");
            yield break;
        }

        int storedScore = -1;
        string response = getRequest.downloadHandler.text;
        if (!string.IsNullOrEmpty(response) && response != "[]")
        {
            // Response format: [{"score": 100}]
            string trimmed = response.Trim('[', ']');
            if (trimmed.Contains("score"))
            {
                var scoreStr = trimmed.Split(':')[1].Trim(' ', '}');
                int.TryParse(scoreStr, out storedScore);
            }
        }

        if (currentScore > storedScore)
        {
            Debug.Log($"New high score! Updating Supabase. Old: {storedScore}, New: {currentScore}");
            StartCoroutine(UpdateSupabaseScore(username, currentScore, sceneIndex));
        }
        else
        {
            Debug.Log($"No update needed. Existing score: {storedScore}, Current score: {currentScore}");
        }
    }

    IEnumerator UpdateSupabaseScore(string username, int score, int sceneIndex)
    {
        string url = $"{SUPABASE_URL}/rest/v1/player_scores?username=eq.{username}";
        string json = $"{{\"score\":{score},\"scene_index\":{sceneIndex}}}";

        UnityWebRequest patchRequest = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        patchRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        patchRequest.downloadHandler = new DownloadHandlerBuffer();

        patchRequest.SetRequestHeader("Content-Type", "application/json");
        patchRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);
        patchRequest.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");
        patchRequest.SetRequestHeader("Prefer", "return=minimal");

        yield return patchRequest.SendWebRequest();

        if (patchRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError($"Error updating score: {patchRequest.error}\n{patchRequest.downloadHandler.text}");
        else
            Debug.Log("Supabase score updated successfully.");
    }
}
