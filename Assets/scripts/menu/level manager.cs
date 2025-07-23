using UnityEngine;
using UnityEngine.SceneManagement;

public class levelmanager : MonoBehaviour
{
    public void PlayLevel1()
    {
        SceneManager.LoadScene("rubbishRushBeginnerLevel");
    }
    public void PlayLevel2()
    {
        SceneManager.LoadScene("rubbishRushMidLevel");
    }
    public void PlayLevel3()
    {
        SceneManager.LoadScene("rubbishRushExpertLevel");
    }
}
