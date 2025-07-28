using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For TMP_InputField

public class MainMenu : MonoBehaviour
{
    public GameObject scoreBoardPanel;
    public GameObject HowtoPlayPanel;
    public GameObject LevelSelectorPanel;
    public GameObject SoundsettingsPanel;
    public GameObject MainPanel;
    public GameObject RegistrationPanel;

    public TMP_InputField nameInputField; // Link in Inspector

    private const string UsernameKey = "PlayerName";

    void Start()
    {
        // Check if the player is already registered
        if (PlayerPrefs.HasKey(UsernameKey))
        {
            RegistrationPanel.SetActive(false);
            MainPanel.SetActive(true);
        }
        else
        {
            RegistrationPanel.SetActive(true);
            MainPanel.SetActive(false);
        }

        // Hide all other panels at start
        scoreBoardPanel.SetActive(false);
        HowtoPlayPanel.SetActive(false);
        LevelSelectorPanel.SetActive(false);
        SoundsettingsPanel.SetActive(false);
    }

    public void RegisterPlayer()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString(UsernameKey, playerName);
            PlayerPrefs.Save();

            RegistrationPanel.SetActive(false);
            MainPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Name field is empty!");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("rubbishRushBeginnerLevel");
    }

    public void OnClickScoreBoardButton()
    {
        ShowOnly(scoreBoardPanel);
    }

    public void OnClickHowtoPlayButton()
    {
        ShowOnly(HowtoPlayPanel);
    }

    public void OnClickLevelSelectorButton()
    {
        ShowOnly(LevelSelectorPanel);
    }

    public void OnClickSoundSettingsButton()
    {
        ShowOnly(SoundsettingsPanel);
    }

    public void OnClickBackButton()
    {
        ShowOnly(MainPanel);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    private void ShowOnly(GameObject panelToShow)
    {
        scoreBoardPanel.SetActive(false);
        HowtoPlayPanel.SetActive(false);
        LevelSelectorPanel.SetActive(false);
        SoundsettingsPanel.SetActive(false);
        MainPanel.SetActive(false);

        panelToShow.SetActive(true);
    }
}
