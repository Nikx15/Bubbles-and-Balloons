using UnityEngine;

public class BubbleCheckTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            ScoreManager.instance.AddScore(1);  // Add 1 to score when a bubble exits
            Destroy(other.gameObject);          // Remove the bubble
        }
    }
}
