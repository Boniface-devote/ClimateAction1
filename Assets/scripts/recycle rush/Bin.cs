using UnityEngine;

public class BinScript : MonoBehaviour
{
    public string acceptedTag; // E.g., "Plastic", "Paper", etc.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(acceptedTag))
        {
            Destroy(other.gameObject); // Dispose of the trash
            ScoreManager.Instance.AddPoint(); // Add point to score
        }
        else if (other.CompareTag("Trash")) // generic trash item with wrong tag
        {
            // Optionally penalize or play an error sound
            Debug.Log("Wrong bin!");
        }
    }
}
