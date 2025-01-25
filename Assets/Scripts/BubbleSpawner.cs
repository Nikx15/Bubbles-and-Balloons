using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;  // Bubble prefab to spawn
    [SerializeField] private float spawnInterval = 2f;  // Time between spawns
    [SerializeField] private Transform spawnArea;       // The rectangle area for spawning

    private float minX, maxX;  // Min and max X positions for spawning

    void Start()
    {
        // Calculate the min and max X positions based on the spawn area
        minX = spawnArea.position.x - (spawnArea.localScale.x / 2);
        maxX = spawnArea.position.x + (spawnArea.localScale.x / 2);

        StartCoroutine(SpawnBubbles());
    }

    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            SpawnBubble();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBubble()
    {
        if (bubblePrefab != null)
        {
            // Random position within the spawn area
            float randomX = Random.Range(minX, maxX);
            Vector2 spawnPosition = new Vector2(randomX, spawnArea.position.y);

            Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
