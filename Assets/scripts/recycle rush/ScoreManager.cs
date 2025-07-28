using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load previous score
        score = PlayerPrefs.GetInt(ScoreKey, 0);

        // Store current level index
        StoreSceneIndex();
    }

    public void AddPoint()
    {
        score += 10;
        items++;

        PlayerPrefs.SetInt(ScoreKey, score); // Save current score
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
        string currentScene = SceneManager.GetActiveScene().name.ToLower(); // Lowercase for safe comparison
        int levelIndex = 0;

        if (currentScene == "rubbishrushbeginnerlevel")
            levelIndex = 1;
        else if (currentScene == "rubbishrushmidlevel")
            levelIndex = 2;
        else if (currentScene == "rubbishrushexpertlevel")
            levelIndex = 3;

        PlayerPrefs.SetInt(SceneIndexKey, levelIndex);
        PlayerPrefs.Save();
    }

    public void ViewPrefs()
    {
        Debug.Log($"PlayerPrefs Saved:\n" +
                  $"Username: {PlayerPrefs.GetString(UsernameKey)}\n" +
                  $"Score: {PlayerPrefs.GetInt(ScoreKey, 0)}\n" +
                  $"Scene Index (Level): {PlayerPrefs.GetInt(SceneIndexKey, 0)}");
    }
}
