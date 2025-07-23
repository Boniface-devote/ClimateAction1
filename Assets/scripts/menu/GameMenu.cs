using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject scoreBoardPanel;
    public GameObject HowtoPlayPanel;
    public GameObject LevelSelectorPanel;
    public GameObject SoundsettingsPanel;
    public GameObject MainPanel;
   

    // Called when the Play button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene("rubbishRushBeginnerLevel");
    }
    public void OnClickScoreBoardButton()
    {
        scoreBoardPanel.SetActive(true);
        MainPanel.SetActive(false);
        HowtoPlayPanel.SetActive(false);
        LevelSelectorPanel.SetActive(false);
        SoundsettingsPanel.SetActive(false);
    }
    public void OnClickHowtoPlayButton()
    {
        scoreBoardPanel.SetActive(false);
        MainPanel.SetActive(false);
        HowtoPlayPanel.SetActive(true);
        LevelSelectorPanel.SetActive(false);
        SoundsettingsPanel.SetActive(false);
    }
    public void OnClickLevelSelectorButton()
    {
        scoreBoardPanel.SetActive(false);
        MainPanel.SetActive(false);
        HowtoPlayPanel.SetActive(false);
        LevelSelectorPanel.SetActive(true);
        SoundsettingsPanel.SetActive(false);
    }
    public void OnClickBackButton()
    {
        scoreBoardPanel.SetActive(false);
        MainPanel.SetActive(true);
        HowtoPlayPanel.SetActive(false);
        LevelSelectorPanel.SetActive(false);
        SoundsettingsPanel.SetActive(false);
    }
    public void OnClickSoundSettingsButton()
    {
        scoreBoardPanel.SetActive(false);
        MainPanel.SetActive(false);
        HowtoPlayPanel.SetActive(false);
        LevelSelectorPanel.SetActive(false);
        SoundsettingsPanel.SetActive(true);
    }

    // Called when the Exit button is clicked
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

}
