using UnityEngine;

public class Spirit : MonoBehaviour
{
    public static int totalSpirits = 0;
    public static int collectedSpirits = 0;

    private void Start()
    {
         if (Spirit.totalSpirits == 0)
            Debug.LogWarning("totalSpirits belum di-reset!");
        Spirit.totalSpirits++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collectedSpirits++;
            FindObjectOfType<SpiritUI>().UpdateSpiritIcons();
            Destroy(gameObject);
        }
    }
}
