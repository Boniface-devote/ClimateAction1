using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public float spawnInterval = 2f;
    public RectTransform gamePanel;
    public float horizontalOffset = 100f; // Max offset from center in either direction

    private void Start()
    {
        if (gamePanel == null)
        {
            Debug.LogError("Game Panel not assigned in TrashSpawner!");
            return;
        }

        InvokeRepeating("SpawnTrash", 1f, spawnInterval);
    }

    void SpawnTrash()
    {
        if (gamePanel == null || trashPrefabs.Length == 0) return;

        int index = Random.Range(0, trashPrefabs.Length);

        // Calculate top center in local space
        float centerX = 0f;
        float offsetX = Random.Range(-horizontalOffset, horizontalOffset);
        float topY = gamePanel.rect.height / 2f;

        Vector3 localSpawnPos = new Vector3(centerX + offsetX, topY, 0f);

        // Instantiate and attach to panel
        GameObject trash = Instantiate(trashPrefabs[index], Vector3.zero, Quaternion.identity);
        trash.transform.SetParent(gamePanel, false);
        trash.GetComponent<RectTransform>().anchoredPosition = localSpawnPos;
    }
}
