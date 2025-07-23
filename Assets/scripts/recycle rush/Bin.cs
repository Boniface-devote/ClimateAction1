using UnityEngine;
using System.Collections;

public class BinScript : MonoBehaviour
{
    public string[] acceptedTags; // Array to hold multiple accepted tags

    private void OnTriggerEnter2D(Collider2D other)
    {
        Draggable draggable = other.GetComponent<Draggable>();
        if (draggable == null) return;

        // Check if the object's tag is in the list of accepted tags
        if (IsTagAccepted(other.tag))
        {
            SoundManager.Instance.PlaySuccess();
            // Play correct disposal animation before destroying
            StartCoroutine(PlayDisposeAnimation(other.gameObject));
            ScoreManager.Instance.AddPoint();
        }
        else
        {
            SoundManager.Instance.PlayWrong();
            // Incorrect bin – return to start position
            draggable.ReturnToStart();
        }
    }

    private bool IsTagAccepted(string tagToCheck)
    {
        foreach (string tag in acceptedTags)
        {
            if (tagToCheck == tag)
                return true;
        }
        return false;
    }

    private IEnumerator PlayDisposeAnimation(GameObject obj)
    {
        float duration = 0.2f;
        Vector3 originalScale = obj.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float time = 0;
        while (time < duration)
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }
}
