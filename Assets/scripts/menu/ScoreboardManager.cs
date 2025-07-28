using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class ScoreEntry
{
    public string username;
    public int score;
    public int scene_index;
}

public class ScoreboardManager : MonoBehaviour
{
    public GameObject rowPrefab; // Assign LeaderboardRow prefab
    public Transform contentParent; // ScrollView Content
    public GameObject scoreboardPanel;

    private const string SUPABASE_URL = "https://unlbdjmgglhdvgnwhlyj.supabase.co";
    private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVubGJkam1nZ2xoZHZnbndobHlqIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTM2ODQ2MDUsImV4cCI6MjA2OTI2MDYwNX0.W5nO-hgBQoaWgKNHNnKk1tRAkjew9guPf7pEnoRwfBo";

    public void ShowScoreboard()
    {
        Debug.Log("ShowScoreboard() called.");
        scoreboardPanel.SetActive(true);
        StartCoroutine(CheckSupabaseConnection());
    }

    private IEnumerator CheckSupabaseConnection()
    {
        string testUrl = $"{SUPABASE_URL}/rest/v1/player_scores?select=id&limit=1";

        UnityWebRequest testRequest = UnityWebRequest.Get(testUrl);
        testRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);
        testRequest.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");

        yield return testRequest.SendWebRequest();

        if (testRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(" Supabase connection failed: " + testRequest.error);
        }
        else
        {
            Debug.Log(" Connected to Supabase table.");
            FetchLeaderboard();
        }
    }

    public void FetchLeaderboard()
    {
        Debug.Log("FetchLeaderboard() called.");
        StartCoroutine(FetchLeaderboardCoroutine());
    }

    private IEnumerator FetchLeaderboardCoroutine()
    {
        Debug.Log("FetchLeaderboardCoroutine() called.");
        string url = $"{SUPABASE_URL}/rest/v1/player_scores?select=username,score,scene_index&order=score.desc";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", SUPABASE_API_KEY);
        request.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch leaderboard: " + request.error);
        }
        else
        {
            string json = "{\"scores\":" + request.downloadHandler.text + "}"; // Wrap array
            ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);

            PopulateUI(wrapper.scores);
        }
        Debug.Log("Raw JSON from Supabase: " + request.downloadHandler.text);

    }

    void PopulateUI(List<ScoreEntry> scores)
    {
        // Clear existing children
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentParent);
            var texts = row.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = (i + 1).ToString(); // Rank
            texts[1].text = scores[i].username;
            texts[2].text = scores[i].score.ToString();
            texts[3].text = GetLevelName(scores[i].scene_index);
        }
    }

    string GetLevelName(int index)
    {
        switch (index)
        {
            case 1: return "Beginner";
            case 2: return "Mid";
            case 3: return "Expert";
            default: return "N/A";
        }
    }

    [System.Serializable]
    public class ScoreListWrapper
    {
        public List<ScoreEntry> scores;
    }
}
