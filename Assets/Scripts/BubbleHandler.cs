using UnityEngine;

public class BubbleHandler : MonoBehaviour
{
    private Rigidbody2D rb;
    private float originalGravityScale;
    [SerializeField] private float Speed = 0.65f;

    void Update()
    {
        transform.position += Vector3.left * Speed * Time.deltaTime;

        Destroy(gameObject, 60f);
    }

    public void OnKicked(Vector2 force)
    {
        if (rb != null)
        {
            rb.gravityScale = 0f;  // Disable gravity when kicked
            rb.velocity = Vector2.zero; // Reset velocity to ensure consistent kicking
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    public void ResetGravity()
    {
        if (rb != null)
        {
            rb.gravityScale = originalGravityScale;  // Restore original gravity
        }
    }
}
