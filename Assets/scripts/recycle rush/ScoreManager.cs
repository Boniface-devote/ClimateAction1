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

        StartCoroutine(UploadDataToSupabase(username, savedScore, sceneIndex));
    }

    IEnumerator UploadDataToSupabase(string username, int score, int sceneIndex)
    {
        string url = $"{SUPABASE_URL}/rest/v1/player_scores";
        string json = $"{{\"username\":\"{username}\",\"score\":{score},\"scene_index\":{sceneIndex}}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", SUPABASE_API_KEY);
        request.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");
        request.SetRequestHeader("Prefer", "return=minimal");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError($"Error uploading to Supabase: {request.error}\n{request.downloadHandler.text}");
        else
            Debug.Log("Score data uploaded to Supabase!");
    }
}
