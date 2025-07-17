using UnityEngine;

public class plant : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 knockDirection = (collision.transform.position - transform.position).normalized;

            // Panggil fungsi TakeDamage dari script PlayerMovement
            collision.GetComponent<PlayerMovement>().TakeDamage(damage, knockDirection);
        }
    }
}
