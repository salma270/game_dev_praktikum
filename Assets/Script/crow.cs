using UnityEngine;

public class crow : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool movingLeft = true;
    public Transform groundCheck;
    public float checkDistance = 1f;
    public LayerMask obstacleLayer;

    private void Update()
    {
        // Gerak horizontal
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Deteksi ujung/halangan pakai raycast
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, direction, checkDistance, obstacleLayer);

        if (hit.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingLeft = !movingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Vector3 dir = movingLeft ? Vector3.left : Vector3.right;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + dir * checkDistance);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 knockDir = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<PlayerMovement>()?.TakeDamage(1, knockDir);
        }
    }

}
