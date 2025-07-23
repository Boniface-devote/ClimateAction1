using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip successClip;
    public AudioClip wrongClip;
    public AudioClip gameOverClip;
    public AudioClip gameMusicClip;

    public AudioSource audioSource;

    private void Start()
    {
        //audioSource = gameMusicClip;
        audioSource.PlayOneShot(gameMusicClip);
        audioSource.loop = true;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySuccess()
    {
        audioSource.PlayOneShot(successClip);
    }

    public void PlayWrong()
    {
        audioSource.PlayOneShot(wrongClip);
    }

    public void PlayGameOver()
    {
        audioSource.PlayOneShot(gameOverClip);
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

}
