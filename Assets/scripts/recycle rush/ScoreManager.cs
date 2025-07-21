using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public int items = 0;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemText2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoint()
    {
        score=score+10;
        items++;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
            scoreText2.text = score.ToString();
            //scoreText.color = Color.white;
            itemText.text = items.ToString();
            itemText2.text = items.ToString();
    }
}
