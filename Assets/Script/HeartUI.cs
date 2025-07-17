using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public GameObject heartPrefab;       // prefab gambar heart
    public Transform heartsContainer;    // container horizontal

    private List<GameObject> hearts = new List<GameObject>();

    public void UpdateHearts(int currentHealth)
    {
        // Hapus hearts lama
        foreach (var heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // Tambah sesuai jumlah currentHealth
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            hearts.Add(heart);
        }
    }
}
