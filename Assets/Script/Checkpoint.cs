using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SetRespawnPoint(transform.position);
                Debug.Log("Checkpoint activated at: " + transform.position);
            }
        }
    }
}
