using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject scoreBoardPanel;
    public GameObject HowtoPlayPanel;
    public GameObject MainPanel;
    // Name or index of the scene to load
    [SerializeField] private string gameSceneName = "GameScene";

    // Called when the Play button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnClickScoreBoardButton()
    {
        scoreBoardPanel.SetActive(true);
        MainPanel.SetActive(false);
        HowtoPlayPanel.SetActive(false);
    }
    public void OnClickHowtoPlayButton()
    {
        scoreBoardPanel.SetActive(false);
        MainPanel.SetActive(false);
        HowtoPlayPanel.SetActive(true);
    }
    public void OnClickBackButton()
    {
        scoreBoardPanel.SetActive(false);
        MainPanel.SetActive(true);
        HowtoPlayPanel.SetActive(false);
    }

    // Called when the Exit button is clicked
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
